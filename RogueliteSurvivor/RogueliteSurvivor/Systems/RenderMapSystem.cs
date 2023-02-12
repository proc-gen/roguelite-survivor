using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace RogueliteSurvivor.Systems
{
    public class RenderMapSystem : ArchSystem, IRenderSystem
    {
        public RenderMapSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<MapInfo>())
        {
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player)
        {
            Vector2 playerPosition = player.Get<Position>().XY;

            world.Query(in query, (ref MapInfo map) =>
            {
                var tileLayers = map.Map.Layers.Where(x => x.type == TiledLayerType.TileLayer);

                foreach (var layer in tileLayers)
                {
                    int minY, maxY, minX, maxX;
                    minY = (int)MathF.Max(playerPosition.Y / 16f - 6, 0);
                    maxY = (int)MathF.Min(playerPosition.Y / 16f + 7, layer.height);
                    minX = (int)MathF.Max(playerPosition.X / 16f - 9, 0);
                    maxX = (int)MathF.Min(playerPosition.X / 16f + 10, layer.width);

                    for (var y = minY; y < maxY; y++)
                    {
                        for (var x = minX; x < maxX; x++)
                        {
                            var index = (y * layer.width) + x;
                            var gid = layer.data[index];
                            var tileX = x * map.Map.TileWidth;
                            var tileY = y * map.Map.TileHeight;

                            if (gid == 0)
                            {
                                continue;
                            }

                            var mapTileset = map.Map.GetTiledMapTileset(gid);

                            var tileset = map.Tilesets[mapTileset.firstgid];

                            var rect = map.Map.GetSourceRect(mapTileset, tileset, gid);

                            var source = new Rectangle(rect.x, rect.y, rect.width, rect.height);
                            var destination = new Rectangle(tileX, tileY, map.Map.TileWidth, map.Map.TileHeight);

                            SpriteEffects effects = SpriteEffects.None;
                            double rotation = 0f;

                            spriteBatch.Draw(textures["tiles"], new Vector2(tileX + 125, tileY + 75), source, Color.White, (float)rotation, playerPosition, 1f, effects, 0);
                        }
                    }
                }
            });
        }
    }
}
