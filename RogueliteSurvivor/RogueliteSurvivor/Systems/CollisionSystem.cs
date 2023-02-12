using Arch.Core;
using Arch.Core.Extensions;
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
        QueryDescription mapQuery;
        public CollisionSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Position, Velocity, Collider>())
        {
            mapQuery = new QueryDescription().WithAll<MapInfo>();
        }

        public static Vector2[] TestVectors = new Vector2[3] { Vector2.One, Vector2.UnitX, Vector2.UnitY };

        public void Update(GameTime gameTime)
        {
            MapInfo map = null;
            world.Query(in mapQuery, (ref MapInfo mapInfo) =>
            {
                map = mapInfo;
            });

            if(map != null)
            {
                var tileLayers = map.Map.Layers.Where(x => x.type == TiledLayerType.TileLayer).ToArray();

                world.Query(in query, (ref Position pos, ref Velocity vel, ref Collider col) => 
                {
                    if (vel.Direction != Vector2.Zero)
                    {
                        bool canMove = false;
                        int testVectorIndex = 0;

                        do
                        {
                            int tileLayerIndex = 0;
                            do
                            {
                                TiledTile tileA = getTile(map, tileLayers[tileLayerIndex], (pos.XY + col.Offset + (vel.Direction * TestVectors[testVectorIndex] * vel.Speed)));
                                TiledTile tileB = getTile(map, tileLayers[tileLayerIndex], (pos.XY - col.Offset + (vel.Direction * TestVectors[testVectorIndex] * vel.Speed)));

                                if (tileA.properties[0].value == "true" && tileB.properties[0].value == "true")
                                {
                                    canMove = true;
                                    vel.Direction = vel.Direction * TestVectors[testVectorIndex];
                                }

                                tileLayerIndex++;
                            } while (!canMove && tileLayerIndex < tileLayers.Count());
                            testVectorIndex++;
                        } while (!canMove && testVectorIndex < 3);

                        
                        if(canMove)
                        {
                            Vector2 XY = pos.XY, Direction = vel.Direction, Offset = col.Offset;
                            float speed = vel.Speed;

                            world.Query(in query, (in Entity otherEntity, ref Position otherPos, ref Velocity otherVel, ref Collider otherCol) =>
                            {
                                if (XY != otherPos.XY && Vector2.DistanceSquared(XY, otherPos.XY) < 512)
                                {
                                    if(Vector2.DistanceSquared(XY + Offset + Direction * speed, otherPos.XY) < 192
                                        || Vector2.DistanceSquared(XY - Offset + Direction * speed, otherPos.XY) < 192)
                                    {
                                        if (otherEntity.Has<Player>())
                                        {
                                            canMove = false;
                                        }
                                        else
                                        {
                                            otherVel.Direction = Vector2.Normalize(Direction + otherVel.Direction);
                                        }
                                    }
                                }
                            });
                        }

                        if (!canMove)
                        {
                            vel.Direction = Vector2.Zero;
                        }
                    }
                });
            }
        }

        private TiledTile getTile(MapInfo map, TiledLayer layer, Vector2 position)
        {
            int x = (int)MathF.Round(position.X / map.Map.TileWidth);
            int y = (int)MathF.Round(position.Y / map.Map.TileHeight);

            int index = (y * layer.width) + x;
            int gid = layer.data[index];

            var mapTileset = map.Map.GetTiledMapTileset(gid);
            var tileset = map.Tilesets[mapTileset.firstgid];

            return map.Map.GetTiledTile(mapTileset, tileset, gid);
        }
    }
}
