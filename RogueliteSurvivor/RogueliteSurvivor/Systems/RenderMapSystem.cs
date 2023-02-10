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
            world.Query(in query, (ref MapInfo map) =>
            {
                var tileLayers = map.Map.Layers.Where(x => x.type == TiledLayerType.TileLayer);

                foreach (var layer in tileLayers)
                {
                    for (var y = 0; y < layer.height; y++)
                    {
                        for (var x = 0; x < layer.width; x++)
                        {
                            var index = (y * layer.width) + x; // Assuming the default render order is used which is from right to bottom
                            var gid = layer.data[index]; // The tileset tile index
                            var tileX = x * map.Map.TileWidth;
                            var tileY = y * map.Map.TileHeight;

                            // Gid 0 is used to tell there is no tile set
                            if (gid == 0)
                            {
                                continue;
                            }

                            // Helper method to fetch the right TieldMapTileset instance
                            // This is a connection object Tiled uses for linking the correct tileset to the gid value using the firstgid property
                            var mapTileset = map.Map.GetTiledMapTileset(gid);

                            // Retrieve the actual tileset based on the firstgid property of the connection object we retrieved just now
                            var tileset = map.Tilesets[mapTileset.firstgid];

                            // Use the connection object as well as the tileset to figure out the source rectangle
                            var rect = map.Map.GetSourceRect(mapTileset, tileset, gid);

                            // Create destination and source rectangles
                            var source = new Rectangle(rect.x, rect.y, rect.width, rect.height);
                            var destination = new Rectangle(tileX, tileY, map.Map.TileWidth, map.Map.TileHeight);

                            SpriteEffects effects = SpriteEffects.None;
                            double rotation = 0f;

                            // Render sprite at position tileX, tileY using the rect
                            spriteBatch.Draw(textures["tiles"], new Vector2(tileX, tileY), source, Color.White, (float)rotation, player.Get<Position>().XY, 1f, effects, 0);
                        }
                    }
                }
            });
        }
    }
}
