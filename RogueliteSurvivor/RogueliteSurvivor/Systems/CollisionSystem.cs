﻿using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;

namespace RogueliteSurvivor.Systems
{
    public class CollisionSystem : ArchSystem, IUpdateSystem
    {
        Box2D.NetStandard.Dynamics.World.World physicsWorld;
        QueryDescription singleTargetQuery = new QueryDescription()
                                                    .WithAll<SingleTarget, Body>();
        public CollisionSystem(World world, Box2D.NetStandard.Dynamics.World.World physicsWorld)
            : base(world, new QueryDescription()
                                .WithAll<Position, Velocity, Body>())
        {
            this.physicsWorld = physicsWorld;
        }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            world.Query(in query, (in Entity entity, ref Position pos, ref Velocity vel, ref Body body) =>
            {
                if (float.IsNaN(vel.VectorPhysics.X) || float.IsNaN(vel.VectorPhysics.Y))
                {
                    vel.Vector = Vector2.Zero;
                }
                else if (entity.Has<Slow>())
                {
                    vel.Vector *= 0.5f;
                }
                else if (entity.Has<Shock>())
                {
                    vel.Vector = Vector2.Zero;
                }
                body.SetLinearVelocity(vel.VectorPhysics / PhysicsConstants.PhysicsToPixelsRatio);
            });

            world.Query(in singleTargetQuery, (ref SingleTarget single, ref Body body) =>
            {
                single.DamageStartDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                single.DamageEndDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (single.DamageStartDelay < 0 && !body.IsAwake())
                {
                    body.SetAwake(true);
                }
                else if (single.DamageEndDelay < 0 && body.IsAwake())
                {
                    body.SetAwake(false);
                }
            });

            physicsWorld.Step(1 / 60f, 8, 3);

            world.Query(in query, (ref Position pos, ref Velocity vel, ref Body body) =>
            {
                var position = body.GetPosition();
                pos.XY = new Vector2(position.X, position.Y) * PhysicsConstants.PhysicsToPixelsRatio;
            });

            physicsWorld.ClearForces();
        }
    }
}
