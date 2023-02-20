using Arch.Core;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace RogueliteSurvivor.Components
{
    public class MapInfo
    {
        public TiledMap Map { get; set; }
        public Dictionary<int, TiledTileset> Tilesets { get; set; }

        public MapInfo(string mapPath, string tilesetPath, Box2D.NetStandard.Dynamics.World.World physicsWorld, Entity mapEntity) 
        {
            Map = new TiledMap(mapPath);
            Tilesets = Map.GetTiledTilesets(tilesetPath);

            var tileLayers = Map.Layers.Where(x => x.type == TiledLayerType.TileLayer);

            foreach (var layer in tileLayers)
            {
                for (int y = 0; y < layer.height; y++)
                {
                    for (int x = 0; x < layer.width; x++)
                    {
                        var tile = getTile(layer, x, y);

                        if (tile.properties[0].value == "false")
                        {
                            int tileX = x * Map.TileWidth + Map.TileWidth / 2;
                            int tileY = y * Map.TileHeight + Map.TileHeight / 2;

                            var body = new BodyDef();
                            body.position = new System.Numerics.Vector2(tileX, tileY) / PhysicsConstants.PhysicsToPixelsRatio;
                            body.fixedRotation = true;
                            body.type = BodyType.Static;


                            var bodyShape = new Box2D.NetStandard.Dynamics.Fixtures.FixtureDef();
                            bodyShape.shape = new PolygonShape(Map.TileWidth / 2f / PhysicsConstants.PhysicsToPixelsRatio, Map.TileHeight / 2f / PhysicsConstants.PhysicsToPixelsRatio);
                            bodyShape.density = 1;
                            bodyShape.friction = 0.0f;

                            var PhysicsBody = physicsWorld.CreateBody(body);
                            PhysicsBody.CreateFixture(bodyShape);
                            PhysicsBody.SetUserData(mapEntity);
                        }
                    }
                }
            }
        }

        private TiledTile getTile(TiledLayer layer, int x, int y)
        {
            int index = (y * layer.width) + x;
            int gid = layer.data[index];

            var mapTileset = Map.GetTiledMapTileset(gid);
            var tileset = Tilesets[mapTileset.firstgid];

            return Map.GetTiledTile(mapTileset, tileset, gid);
        }
    }
}
