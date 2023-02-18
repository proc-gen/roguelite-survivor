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
    public class TargetingSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription playerQuery = new QueryDescription()
                                            .WithAll<Player>();
        QueryDescription enemyQuery = new QueryDescription()
                                            .WithAll<Enemy>();
        public TargetingSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Position, Velocity, Speed, Target>()
                                .WithAny<Player, Enemy>())
        { }

        public void Update(GameTime gameTime, float totalElapsedTime) 
        {
            world.Query(in query, (in Entity entity, ref Position pos, ref Target target) =>
            {
                Entity? targetEntity = findTarget(entity.Has<Enemy>() ? playerQuery : enemyQuery, pos.XY);

                if(targetEntity.HasValue)
                {
                    target.Entity = targetEntity.Value;
                }                
            });
        }

        private Entity? findTarget(QueryDescription targetQuery, Vector2 sourcePosition)
        {
            Entity? target = null;
            Vector2 targetPos = new Vector2(9999, 9999);
            world.Query(in targetQuery, (in Entity other, ref Position otherPos) =>
            {
                if (!target.HasValue)
                {
                    targetPos = otherPos.XY;
                    target = other;
                }
                else
                {
                    if (Vector2.Distance(sourcePosition, otherPos.XY) < Vector2.Distance(sourcePosition, targetPos))
                    {
                        targetPos = otherPos.XY;
                        target = other;
                    }
                }
            });

            return target;
        }
    }
}
