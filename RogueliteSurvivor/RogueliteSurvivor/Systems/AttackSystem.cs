using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.ComponentFactories;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Physics;
using System;
using System.Collections.Generic;

namespace RogueliteSurvivor.Systems
{
    public class AttackSystem : ArchSystem, IUpdateSystem
    {
        Dictionary<string, Texture2D> textures;
        Box2D.NetStandard.Dynamics.World.World physicsWorld;
        Random random;
        Dictionary<Spells, SpellContainer> spellContainers;

        QueryDescription spell1Query = new QueryDescription()
                                .WithAll<Target, Spell1>();

        QueryDescription spell2Query = new QueryDescription()
                                .WithAll<Target, Spell2>();



        public AttackSystem(World world, Dictionary<string, Texture2D> textures, Box2D.NetStandard.Dynamics.World.World physicsWorld, Dictionary<Spells, SpellContainer> spellContainers)
            : base(world, new QueryDescription())
        {
            this.textures = textures;
            this.physicsWorld = physicsWorld;
            this.spellContainers = spellContainers;
            random = new Random();
        }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            world.Query(in spell1Query, (in Entity entity, ref Position pos, ref Target target, ref Spell1 spell1) =>
            {
                spell1.Cooldown = processSpell(gameTime, entity, pos, target, spell1);
            });

            world.Query(in spell2Query, (in Entity entity, ref Position pos, ref Target target, ref Spell2 spell2) =>
            {
                spell2.Cooldown = processSpell(gameTime, entity, pos, target, spell2);
            });
        }

        private float processSpell(GameTime gameTime, Entity entity, Position pos, Target target, ISpell spell)
        {
            spell.Cooldown += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            if (spell.Type != SpellType.None
                    && spell.Cooldown > spell.CurrentAttackSpeed)
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
            var projectile = world.Create<Projectile, EntityStatus, Position, Velocity, Speed, Animation, SpriteSheet, Damage, Owner, Pierce, Body>();

            var body = new BodyDef();
            var velocityVector = Vector2.Normalize(target.TargetPosition - pos.XY);
            var position = pos.XY + velocityVector;
            body.position = new System.Numerics.Vector2(position.X, position.Y) / PhysicsConstants.PhysicsToPixelsRatio;
            body.fixedRotation = true;

            projectile.Set(
                new Projectile(),
                new EntityStatus(),
                new Position() { XY = new Vector2(position.X, position.Y) },
                new Velocity() { Vector = velocityVector * spell.CurrentProjectileSpeed },
                new Speed() { speed = spell.CurrentProjectileSpeed },
                SpellFactory.GetSpellAliveAnimation(spellContainers[spell.Spell]),
                SpellFactory.GetSpellAliveSpriteSheet(textures, spellContainers[spell.Spell], pos.XY, target.TargetPosition),
                new Damage() { Amount = spell.CurrentDamage, BaseAmount = spell.CurrentDamage, SpellEffect = effect },
                new Owner() { Entity = entity },
                new Pierce(entity.Has<Pierce>() ? entity.Get<Pierce>().Num : 0),
                BodyFactory.CreateCircularBody(projectile, 16, physicsWorld, body, .1f)
            );
        }

        private void createSingleTarget(Entity entity, ISpell spell, Target target, Position pos, SpellEffects effect)
        {
            var singleTarget = world.Create<SingleTarget, EntityStatus, Position, Speed, Animation, SpriteSheet, Damage, Owner, Body>();

            var body = new BodyDef();
            body.position = new System.Numerics.Vector2(target.TargetPosition.X, target.TargetPosition.Y) / PhysicsConstants.PhysicsToPixelsRatio;
            body.fixedRotation = true;

            float radiusMultiplier = entity.Has<AreaOfEffect>() ? entity.Get<AreaOfEffect>().Radius : 1f;

            singleTarget.Set(
                SpellFactory.CreateSingleTarget(spellContainers[spell.Spell]),
                new EntityStatus(),
                new Position() { XY = target.TargetPosition },
                new Speed() { speed = spell.CurrentProjectileSpeed },
                SpellFactory.GetSpellAliveAnimation(spellContainers[spell.Spell]),
                SpellFactory.GetSpellAliveSpriteSheet(textures, spellContainers[spell.Spell], pos.XY, target.TargetPosition, radiusMultiplier),
                new Damage() { Amount = spell.CurrentDamage, BaseAmount = spell.CurrentDamage, SpellEffect = effect },
                new Owner() { Entity = entity },
                BodyFactory.CreateCircularBody(singleTarget, (int)(32 * radiusMultiplier), physicsWorld, body, .1f, false)
            );
        }
    }
}
