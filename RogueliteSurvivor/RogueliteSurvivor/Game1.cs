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
using System.Threading.Tasks;

namespace RogueliteSurvivor
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        const int scaleFactor = 3;
        private Matrix transformMatrix;

        private World world = null;
        Box2D.NetStandard.Dynamics.World.World physicsWorld = null;
        System.Numerics.Vector2 gravity = System.Numerics.Vector2.Zero;

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
            world = World.Create();
            physicsWorld = new Box2D.NetStandard.Dynamics.World.World(gravity);
            physicsWorld.SetContactListener(new GameContactListener());
            physicsWorld.SetContactFilter(new GameContactFilter());

            GameScene gameScene = new GameScene(_spriteBatch, Content, _graphics, world, physicsWorld);

            MainMenuScene mainMenu = new MainMenuScene(_spriteBatch, Content, _graphics, world, physicsWorld);
            mainMenu.LoadContent();

            LoadingScene loadingScene = new LoadingScene(_spriteBatch, Content, _graphics, world, physicsWorld);
            loadingScene.LoadContent();

            GameOverScene gameOverScene = new GameOverScene(_spriteBatch, Content, _graphics, world, physicsWorld);
            gameOverScene.LoadContent();

            scenes.Add("game", gameScene);
            scenes.Add("main-menu", mainMenu);
            scenes.Add("loading", loadingScene);
            scenes.Add("game-over", gameOverScene);
        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentScene)
            {
                case "loading":
                    nextScene = scenes[currentScene].Update(gameTime, scenes["game"].Loaded);
                    break;
                default:
                    nextScene = scenes[currentScene].Update(gameTime);
                    break;
            }
            
            if(!string.IsNullOrEmpty(nextScene))
            {
                switch (nextScene)
                {
                    case "game":
                        break;
                    case "main-menu":
                        break;
                    case "game-over":
                        break;
                    case "loading":
                        Task.Run(() => 
                            {
                                ((GameScene)scenes["game"]).SetGameSettings(((MainMenuScene)scenes["main-menu"]).GetGameSettings());
                                scenes["game"].LoadContent(); 
                            });
                        break;
                    case "exit":
                        Exit();
                        break;
                }

                currentScene = nextScene;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (currentScene)
            {
                case "loading":
                    scenes[currentScene].Draw(gameTime, transformMatrix, scenes["game"].Loaded);
                    break;
                default:
                    scenes[currentScene].Draw(gameTime, transformMatrix);
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
