using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class ProjectileCleanupSystem : ArchSystem, IUpdateSystem
    {
        Box2D.NetStandard.Dynamics.World.World physicsWorld;

        public ProjectileCleanupSystem(World world, Box2D.NetStandard.Dynamics.World.World physicsWorld)
            : base(world, new QueryDescription()
                                .WithAll<Projectile>())
        { 
            this.physicsWorld = physicsWorld;
        }

        public void Update(GameTime gameTime) 
        {
            
            world.Query(in query, (in Entity entity, ref Projectile projectile, ref Body body) =>
            {
                if(projectile.State == EntityState.Dead)
                {
                    physicsWorld.DestroyBody(body);
                    world.Destroy(entity);
                }
            });
        }
    }
}
