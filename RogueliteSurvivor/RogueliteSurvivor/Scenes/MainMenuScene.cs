using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Scenes
{
    public class MainMenuScene : Scene
    {
        private Dictionary<string, Texture2D> textures = null;
        private Dictionary<string, SpriteFont> fonts = null;

        private bool readyForInput = false;
        private float counter = 0f;

        private MainMenuState state;
        private Spells selectedSpell;

        public MainMenuScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld)
            : base(spriteBatch, contentManager, graphics, world, physicsWorld)
        {
        }

        public override void LoadContent()
        {
            if (fonts == null)
            {
                fonts = new Dictionary<string, SpriteFont>()
                {
                    { "Font", Content.Load<SpriteFont>(Path.Combine("Fonts", "Font")) },
                };
            }

            state = MainMenuState.MainMenu;
            selectedSpell = Spells.Fireball;
            Loaded = true;
        }

        public override string Update(GameTime gameTime, params object[] values)
        {
            string retVal = string.Empty;

            if (!readyForInput)
            {
                counter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (counter > 0.1f)
                {
                    counter = 0f;
                    readyForInput = true;
                }
            }
            else if (readyForInput)
            {
                if(state == MainMenuState.MainMenu) 
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                    {
                        state = MainMenuState.SpellSelection;
                        readyForInput = false;
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        retVal = "exit";
                        readyForInput = false;
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D1) || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                    {
                        selectedSpell = Spells.Fireball;
                        state = MainMenuState.MainMenu;
                        retVal = "loading";
                        readyForInput = false;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D2) || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                    {
                        selectedSpell = Spells.IceShard;
                        state = MainMenuState.MainMenu;
                        retVal = "loading";
                        readyForInput = false;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D3) || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                    {
                        selectedSpell = Spells.LightningBlast;
                        state = MainMenuState.MainMenu;
                        retVal = "loading";
                        readyForInput = false;
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        state = MainMenuState.MainMenu;
                        readyForInput = false;
                    }
                }
                
            }

            return retVal;
        }

        public override void Draw(GameTime gameTime, params object[] values)
        {
            if (state == MainMenuState.MainMenu)
            {
                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Press Space on the keyboard or Start on the controller to select a starting spell",
                    new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6),
                    Color.White
                );

                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Press Esc on the keyboard or Back on the controller to exit",
                    new Vector2(_graphics.PreferredBackBufferWidth / 32, _graphics.PreferredBackBufferHeight / 6 + 32),
                    Color.White
                );
            }
            else
            {
                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Press 1 on the keyboard or up on the controller direction pad to select fireball",
                    new Vector2(10, _graphics.PreferredBackBufferHeight / 6),
                    Color.White
                );

                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Press 2 on the keyboard or left on the controller direction pad to select ice shard",
                    new Vector2(10, _graphics.PreferredBackBufferHeight / 6 + 32),
                    Color.White
                );

                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Press 3 on the keyboard or right on the controller direction pad to select lightning blast",
                    new Vector2(10, _graphics.PreferredBackBufferHeight / 6 + 64),
                    Color.White
                );

                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Press Esc on the keyboard or Back on the controller to exit",
                    new Vector2(10, _graphics.PreferredBackBufferHeight / 6 + 128),
                    Color.White
                );
            }
        }

        public GameSettings GetGameSettings()
        {
            var gameSettings = new GameSettings();
            gameSettings.StartingSpell = selectedSpell;

            switch (selectedSpell)
            {
                case Spells.Fireball:
                    gameSettings.PlayerTexture = "player";
                    break;
                case Spells.IceShard:
                    gameSettings.PlayerTexture = "player_blue";
                    break;
                case Spells.LightningBlast:
                    gameSettings.PlayerTexture = "player_yellow";
                    break;
            }

            return gameSettings;
        }
    }
}
