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
using RogueliteSurvivor.Physics;
using JobScheduler;
using Box2D.NetStandard.Dynamics.Bodies;
using RogueliteSurvivor.Scenes;

namespace RogueliteSurvivor
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        const int scaleFactor = 3;
        private Matrix transformMatrix;

        Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
        string currentScene = "main-menu";
        string nextScene = string.Empty;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.ApplyChanges(); //Needed because the graphics device is null before this is called
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            transformMatrix = Matrix.CreateScale(scaleFactor, scaleFactor, 1f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameScene gameScene = new GameScene(_spriteBatch, Content, _graphics);
            MainMenuScene mainMenu = new MainMenuScene(_spriteBatch, Content, _graphics);
            mainMenu.LoadContent();

            scenes.Add("game", gameScene);
            scenes.Add("main-menu", mainMenu);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                _graphics.ToggleFullScreen();
            }
            
            else
            {
                nextScene = scenes[currentScene].Update(gameTime);
                if(!string.IsNullOrEmpty(nextScene))
                {
                    switch (nextScene)
                    {
                        case "game":
                            scenes["game"].LoadContent();
                            break;
                        case "main-menu":
                            break;
                        case "exit":
                            Exit();
                            break;
                    }

                    currentScene = nextScene;
                }

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);

            scenes[currentScene].Draw(gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
