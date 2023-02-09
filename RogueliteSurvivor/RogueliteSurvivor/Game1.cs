using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TiledCS;

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

        private Vector2 playerPosition = new Vector2(50,50);
        private float playerSpeed = 100f;
        private Texture2D playerTexture;
        private AnimationData playerAnimationData;
        private AnimationTimer playerAnimationTimer;
        private int currentPlayerDirection = 0;

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

            map = new TiledMap(Content.RootDirectory + "\\Demo.tmx");
            tilesets = map.GetTiledTilesets(Content.RootDirectory + "/");
            tilesetTexture = Content.Load<Texture2D>("Tiles");

            playerTexture = Content.Load<Texture2D>("Animated_Mage_Character");
            playerAnimationData = new AnimationData(playerTexture, 3, 8);
            playerAnimationTimer = new AnimationTimer(1, 1, .1f);
        }

        private void setPlayerAnimation(int direction, int speed)
        {
            currentPlayerDirection = direction;
            switch(direction)
            {
                case 0: //Idle
                    playerAnimationTimer.Reset(1, 1);
                    break;
                case 1: //Down
                    playerAnimationTimer.Reset(0, 2);
                    break;
                case 2: //Left
                    playerAnimationTimer.Reset(3, 5);
                    break;
                case 3: //Up
                    playerAnimationTimer.Reset(6, 8);
                    break;
                case 4: //Right
                    playerAnimationTimer.Reset(9, 11);
                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
            {
                playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(currentPlayerDirection != 3)
                {
                    setPlayerAnimation(3, 0);
                }
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                playerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentPlayerDirection != 1)
                {
                    setPlayerAnimation(1, 0);
                }
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                playerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentPlayerDirection != 2)
                {
                    setPlayerAnimation(2, 0);
                }
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                playerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentPlayerDirection != 4)
                {
                    setPlayerAnimation(4, 0);
                }
            }

            if (kstate.GetPressedKeyCount() == 0) 
            {
                setPlayerAnimation(0, 0);
            }
            
            playerAnimationTimer.Update(gameTime);

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
                        _spriteBatch.Draw(tilesetTexture, destination, source, Color.White, (float)rotation, Vector2.Zero, effects, 0);
                    }
                }
            }

            _spriteBatch.Draw(playerTexture, playerPosition, playerAnimationData.SourceRectangle(playerAnimationTimer.currentFrame), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}