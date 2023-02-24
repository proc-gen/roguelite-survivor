using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class EnemyAISystem : ArchSystem, IUpdateSystem
    {
        public EnemyAISystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Enemy, Position, Velocity, Speed, Target>())
        { }

        public void Update(GameTime gameTime, float totalElapsedTime) 
        {
            world.Query(in query, (in Entity entity, ref Position pos, ref Velocity vel, ref Speed sp, ref Target target) =>
            {
                if (entity.IsAlive())
                {
                    if (target.Entity.Has<Position>())
                    {
                        var targetPosition = target.Entity.Get<Position>();
                        vel.Vector = Vector2.Normalize(targetPosition.XY - pos.XY) * sp.speed;
                    }
                }
            });
        }
    }
}
