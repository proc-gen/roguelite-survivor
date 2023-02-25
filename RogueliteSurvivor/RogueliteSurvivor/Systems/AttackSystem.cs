using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.ComponentFactories;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class AttackSystem : ArchSystem, IUpdateSystem
    {
        Dictionary<string, Texture2D> textures;
        Box2D.NetStandard.Dynamics.World.World physicsWorld;
        Random random;
        Dictionary<Spells, SpellContainer> spellContainers;

        public AttackSystem(World world, Dictionary<string, Texture2D> textures, Box2D.NetStandard.Dynamics.World.World physicsWorld, Dictionary<Spells, SpellContainer> spellContainers)
            : base(world, new QueryDescription()
                                .WithAll<Target>()
                                .WithAny<Spell1, Spell2>())
        {
            this.textures = textures;
            this.physicsWorld = physicsWorld;
            this.spellContainers = spellContainers;
            random = new Random();
        }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            world.Query(in query, (in Entity entity, ref Position pos, ref Target target) =>
            {
                if (entity.IsAlive())
                {
                    if (entity.TryGet(out Spell1 spell1))
                    {
                        spell1.Cooldown = processSpell(gameTime, entity, pos, target, spell1);
                        entity.Set(spell1);
                    }
                    if (entity.TryGet(out Spell2 spell2))
                    {
                        spell2.Cooldown = processSpell(gameTime, entity, pos, target, spell2);
                        entity.Set(spell2);
                    }
                }
            });
        }

        private float processSpell(GameTime gameTime, Entity entity, Position pos, Target target, ISpell spell)
        {
            spell.Cooldown += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            if (spell.Type != SpellType.None
                    && spell.Cooldown > spell.CurrentAttackSpeed
                    && target.Entity.IsAlive()
                    && target.Entity.Has<Position>())
            {
                spell.Cooldown -= spell.CurrentAttackSpeed;

                SpellEffects effect = SpellEffects.None;
                if (spell.Effect != SpellEffects.None)
                {
                    if (random.Next(1000) < (spell.CurrentEffectChance * 1000))
                    {
                        effect = spell.Effect;
                    }
                }

                if (spell.Type == SpellType.Projectile)
                {
                    createProjectile(entity, spell, target, pos, effect);
                }
                else if (spell.Type == SpellType.SingleTarget)
                {
                    createSingleTarget(entity, spell, target, pos, effect);
                }
            }

            return spell.Cooldown;
        }

        private void createProjectile(Entity entity, ISpell spell, Target target, Position pos, SpellEffects effect)
        {
            var projectile = world.Create<Projectile, Position, Velocity, Speed, Animation, SpriteSheet, Damage, Owner, Body>();

            var body = new BodyDef();
            var targetPosition = target.Entity.Get<Position>().XY;
            var velocityVector = Vector2.Normalize(targetPosition - pos.XY);
            var position = pos.XY + velocityVector;
            body.position = new System.Numerics.Vector2(position.X, position.Y) / PhysicsConstants.PhysicsToPixelsRatio;
            body.fixedRotation = true;

            projectile.SetRange(
                new Projectile() { State = EntityState.Alive },
                new Position() { XY = new Vector2(body.position.X, body.position.Y) },
                new Velocity() { Vector = velocityVector * spell.CurrentProjectileSpeed },
                new Speed() { speed = spell.CurrentProjectileSpeed },
                SpellFactory.GetSpellAliveAnimation(spellContainers[spell.Spell]),
                SpellFactory.GetSpellAliveSpriteSheet(textures, spellContainers[spell.Spell], pos.XY, targetPosition),
                new Damage() { Amount = spell.CurrentDamage, BaseAmount = spell.CurrentDamage, SpellEffect = effect },
                new Owner() { Entity = entity },
                BodyFactory.CreateCircularBody(projectile, 16, physicsWorld, body, .1f)
            );
        }

        private void createSingleTarget(Entity entity, ISpell spell, Target target, Position pos, SpellEffects effect)
        {
            var singleTarget = world.Create<SingleTarget, Position, Speed, Animation, SpriteSheet, Damage, Owner, Body>();

            var body = new BodyDef();
            var targetPosition = target.Entity.Get<Position>().XY;
            body.position = new System.Numerics.Vector2(targetPosition.X, targetPosition.Y) / PhysicsConstants.PhysicsToPixelsRatio;
            body.fixedRotation = true;

            singleTarget.SetRange(
                SpellFactory.CreateSingleTarget(spellContainers[spell.Spell]),
                new Position() { XY = new Vector2(targetPosition.X, targetPosition.Y) },
                new Speed() { speed = spell.CurrentProjectileSpeed },
                SpellFactory.GetSpellAliveAnimation(spellContainers[spell.Spell]),
                SpellFactory.GetSpellAliveSpriteSheet(textures, spellContainers[spell.Spell], pos.XY, targetPosition),
                new Damage() { Amount = spell.CurrentDamage, BaseAmount = spell.CurrentDamage, SpellEffect = effect },
                new Owner() { Entity = entity },
                BodyFactory.CreateCircularBody(singleTarget, 32, physicsWorld, body, .1f, false)
            );
        }
    }
}
