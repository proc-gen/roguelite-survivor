using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        const int scaleFactor = 3;
        private Matrix transformMatrix;

        private World world;
        private List<IUpdateSystem> updateSystems;
        private List<IRenderSystem> renderSystems;
        private Entity player;

        private Dictionary<string, Texture2D> textures;

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

            textures = new Dictionary<string, Texture2D>
            {
                { "tiles", Content.Load<Texture2D>("Tiles") },
                { "player", Content.Load<Texture2D>("Animated_Mage_Character") }
            };

            world = World.Create();
            world.Create(new MapInfo(Path.Combine(Content.RootDirectory, "Demo.tmx"), Content.RootDirectory + "/"));
            player = world.Create(
                new Player(),
                new Position() { XY = new Vector2(50, 50) },
                new Velocity() { Dxy = Vector2.Zero },
                new Speed() { speed = 100f },
                new Animation(1, 1, .1f),
                new SpriteSheet(textures["player"], "player", 3, 8)
            );

            updateSystems = new List<IUpdateSystem>
            {
                new PlayerInputSystem(world),
                new AnimationSetSystem(world),
                new AnimationUpdateSystem(world),
                new MoveSystem(world),
            };

            renderSystems = new List<IRenderSystem>
            {
                new RenderMapSystem(world),
                new RenderSpriteSystem(world),
            };
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

            foreach(var system in renderSystems)
            {
                system.Render(gameTime, _spriteBatch, textures, player);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
