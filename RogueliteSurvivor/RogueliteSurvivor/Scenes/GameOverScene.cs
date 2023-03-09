using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RogueliteSurvivor.Scenes
{
    public class GameOverScene : Scene
    {
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        private Dictionary<string, MapContainer> mapContainers;

        private GameSettings gameSettings;
        private GameStats gameStats;
        private bool saved = false;
        private bool newBest = false;
        private MapContainer unlockedMap;

        public GameOverScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld, ProgressionContainer progressionContainer, Dictionary<string, MapContainer> mapContainers)
            : base(spriteBatch, contentManager, graphics, world, physicsWorld, progressionContainer)
        {
            this.mapContainers = mapContainers;
        }

        public override void LoadContent()
        {
            fonts = new Dictionary<string, SpriteFont>()
            {
                { "Font", Content.Load<SpriteFont>(Path.Combine("Fonts", "Font")) },
                { "FontSmall", Content.Load<SpriteFont>(Path.Combine("Fonts", "FontSmall")) },
            };

            Loaded = true;
        }

        public void SetGameSettings(GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;
        }

        public void SetGameStats(GameStats gameStats)
        {
            this.gameStats = gameStats;
        }

        public override string Update(GameTime gameTime, params object[] values)
        {
            string retVal = string.Empty;

            if (!saved)
            {
                unlockedMap = null;
                var level = progressionContainer.LevelProgressions.Where(a => a.Name == gameSettings.MapName).FirstOrDefault();
                newBest = gameStats.PlayTime > level.BestTime;

                if (newBest)
                {                    
                    unlockedMap = mapContainers.Values.Where(a => a.UnlockRequirement.MapUnlockType == Constants.MapUnlockType.MapBestTime 
                                                                    && a.UnlockRequirement.RequirementText == gameSettings.MapName
                                                                    && a.UnlockRequirement.RequirementAmount <= gameStats.PlayTime).FirstOrDefault();

                    if(unlockedMap != null && level.BestTime >= unlockedMap.UnlockRequirement.RequirementAmount)
                    {
                        unlockedMap = null;
                    }

                    level.BestTime = MathF.Max(gameStats.PlayTime, level.BestTime);
                    progressionContainer.Save();
                }
                
                saved = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                retVal = "main-menu";
                saved = false;
            }

            return retVal;
        }

        public override void Draw(GameTime gameTime, Matrix transformMatrix, params object[] values)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: transformMatrix);

            _spriteBatch.DrawString(
                fonts["Font"],
               string.Concat(getProperArticle(gameStats.Killer), gameStats.Killer, " pummeled you into oblivion..."),
                new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6 - 64),
                Color.White
            );

            _spriteBatch.DrawString(
                fonts["Font"],
               string.Concat("You killed ", gameStats.EnemiesKilled, " enemies in ", float.Round(gameStats.PlayTime, 2), " seconds!"),
                new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6 - 32),
                Color.White
            );

            if (newBest)
            {
                _spriteBatch.DrawString(
                    fonts["Font"],
                   string.Concat("New best time for ", gameSettings.MapName,"!"),
                    new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6),
                    Color.White
                );
            }

            if(unlockedMap != null)
            {
                _spriteBatch.DrawString(
                    fonts["Font"],
                   string.Concat("You've unlocked ", unlockedMap.Name, "!"),
                    new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6 + 32),
                    Color.White
                );
            }
            

            _spriteBatch.DrawString(
                fonts["FontSmall"],
                "Press Space on the keyboard or Start on the controller to return to the main menu",
                new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6 + 96),
                Color.White
            );

            _spriteBatch.End();
        }

        private string getProperArticle(string enemyName)
        {
            if (enemyName.StartsWith('A') || enemyName.StartsWith('E') || enemyName.StartsWith('I') || enemyName.StartsWith('O') || enemyName.StartsWith('U'))
            {
                return "An ";
            }

            return "A ";
        }
    }
}
