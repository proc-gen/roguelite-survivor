using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Scenes
{
    public class MainMenuScene : Scene
    {
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        public MainMenuScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics) : base(spriteBatch, contentManager, graphics)
        {
        }

        public override void LoadContent()
        {
            fonts = new Dictionary<string, SpriteFont>()
            {
                { "Font", Content.Load<SpriteFont>("Font") },
            };
        }

        public override string Update(GameTime gameTime)
        {
            string retVal = string.Empty;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                retVal = "game";
            }
            else if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                retVal = "exit";
            }

            return retVal;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.DrawString(
                fonts["Font"],
                "Press Space on the keyboard or Start on the controller to begin the game",
                new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6),
                Color.White
            );
        }
    }
}
