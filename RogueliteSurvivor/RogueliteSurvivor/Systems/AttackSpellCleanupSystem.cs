using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class AttackSpellCleanupSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription projectileQuery = new QueryDescription()
                                            .WithAll<Projectile>();
        QueryDescription singleTargetQuery = new QueryDescription()
                                            .WithAll<SingleTarget>();

        public AttackSpellCleanupSystem(World world)
            : base(world, new QueryDescription())
        { }

        public void Update(GameTime gameTime, float totalElapsedTime) 
        {
            
            world.Query(in projectileQuery, (in Entity entity, ref Projectile projectile) =>
            {
                if(projectile.State == EntityState.Dead)
                {
                    world.TryDestroy(entity);
                }
            });

            world.Query(in singleTargetQuery, (in Entity entity, ref SingleTarget single) =>
            {
                if (single.State == EntityState.Dead)
                {
                    world.TryDestroy(entity);
                }
            });
        }
    }
}
