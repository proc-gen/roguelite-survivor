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

        public AttackSystem(World world, Dictionary<string, Texture2D> textures, Box2D.NetStandard.Dynamics.World.World physicsWorld)
            : base(world, new QueryDescription()
                                .WithAll<Target, AttackSpeed>())
        {
            this.textures = textures;
            this.physicsWorld = physicsWorld;
        }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            world.Query(in query, (in Entity entity, ref Position pos, ref Target target, ref AttackSpeed attackSpeed) =>
            {
                attackSpeed.Cooldown += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                if (entity.TryGet(out Spell spell))
                {
                    if (attackSpeed.Cooldown > attackSpeed.CurrentAttackSpeed
                            && target.Entity.IsAlive() && target.Entity.Has<Position>())
                    {
                        attackSpeed.Cooldown -= attackSpeed.CurrentAttackSpeed;

                        var body = new BodyDef();
                        var targetPosition = target.Entity.Get<Position>().XY;
                        var velocityVector = Vector2.Normalize(targetPosition - pos.XY);
                        var position = pos.XY + velocityVector;
                        body.position = new System.Numerics.Vector2(position.X, position.Y);
                        body.fixedRotation = true;

                        var projectile = world.Create<Projectile, Position, Velocity, Speed, Animation, SpriteSheet, Damage, Owner, Body>();

                        projectile.SetRange(
                            new Projectile() { State = EntityState.Alive },
                            new Position() { XY = new Vector2(body.position.X, body.position.Y) },
                            new Velocity() { Vector = velocityVector * spell.CurrentProjectileSpeed},
                            new Speed() { speed = spell.CurrentProjectileSpeed },
                            getProjectileAnimation(spell.CurrentSpell),
                            getProjectileSpriteSheet(spell.CurrentSpell, pos.XY, targetPosition),
                            new Damage() { Amount = spell.CurrentDamage, BaseAmount = spell.CurrentDamage },
                            new Owner() { Entity = entity },
                            BodyFactory.CreateCircularBody(projectile, 16, physicsWorld, body, .1f)
                        );
                    }
                }
            });
        }

        private Animation getProjectileAnimation(AvailableSpells currentSpell)
        {
            Animation? animation = null;
            switch (currentSpell)
            {
                case AvailableSpells.SmallFireball:
                    animation = new Animation(0, 59, .1f, 1);
                    break;
                case AvailableSpells.MediumFireball:
                    animation = new Animation(0, 59, .1f, 1);
                    break;
                case AvailableSpells.LargeFireball:
                    animation = new Animation(0, 59, .1f, 1);
                    break;
                case AvailableSpells.IceShard:
                    animation = new Animation(0, 9, .1f, 1);
                    break;
                case AvailableSpells.LightningBlast:
                    animation = new Animation(0, 4, .1f, 1);
                    break;
            }
            return animation.Value;
        }

        private SpriteSheet getProjectileSpriteSheet(AvailableSpells currentSpell, Vector2 currentPosition, Vector2 targetPosition)
        {
            SpriteSheet? spriteSheet = null;
            switch (currentSpell)
            {
                case AvailableSpells.SmallFireball:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 60, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
                case AvailableSpells.MediumFireball:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 60, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
                case AvailableSpells.LargeFireball:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 60, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
                case AvailableSpells.IceShard:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 10, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
                case AvailableSpells.LightningBlast:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 5, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
            }
            return spriteSheet.Value;
        }
    }
}
