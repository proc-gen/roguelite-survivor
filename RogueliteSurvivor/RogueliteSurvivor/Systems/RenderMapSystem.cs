using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledCS;

namespace RogueliteSurvivor.Systems
{
    public class RenderMapSystem : ArchSystem, IRenderSystem
    {
        GraphicsDeviceManager graphics;
        public RenderMapSystem(World world, GraphicsDeviceManager graphics)
            : base(world, new QueryDescription()
                                .WithAll<MapInfo>())
        {
            this.graphics = graphics;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player, float totalElapsedTime, GameState gameState, int layer)
        {
            Vector2 playerPosition = player.Get<Position>().XY;
            Vector2 offset = new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight / 6);

            world.Query(in query, (ref MapInfo map) =>
            {
                var tileLayer = map.Map.Layers.Where(x => x.type == TiledLayerType.TileLayer && x.id == layer).FirstOrDefault();

                if (tileLayer != null)
                {
                    int minY, maxY, minX, maxX;
                    minY = (int)MathF.Max((playerPosition.Y - graphics.PreferredBackBufferHeight / 2) / 16f, 0);
                    maxY = (int)MathF.Min((playerPosition.Y + graphics.PreferredBackBufferHeight / 2) / 16f, tileLayer.height);
                    minX = (int)MathF.Max((playerPosition.X - graphics.PreferredBackBufferWidth / 2) / 16f, 0);
                    maxX = (int)MathF.Min((playerPosition.X + graphics.PreferredBackBufferWidth / 2) / 16f, tileLayer.width);

                    for (var y = minY; y < maxY; y++)
                    {
                        for (var x = minX; x < maxX; x++)
                        {
                            var index = (y * tileLayer.width) + x;
                            var gid = tileLayer.data[index];
                            var tileX = x * map.Map.TileWidth;
                            var tileY = y * map.Map.TileHeight;

                            if (gid == 0)
                            {
                                continue;
                            }

                            var mapTileset = map.Map.GetTiledMapTileset(gid);

                            var tileset = map.Tilesets[mapTileset.firstgid];
                            string path = tileset.Image.source.Replace(".png", "").ToLower();

                            var rect = map.Map.GetSourceRect(mapTileset, tileset, gid);

                            var source = new Rectangle(rect.x, rect.y, rect.width, rect.height);
                            var destination = new Rectangle(tileX, tileY, map.Map.TileWidth, map.Map.TileHeight);

                            spriteBatch.Draw(textures[path], new Vector2(tileX + offset.X, tileY + offset.Y), source, Color.White, 0f, playerPosition, 1f, SpriteEffects.None, .05f * layer);
                        }
                    }
                }
            });
        }
    }
}
