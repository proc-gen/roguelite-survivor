using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;

namespace RogueliteSurvivor.Systems
{
    public class EnemyAISystem : ArchSystem, IUpdateSystem
    {
        QueryDescription mapQuery = new QueryDescription()
                                            .WithAll<MapInfo>();
        public EnemyAISystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Enemy, Position, Velocity, Speed, Target>())
        { }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            MapInfo map = null;
            world.Query(in mapQuery, (ref MapInfo mapInfo) =>
            {
                if (map == null)
                {
                    map = mapInfo;
                }
            });

            world.Query(in query, (ref Position pos, ref Velocity vel, ref Speed sp, ref Target target) =>
            {
                vel.Vector = Vector2.Normalize(target.TargetPosition - pos.XY);

                if(!map.IsTileWalkable((int)(pos.XY.X + vel.Vector.X), (int)(pos.XY.Y + vel.Vector.Y)))
                {
                    Vector2 clockwise = new Vector2(vel.Vector.Y, -vel.Vector.X);
                    Vector2 counterClockwise = new Vector2(-vel.Vector.Y, vel.Vector.X);

                    if(Vector2.Distance(pos.XY + clockwise, target.TargetPosition) > Vector2.Distance(pos.XY + counterClockwise, target.TargetPosition))
                    {
                        vel.Vector = counterClockwise;
                    }
                    else
                    {
                        vel.Vector = clockwise;
                    }
                }

                vel.Vector *= sp.speed;
            });
        }
    }
}
