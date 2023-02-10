using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TiledCS;
using Arch;
using Arch.Core;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Systems;
using Arch.Core.Extensions;

namespace RogueliteSurvivor
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TiledMap map;
        private Dictionary<int, TiledTileset> tilesets;
        private Texture2D tilesetTexture;

        const int scaleFactor = 3;
        private Matrix transformMatrix;

        private Texture2D playerTexture;
        private AnimationData playerAnimationData;

        private World world;
        private List<IUpdateSystem> updateSystems;
        private Entity player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            transformMatrix = Matrix.CreateScale(scaleFactor, scaleFactor, 1f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            map = new TiledMap(Path.Combine(Content.RootDirectory, "Demo.tmx"));
            tilesets = map.GetTiledTilesets(Content.RootDirectory + "/");
            tilesetTexture = Content.Load<Texture2D>("Tiles");

            world = World.Create();
            player = world.Create(
                new Player(),
                new Position() { XY = new Vector2(50, 50) },
                new Velocity() { Dxy = Vector2.Zero },
                new Speed() { speed = 100f },
                new Animation(1, 1, .1f)
            );

            updateSystems = new List<IUpdateSystem>
            {
                new PlayerInputSystem(world),
                new AnimationSetSystem(world),
                new AnimationUpdateSystem(world),
                new MoveSystem(world),
            };

            playerTexture = Content.Load<Texture2D>("Animated_Mage_Character");
            playerAnimationData = new AnimationData(playerTexture, 3, 8);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach(var system in updateSystems)
            {
                system.Update(gameTime);
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);  // Set samplerState to null to work with high res assets

            var tileLayers = map.Layers.Where(x => x.type == TiledLayerType.TileLayer);

            foreach (var layer in tileLayers)
            {
                for (var y = 0; y < layer.height; y++)
                {
                    for (var x = 0; x < layer.width; x++)
                    {
                        var index = (y * layer.width) + x; // Assuming the default render order is used which is from right to bottom
                        var gid = layer.data[index]; // The tileset tile index
                        var tileX = x * map.TileWidth;
                        var tileY = y * map.TileHeight;

                        // Gid 0 is used to tell there is no tile set
                        if (gid == 0)
                        {
                            continue;
                        }

                        // Helper method to fetch the right TieldMapTileset instance
                        // This is a connection object Tiled uses for linking the correct tileset to the gid value using the firstgid property
                        var mapTileset = map.GetTiledMapTileset(gid);

                        // Retrieve the actual tileset based on the firstgid property of the connection object we retrieved just now
                        var tileset = tilesets[mapTileset.firstgid];

                        // Use the connection object as well as the tileset to figure out the source rectangle
                        var rect = map.GetSourceRect(mapTileset, tileset, gid);

                        // Create destination and source rectangles
                        var source = new Rectangle(rect.x, rect.y, rect.width, rect.height);
                        var destination = new Rectangle(tileX, tileY, map.TileWidth, map.TileHeight);

                        SpriteEffects effects = SpriteEffects.None;
                        double rotation = 0f;

                        // Render sprite at position tileX, tileY using the rect
                        _spriteBatch.Draw(tilesetTexture, new Vector2(tileX, tileY), source, Color.White, (float)rotation, player.Get<Position>().XY, 1f, effects, 0);
                    }
                }
            }

            _spriteBatch.Draw(playerTexture, new Vector2(125, 75), playerAnimationData.SourceRectangle(player.Get<Animation>().CurrentFrame), Color.White, 0f, new Vector2(0, 4), 1f, SpriteEffects.None, 0);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
