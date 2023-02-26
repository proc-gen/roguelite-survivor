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
    public class AttackSpellCleanupSystem : ArchSystem, IUpdateSystem
    {
        public AttackSpellCleanupSystem(World world)
            : base(world, new QueryDescription()
                                .WithAny<Projectile, SingleTarget>())
        { }

        public void Update(GameTime gameTime, float totalElapsedTime) 
        {
            
            world.Query(in query, (in Entity entity, ref EntityStatus entityStatus) =>
            {
                if(entityStatus.State == State.Dead)
                {
                    world.Destroy(entity);
                }
            });
        }
    }
}
