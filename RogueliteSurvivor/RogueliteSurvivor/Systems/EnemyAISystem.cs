using Arch.Core;
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
        QueryDescription playerQuery = new QueryDescription()
                                            .WithAll<Player, Position>();
        public EnemyAISystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Enemy, Position, Velocity, Speed>())
        { }

        public void Update(GameTime gameTime) 
        {
            world.Query(in query, (ref Position pos, ref Velocity vel, ref Speed sp) =>
            {
                Position? player = null;
                Vector2 enemyPos = pos.XY;
                world.Query(in playerQuery, (ref Position playerPos) =>
                {
                    if (!player.HasValue)
                    {
                        player = playerPos;
                    }
                    else
                    {
                        if(Vector2.DistanceSquared(enemyPos, playerPos.XY) < Vector2.DistanceSquared(enemyPos, player.Value.XY))
                        {
                            player = playerPos;
                        }
                    }
                });

                if (player.HasValue)
                {
                    vel.Vector = Vector2.Normalize(player.Value.XY - enemyPos) * sp.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            });
        }
    }
}
