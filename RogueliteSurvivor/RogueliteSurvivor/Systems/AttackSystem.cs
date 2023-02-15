using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
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
                                .WithAll<Spell, Target, AttackSpeed>())
        {
            this.textures = textures;
            this.physicsWorld = physicsWorld;
        }

        public void Update(GameTime gameTime)
        {
            world.Query(in query, (ref Position pos, ref Spell spell, ref Target target, ref AttackSpeed attackSpeed) =>
            {
                attackSpeed.Cooldown += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                if (attackSpeed.Cooldown > attackSpeed.CurrentAttackSpeed 
                        && target.Entity.Has<Position>())
                {
                    attackSpeed.Cooldown -= attackSpeed.CurrentAttackSpeed;

                    var body = new BodyDef();
                    var targetPosition = target.Entity.Get<Position>().XY;
                    var velocityVector = Vector2.Normalize(targetPosition - pos.XY);
                    var position = pos.XY + velocityVector;
                    body.position = new System.Numerics.Vector2(position.X, position.Y);
                    body.fixedRotation = true;

                    var entity = world.Create(
                        new Projectile() { State = ProjectileState.Alive },
                        new Position() { XY = new Vector2(body.position.X, body.position.Y) },
                        new Velocity() { Vector = velocityVector * 32000f * (float)gameTime.ElapsedGameTime.TotalSeconds },
                        new Speed() { speed = 32000f },
                        new Animation(0, 59, 1/60f, 1),
                        new SpriteSheet(textures[spell.CurrentSpell.ToString()], spell.CurrentSpell.ToString(), 60, 1, MathF.Atan2(targetPosition.Y - pos.XY.Y, targetPosition.X - pos.XY.X), 0.5f),
                        new Collider(16, 16, physicsWorld, body)
                        );

                    entity.Get<Collider>().SetEntityForPhysics(entity);
                }
            });
        }
    }
}
