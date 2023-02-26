using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Utils;
using System.Collections.Generic;
using System.IO;

namespace RogueliteSurvivor.Scenes
{
    public class MainMenuScene : Scene
    {
        private Dictionary<string, Texture2D> textures = null;
        private Dictionary<string, SpriteFont> fonts = null;

        private bool readyForInput = false;
        private float counter = 0f;

        private MainMenuState state;
        private string selectedPlayer;
        private int selectedButton = 1;

        private Dictionary<string, PlayerContainer> playerContainers;


        public MainMenuScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld, Dictionary<string, PlayerContainer> playerContainers)
            : base(spriteBatch, contentManager, graphics, world, physicsWorld)
        {
            this.playerContainers = playerContainers;
        }

        public override void LoadContent()
        {
            if (textures == null)
            {
                textures = new Dictionary<string, Texture2D>
                {
                    { "MainMenuButtons", Content.Load<Texture2D>(Path.Combine("UI", "main-menu-buttons")) },
                    { "CharacterSelectButtons", Content.Load<Texture2D>(Path.Combine("UI", "character-selection-buttons")) },
                };
            }

            if (fonts == null)
            {
                fonts = new Dictionary<string, SpriteFont>()
                {
                    { "Font", Content.Load<SpriteFont>(Path.Combine("Fonts", "Font")) },
                };
            }

            state = MainMenuState.MainMenu;
            selectedPlayer = "Fire";
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
                var kState = Keyboard.GetState();
                var gState = GamePad.GetState(PlayerIndex.One);

                if (state == MainMenuState.MainMenu)
                {
                    if (kState.IsKeyDown(Keys.Enter) || gState.Buttons.A == ButtonState.Pressed)
                    {
                        switch (selectedButton)
                        {
                            case 1:
                                state = MainMenuState.SpellSelection;
                                break;
                            case 2:
                                break;
                            case 3:
                                retVal = "exit";
                                break;
                        }

                        readyForInput = false;
                    }
                    else if (kState.IsKeyDown(Keys.Up) || gState.DPad.Up == ButtonState.Pressed || gState.ThumbSticks.Left.Y > 0.5f)
                    {
                        selectedButton = int.Max(selectedButton - 1, 1);
                        readyForInput = false;
                    }
                    else if (kState.IsKeyDown(Keys.Down) || gState.DPad.Down == ButtonState.Pressed || gState.ThumbSticks.Left.Y < -0.5f)
                    {
                        selectedButton = int.Min(selectedButton + 1, 3);
                        readyForInput = false;
                    }
                }
                else
                {
                    if (kState.IsKeyDown(Keys.Enter) || gState.Buttons.A == ButtonState.Pressed)
                    {
                        switch (selectedButton)
                        {
                            case 1:
                                selectedPlayer = "Fire";
                                break;
                            case 2:
                                selectedPlayer = "Ice";
                                break;
                            case 3:
                                selectedPlayer = "Lightning";
                                break;
                        }

                        state = MainMenuState.MainMenu;
                        retVal = "loading";
                        readyForInput = false;
                        selectedButton = 1;
                    }
                    else if (kState.IsKeyDown(Keys.Left) || gState.DPad.Left == ButtonState.Pressed || gState.ThumbSticks.Left.X < -0.5f)
                    {
                        selectedButton = int.Max(selectedButton - 1, 1);
                        readyForInput = false;
                    }
                    else if (kState.IsKeyDown(Keys.Right) || gState.DPad.Right == ButtonState.Pressed || gState.ThumbSticks.Left.X > 0.5f)
                    {
                        selectedButton = int.Min(selectedButton + 1, 3);
                        readyForInput = false;
                    }
                    else if (gState.Buttons.B == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                    {
                        state = MainMenuState.MainMenu;
                        selectedButton = 1;
                        readyForInput = false;
                    }
                }

            }

            return retVal;
        }

        public override void Draw(GameTime gameTime, Matrix transformMatrix, params object[] values)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: transformMatrix);

            _spriteBatch.DrawString(
                fonts["Font"],
                "Roguelite Survivor",
                new Vector2(_graphics.PreferredBackBufferWidth / 6 - 62, _graphics.PreferredBackBufferHeight / 6 - 64),
                Color.White
            );

            if (state == MainMenuState.MainMenu)
            {
                _spriteBatch.Draw(
                    textures["MainMenuButtons"],
                    new Vector2(_graphics.PreferredBackBufferWidth / 6, _graphics.PreferredBackBufferHeight / 6),
                    new Rectangle(0 + selectedButton == 1 ? 128 : 0, 32, 128, 32),
                    Color.White,
                    0f,
                    new Vector2(64, 16),
                    1f,
                    SpriteEffects.None,
                    0f
                );

                _spriteBatch.Draw(
                    textures["MainMenuButtons"],
                    new Vector2(_graphics.PreferredBackBufferWidth / 6, _graphics.PreferredBackBufferHeight / 6 + 48),
                    new Rectangle(0 + selectedButton == 2 ? 128 : 0, 64, 128, 32),
                    Color.White,
                    0f,
                    new Vector2(64, 16),
                    1f,
                    SpriteEffects.None,
                    0f
                );

                _spriteBatch.Draw(
                    textures["MainMenuButtons"],
                    new Vector2(_graphics.PreferredBackBufferWidth / 6, _graphics.PreferredBackBufferHeight / 6 + 96),
                    new Rectangle(0 + selectedButton == 3 ? 128 : 0, 96, 128, 32),
                    Color.White,
                    0f,
                    new Vector2(64, 16),
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }
            else
            {
                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Choose your wizard:",
                    new Vector2(_graphics.PreferredBackBufferWidth / 6 - 70, _graphics.PreferredBackBufferHeight / 6 - 32),
                    Color.White
                );

                _spriteBatch.Draw(
                    textures["CharacterSelectButtons"],
                    new Vector2(_graphics.PreferredBackBufferWidth / 6 - 80, _graphics.PreferredBackBufferHeight / 6 + 32),
                    new Rectangle(0 + selectedButton == 1 ? 64 : 0, 64, 64, 64),
                    Color.White,
                    0f,
                    new Vector2(32, 32),
                    1f,
                    SpriteEffects.None,
                    0f
                );

                _spriteBatch.Draw(
                    textures["CharacterSelectButtons"],
                    new Vector2(_graphics.PreferredBackBufferWidth / 6, _graphics.PreferredBackBufferHeight / 6 + 32),
                    new Rectangle(0 + selectedButton == 2 ? 64 : 0, 128, 64, 64),
                    Color.White,
                    0f,
                    new Vector2(32, 32),
                    1f,
                    SpriteEffects.None,
                    0f
                );

                _spriteBatch.Draw(
                    textures["CharacterSelectButtons"],
                    new Vector2(_graphics.PreferredBackBufferWidth / 6 + 80, _graphics.PreferredBackBufferHeight / 6 + 32),
                    new Rectangle(0 + selectedButton == 3 ? 64 : 0, 192, 64, 64),
                    Color.White,
                    0f,
                    new Vector2(32, 32),
                    1f,
                    SpriteEffects.None,
                    0f
                );

                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Press Esc on the keyboard or B on the controller to go back",
                    new Vector2(_graphics.PreferredBackBufferWidth / 6 - 200, _graphics.PreferredBackBufferHeight / 6 + 96),
                    Color.White
                );
            }

            _spriteBatch.End();
        }

        public GameSettings GetGameSettings()
        {
            var gameSettings = new GameSettings()
            {
                PlayerName = selectedPlayer
            };

            return gameSettings;
        }
    }
}
