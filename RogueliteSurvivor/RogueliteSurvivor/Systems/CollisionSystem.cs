using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace RogueliteSurvivor.Systems
{
    public class CollisionSystem : ArchSystem, IUpdateSystem
    {
        Box2D.NetStandard.Dynamics.World.World physicsWorld;
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
                body.SetLinearVelocity(vel.VectorPhysics);
            });

            physicsWorld.Step(1/60f, 8, 3);

            world.Query(in query, (in Entity entity, ref Position pos, ref Velocity vel, ref Body body) =>
            {
                var position = body.GetPosition();
                pos.XY = new Vector2(position.X, position.Y);
            });
        }
    }
}
