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
            : base(world, new QueryDescription())
        { }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            world.Query(in playerQuery, (ref EntityStatus status, ref Position pos, ref Target target) =>
            {
                if (status.State == Constants.State.Alive)
                {
                    target.TargetPosition = findTarget(enemyQuery, pos.XY);
                }
            });

            world.Query(in enemyQuery, (ref EntityStatus status, ref Position pos, ref Target target) =>
            {
                if (status.State == Constants.State.Alive)
                {
                    target.TargetPosition = findTarget(playerQuery, pos.XY);
                }
            });
        }

        private Vector2 findTarget(QueryDescription targetQuery, Vector2 sourcePosition)
        {
            Vector2 targetPos = new Vector2(9999, 9999);
            world.Query(in targetQuery, (ref EntityStatus status, ref Position otherPos) =>
            {
                if (status.State == Constants.State.Alive)
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
