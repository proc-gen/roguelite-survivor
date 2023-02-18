using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Scenes
{
    public class GameOverScene : Scene
    {
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        private QueryDescription queryDescription;

        public GameOverScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld) 
            : base(spriteBatch, contentManager, graphics, world, physicsWorld)
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

        public override string Update(GameTime gameTime, params object[] values)
        {
            string retVal = string.Empty;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                retVal = "main-menu";
            }

            return retVal;
        }

        public override void Draw(GameTime gameTime, params object[] values)
        {
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
