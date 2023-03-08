using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RogueliteSurvivor.Scenes
{
    public class GameOverScene : Scene
    {
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        private QueryDescription queryDescription;

        private GameSettings gameSettings;
        private bool saved = false;

        public GameOverScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld, ProgressionContainer progressionContainer)
            : base(spriteBatch, contentManager, graphics, world, physicsWorld, progressionContainer)
        {
            queryDescription = new QueryDescription()
                                    .WithAll<Player, KillCount>();
        }

        public override void LoadContent()
        {
            fonts = new Dictionary<string, SpriteFont>()
            {
                { "Font", Content.Load<SpriteFont>(Path.Combine("Fonts", "Font")) },
            };

            Loaded = true;
        }

        public void SetGameSettings(GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;
        }

        public override string Update(GameTime gameTime, params object[] values)
        {
            string retVal = string.Empty;

            if (!saved)
            {
                var level = progressionContainer.LevelProgressions.Where(a => a.Name == gameSettings.MapName).FirstOrDefault();
                level.BestTime = 11;

                progressionContainer.Save();
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
               "Oh snap, the bats killed you!",
                new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6),
                Color.White
            );

            _spriteBatch.DrawString(
                fonts["Font"],
               string.Concat("Enemies Killed: ", getPlayerKillCount().Count),
                new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6 + 32),
                Color.White
            );


            _spriteBatch.DrawString(
                fonts["Font"],
                "Press Space on the keyboard or Start on the controller to return to the main menu",
                new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6 + 96),
                Color.White
            );

            _spriteBatch.End();
        }

        private KillCount getPlayerKillCount()
        {
            KillCount playerKillCount = new KillCount();
            world.Query(in queryDescription, (ref KillCount killCount) =>
            {
                playerKillCount.Count = killCount.Count;
            });
            return playerKillCount;
        }
    }
}
