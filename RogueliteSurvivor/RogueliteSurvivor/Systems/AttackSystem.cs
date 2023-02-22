using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
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

        public AttackSystem(World world, Dictionary<string, Texture2D> textures, Box2D.NetStandard.Dynamics.World.World physicsWorld)
            : base(world, new QueryDescription()
                                .WithAll<Target, Spell>())
        {
            this.textures = textures;
            this.physicsWorld = physicsWorld;
            random = new Random();
        }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            world.Query(in query, (in Entity entity, ref Position pos, ref Target target, ref Spell spell) =>
            {
                spell.Cooldown += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                if (spell.BaseProjectileSpeed > 0 
                        && spell.Cooldown > spell.CurrentAttackSpeed
                        && target.Entity.IsAlive() 
                        && target.Entity.Has<Position>())
                {
                    spell.Cooldown -= spell.CurrentAttackSpeed;

                    var body = new BodyDef();
                    var targetPosition = target.Entity.Get<Position>().XY;
                    var velocityVector = Vector2.Normalize(targetPosition - pos.XY);
                    var position = pos.XY + velocityVector;
                    body.position = new System.Numerics.Vector2(position.X, position.Y) / PhysicsConstants.PhysicsToPixelsRatio;
                    body.fixedRotation = true;

                    var projectile = world.Create<Projectile, Position, Velocity, Speed, Animation, SpriteSheet, Damage, Owner, Body>();

                    SpellEffects projectileEffect = SpellEffects.None;
                    if(spell.Effect != SpellEffects.None)
                    {
                        if(random.Next(1000) < (spell.CurrentEffectChance * 1000))
                        {
                            projectileEffect = spell.Effect;
                        }
                    }

                    projectile.SetRange(
                        new Projectile() { State = EntityState.Alive },
                        new Position() { XY = new Vector2(body.position.X, body.position.Y) },
                        new Velocity() { Vector = velocityVector * spell.CurrentProjectileSpeed },
                        new Speed() { speed = spell.CurrentProjectileSpeed },
                        getProjectileAnimation(spell.CurrentSpell),
                        getProjectileSpriteSheet(spell.CurrentSpell, pos.XY, targetPosition),
                        new Damage() { Amount = spell.CurrentDamage, BaseAmount = spell.CurrentDamage, SpellEffect = projectileEffect },
                        new Owner() { Entity = entity },
                        BodyFactory.CreateCircularBody(projectile, 16, physicsWorld, body, .1f)
                    );
                }
                
            });
        }

        private Animation getProjectileAnimation(Spells currentSpell)
        {
            Animation? animation = null;
            switch (currentSpell)
            {
                case Spells.Fireball:
                    animation = new Animation(0, 3, .1f, 1);
                    break;
                case Spells.IceShard:
                    animation = new Animation(0, 9, .1f, 1);
                    break;
                case Spells.LightningBlast:
                    animation = new Animation(0, 4, .1f, 1);
                    break;
            }
            return animation.Value;
        }

        private SpriteSheet getProjectileSpriteSheet(Spells currentSpell, Vector2 currentPosition, Vector2 targetPosition)
        {
            SpriteSheet? spriteSheet = null;
            switch (currentSpell)
            {
                case Spells.Fireball:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 4, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
                case Spells.IceShard:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 10, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
                case Spells.LightningBlast:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 5, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
            }
            return spriteSheet.Value;
        }
    }
}
