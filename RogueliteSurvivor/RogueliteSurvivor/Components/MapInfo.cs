﻿using Arch.Core;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TiledCS;

namespace RogueliteSurvivor.Components
{
    public class MapInfo
    {
        public TiledMap Map { get; set; }
        public Dictionary<int, TiledTileset> Tilesets { get; set; }
        private List<SpawnableAreaContainer> spawnableAreas;

        public MapInfo(string mapPath, string tilesetPath, Box2D.NetStandard.Dynamics.World.World physicsWorld, Entity mapEntity, List<SpawnableAreaContainer> spawnableAreas)
        {
            Map = new TiledMap(mapPath);
            Tilesets = Map.GetTiledTilesetsCrossPlatform(tilesetPath);
            this.spawnableAreas = spawnableAreas;

            var tileLayers = Map.Layers.Where(x => x.type == TiledLayerType.TileLayer);

            
            for (int y = 0; y < Map.Width; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    bool passable = true;
                    foreach (var layer in tileLayers)
                    {
                        if (layer.properties[0].value == "true")
                        {
                            var tile = getTile(layer, x, y);

                            if (tile != null)
                            {
                                passable = !(tile.properties.Where(a => a.name == "Passable").First().value == "false");
                            }
                        }
                    }

                    if(!passable)
                    {
                        int tileX = x * Map.TileWidth + Map.TileWidth / 2;
                        int tileY = y * Map.TileHeight + Map.TileHeight / 2;

                        var body = new BodyDef();
                        body.position = new System.Numerics.Vector2(tileX, tileY) / PhysicsConstants.PhysicsToPixelsRatio;
                        body.fixedRotation = true;
                        body.type = BodyType.Static;


                        var bodyShape = new Box2D.NetStandard.Dynamics.Fixtures.FixtureDef();
                        bodyShape.shape = new PolygonShape((Map.TileWidth - 1f) / 2f / PhysicsConstants.PhysicsToPixelsRatio, (Map.TileHeight - 1f) / 2f / PhysicsConstants.PhysicsToPixelsRatio);
                        bodyShape.density = 1;
                        bodyShape.friction = 0.0f;

                        var PhysicsBody = physicsWorld.CreateBody(body);
                        PhysicsBody.CreateFixture(bodyShape);
                        PhysicsBody.SetUserData(mapEntity);
                    }
                }
            }
        }

        public bool IsTileWalkable(int x, int y)
        {
            var tileLayers = Map.Layers.Where(x => x.type == TiledLayerType.TileLayer);
            bool passable = spawnableAreas.Exists(a => a.SpawnMinX <= x && a.SpawnMaxX >= x && a.SpawnMinY <= y && a.SpawnMaxY >= y);

            if (passable)
            {
                foreach (var layer in tileLayers)
                {
                    if (layer.properties[0].value == "true")
                    {
                        var tile = getTile(layer, x / Map.TileWidth, y / Map.TileHeight);

                        if (tile != null && tile.properties.Where(a => a.name == "Passable").First().value == "false")
                        {
                            passable = false;
                        }
                    }
                }
            }

            return passable;
        }

        public bool IsTileFullHeight(int x, int y)
        {
            bool fullHeight = true;
            var tileLayers = Map.Layers.Where(x => x.type == TiledLayerType.TileLayer);
            
                foreach (var layer in tileLayers)
                {
                    if (layer.properties[0].value == "true")
                    {
                        var tile = getTile(layer, x / Map.TileWidth, y / Map.TileHeight);

                        if (tile != null && tile.properties.Where(a => a.name == "Full Height").First().value == "false")
                        {
                            fullHeight = false;
                        }
                    }
                }
            return fullHeight;
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
