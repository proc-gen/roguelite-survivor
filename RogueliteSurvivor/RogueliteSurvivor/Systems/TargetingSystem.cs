using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;

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
                if (entity.IsAlive())
                {
                    target.TargetPosition = findTarget(entity.Has<Enemy>() ? playerQuery : enemyQuery, pos.XY);
                }
            });
        }

        private Vector2 findTarget(QueryDescription targetQuery, Vector2 sourcePosition)
        {
            Vector2 targetPos = new Vector2(9999, 9999);
            world.Query(in targetQuery, (in Entity other, ref Position otherPos) =>
            {
                if (other.IsAlive() && (!other.Has<Enemy>() || other.Get<EntityStatus>().State != Constants.State.Dead))
                {
                    if (Vector2.Distance(sourcePosition, otherPos.XY) < Vector2.Distance(sourcePosition, targetPos))
                    {
                        targetPos = otherPos.XY;
                    }
                }
            });

            return targetPos;
        }
    }
}
