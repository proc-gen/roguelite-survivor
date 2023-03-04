using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        private string selectedMap = "Demo";

        private Dictionary<string, PlayerContainer> playerContainers;
        private List<MapContainer> mapContainers;
        private CreditsContainer creditsContainer = null;


        public MainMenuScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld, Dictionary<string, PlayerContainer> playerContainers, Dictionary<string, MapContainer> mapContainers)
            : base(spriteBatch, contentManager, graphics, world, physicsWorld)
        {
            this.playerContainers = playerContainers;
            this.mapContainers = mapContainers.Values.ToList();
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
                    { "FontSmall", Content.Load<SpriteFont>(Path.Combine("Fonts", "FontSmall")) },
                };
            }

            if(creditsContainer == null)
            {
                JObject credits = JObject.Parse(File.ReadAllText(Path.Combine(Content.RootDirectory, "Datasets", "credits.json")));
                creditsContainer = CreditsContainer.ToCreditsContainer(credits["data"]);
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
                                state = MainMenuState.CharacterSelection;
                                break;
                            case 2:
                                break;
                            case 3:
                                state = MainMenuState.Credits;
                                break;
                            case 4:
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
                        selectedButton = int.Min(selectedButton + 1, 4);
                        readyForInput = false;
                    }
                }
                else if (state == MainMenuState.CharacterSelection)
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

                        state = MainMenuState.MapSelection;
                        readyForInput = false;
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
                else if(state == MainMenuState.MapSelection)
                {
                    if (kState.IsKeyDown(Keys.Enter) || gState.Buttons.A == ButtonState.Pressed)
                    {
                        state = MainMenuState.MainMenu;
                        retVal = "loading";
                        readyForInput = false;
                        selectedButton = 1;
                    }
                    else if (gState.Buttons.B == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                    {
                        state = MainMenuState.CharacterSelection;
                        readyForInput = false;
                    }
                    else if (kState.IsKeyDown(Keys.Up) || gState.DPad.Up == ButtonState.Pressed || gState.ThumbSticks.Left.Y > 0.5f)
                    {
                        if(selectedMap != mapContainers[0].Name)
                        {
                            int index = mapContainers.IndexOf(mapContainers.Where(a => a.Name == selectedMap).First()) - 1;
                            selectedMap = mapContainers[index].Name;
                        }
                        readyForInput = false;
                    }
                    else if (kState.IsKeyDown(Keys.Down) || gState.DPad.Down == ButtonState.Pressed || gState.ThumbSticks.Left.Y < -0.5f)
                    {
                        if (selectedMap != mapContainers.Last().Name)
                        {
                            int index = mapContainers.IndexOf(mapContainers.Where(a => a.Name == selectedMap).First()) + 1;
                            selectedMap = mapContainers[index].Name;
                        }
                        readyForInput = false;
                    }
                }
                else if (state == MainMenuState.Credits)
                {
                    if (gState.Buttons.A == ButtonState.Pressed || gState.Buttons.B == ButtonState.Pressed 
                        || kState.IsKeyDown(Keys.Escape) || kState.IsKeyDown(Keys.Enter))
                    {
                        state = MainMenuState.MainMenu;
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
                    new Rectangle(0 + selectedButton == 3 ? 128 : 0, 128, 128, 32),
                    Color.White,
                    0f,
                    new Vector2(64, 16),
                    1f,
                    SpriteEffects.None,
                    0f
                );

                _spriteBatch.Draw(
                    textures["MainMenuButtons"],
                    new Vector2(_graphics.PreferredBackBufferWidth / 6, _graphics.PreferredBackBufferHeight / 6 + 144),
                    new Rectangle(0 + selectedButton == 4 ? 128 : 0, 96, 128, 32),
                    Color.White,
                    0f,
                    new Vector2(64, 16),
                    1f,
                    SpriteEffects.None,
                    0f
                );
            }
            else if (state == MainMenuState.CharacterSelection)
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
            else if (state == MainMenuState.MapSelection)
            {
                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Select a map:",
                    new Vector2(_graphics.PreferredBackBufferWidth / 6 - 50, _graphics.PreferredBackBufferHeight / 6 - 32),
                    Color.White
                );

                int counter = 0;
                foreach(var map in mapContainers)
                {
                    _spriteBatch.DrawString(
                        fonts["Font"],
                        map.Name,
                        new Vector2(_graphics.PreferredBackBufferWidth / 6 - 45, _graphics.PreferredBackBufferHeight / 6 + counter),
                        selectedMap == map.Name ? Color.Green : Color.White
                    );

                    counter += 32;
                }

                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Press Esc on the keyboard or B on the controller to go back",
                    new Vector2(_graphics.PreferredBackBufferWidth / 6 - 200, _graphics.PreferredBackBufferHeight / 6 + 32 + counter),
                    Color.White
                );
            }
            else if (state == MainMenuState.Credits)
            {
                int counterX = 0, counterY = 0;
                foreach(var outsideResource in creditsContainer.OutsideResources)
                {
                    _spriteBatch.DrawString(
                        fonts["Font"],
                        outsideResource.Author,
                        new Vector2(_graphics.PreferredBackBufferWidth / 6 - 300 + counterX, _graphics.PreferredBackBufferHeight / 6 + counterY),
                        Color.White
                    );
                    counterY += 18;

                    foreach(var package in outsideResource.Packages)
                    {
                        if(package.Length > 40)
                        {
                            string part1, part2;
                            part1 = package.Substring(0, package.IndexOf(' ', 30));
                            part2 = package.Substring(package.IndexOf(' ', 30));
                            _spriteBatch.DrawString(
                                fonts["FontSmall"],
                                part1,
                                new Vector2(_graphics.PreferredBackBufferWidth / 6 - 288 + counterX, _graphics.PreferredBackBufferHeight / 6 + counterY),
                                Color.White
                            );
                            counterY += 12;
                            _spriteBatch.DrawString(
                                fonts["FontSmall"],
                                part2,
                                new Vector2(_graphics.PreferredBackBufferWidth / 6 - 276 + counterX, _graphics.PreferredBackBufferHeight / 6 + counterY),
                                Color.White
                            );
                        }
                        else
                        {
                            _spriteBatch.DrawString(
                                fonts["FontSmall"],
                                package,
                                new Vector2(_graphics.PreferredBackBufferWidth / 6 - 288 + counterX, _graphics.PreferredBackBufferHeight / 6 + counterY),
                                Color.White
                            );
                        }
                        
                        counterY += 12;
                    }

                    counterY += 18;

                    if(counterY > 100)
                    {
                        counterY = 0;
                        counterX += 200;
                    }
                }
            }
            _spriteBatch.End();
        }

        public GameSettings GetGameSettings()
        {
            var gameSettings = new GameSettings()
            {
                PlayerName = selectedPlayer,
                MapName = selectedMap,
            };

            return gameSettings;
        }
    }
}
