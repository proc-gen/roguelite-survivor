using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using RogueliteSurvivor.ComponentFactories;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Extensions;
using RogueliteSurvivor.Helpers;
using RogueliteSurvivor.Physics;
using RogueliteSurvivor.Systems;
using RogueliteSurvivor.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;

namespace RogueliteSurvivor.Scenes
{
    public class GameScene : Scene
    {

        private List<IUpdateSystem> updateSystems;
        private List<IRenderSystem> renderSystems;
        private Entity player;

        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        private float totalGameTime = 0f;

        private GameState gameState;
        private float stateChangeTime = .11f;
        private GameSettings gameSettings;

        private Dictionary<string, EnemyContainer> enemyContainers;
        private Dictionary<Spells, SpellContainer> spellContainers;
        private Dictionary<string, PlayerContainer> playerContainers;
        private Dictionary<string, MapContainer> mapContainers;

        private MapContainer mapContainer;
        
        private Random random;
        private List<PickupType> levelUpChoices = new List<PickupType>();
        private PickupType selectedLevelUpChoice;

        public GameScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld, Dictionary<string, PlayerContainer> playerContainers, Dictionary<string, MapContainer> mapContainers)
            : base(spriteBatch, contentManager, graphics, world, physicsWorld)
        {
            this.playerContainers = playerContainers;
            this.mapContainers = mapContainers;

            random = new Random();
        }

        public void SetGameSettings(GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;
        }

        public override void LoadContent()
        {
            resetWorld();
            loadTexturesAndFonts();

            loadMap();
            loadEnemies();
            loadSpells();
            initializeSystems();
            placePlayer();

            totalGameTime = 0;
            gameState = GameState.Running;

            Loaded = true;
        }

        private void loadTexturesAndFonts()
        {
            textures = new Dictionary<string, Texture2D>
            {
                { "Fireball", Content.Load<Texture2D>(Path.Combine("Spells", "fireball")) },
                { "IceShard", Content.Load<Texture2D>(Path.Combine("Spells", "ice-shard")) },
                { "LightningBlast", Content.Load<Texture2D>(Path.Combine("Spells", "lightning-blast")) },
                { "FireExplosion", Content.Load<Texture2D>(Path.Combine("Spells", "fire-explosion")) },
                { "IceSpikes", Content.Load<Texture2D>(Path.Combine("Spells", "ice-spikes")) },
                { "LightningStrike", Content.Load<Texture2D>(Path.Combine("Spells", "lightning-strike")) },

                { "StatBar", Content.Load<Texture2D>(Path.Combine("Hud", "StatBar")) },
                { "HealthBar", Content.Load<Texture2D>(Path.Combine("Hud", "HealthBar")) },
                { "StatsBackground", Content.Load<Texture2D>(Path.Combine("Hud", "StatsBackground")) },

                { "LevelUpChoices", Content.Load<Texture2D>(Path.Combine("UI", "level-up-buttons")) },

                { "pickups", Content.Load<Texture2D>(Path.Combine("Pickups", "player-pickups")) },

                { "MiniBlood1", Content.Load<Texture2D>(Path.Combine("Effects", "mini-blood-1")) },
                { "MiniBlood2", Content.Load<Texture2D>(Path.Combine("Effects", "mini-blood-2")) },
                { "MiniBlood3", Content.Load<Texture2D>(Path.Combine("Effects", "mini-blood-3")) },
                { "MiniBlood4", Content.Load<Texture2D>(Path.Combine("Effects", "mini-blood-4")) },
                { "MiniBlood5", Content.Load<Texture2D>(Path.Combine("Effects", "mini-blood-5")) },
                { "MiniBlood6", Content.Load<Texture2D>(Path.Combine("Effects", "mini-blood-6")) },
                { "MiniBlood7", Content.Load<Texture2D>(Path.Combine("Effects", "mini-blood-7")) },
                { "MiniBlood8", Content.Load<Texture2D>(Path.Combine("Effects", "mini-blood-8")) },
                { "MiniBlood9", Content.Load<Texture2D>(Path.Combine("Effects", "mini-blood-9")) },
                { "Blood1", Content.Load<Texture2D>(Path.Combine("Effects", "blood-1")) },
                { "Blood2", Content.Load<Texture2D>(Path.Combine("Effects", "blood-2")) },
                { "Blood3", Content.Load<Texture2D>(Path.Combine("Effects", "blood-3")) },
                { "Blood4", Content.Load<Texture2D>(Path.Combine("Effects", "blood-4")) },
                { "Blood5", Content.Load<Texture2D>(Path.Combine("Effects", "blood-5")) },

                { "IceShardHit", Content.Load<Texture2D>(Path.Combine("Effects", "ice-shard-hit")) },
                { "LightningBlastHit", Content.Load<Texture2D>(Path.Combine("Effects", "lightning-blast-hit")) },
                { "FireballHit", Content.Load<Texture2D>(Path.Combine("Effects", "fireball-hit")) },
            };

            fonts = new Dictionary<string, SpriteFont>()
            {
                { "Font", Content.Load<SpriteFont>(Path.Combine("Fonts", "Font")) },
                { "FontSmall", Content.Load<SpriteFont>(Path.Combine("Fonts", "FontSmall")) },
            };
        }

        private void loadEnemies()
        {
            JObject enemies = JObject.Parse(File.ReadAllText(Path.Combine(Content.RootDirectory, "Datasets", "enemies.json")));
            enemyContainers = new Dictionary<string, EnemyContainer>();

            foreach (var enemy in enemies["data"])
            {
                enemyContainers.Add(
                    EnemyContainer.EnemyContainerName(enemy),
                    EnemyContainer.ToEnemyContainer(enemy)
                );
            }
        }

        private void loadSpells()
        {
            JObject spells = JObject.Parse(File.ReadAllText(Path.Combine(Content.RootDirectory, "Datasets", "spells.json")));
            spellContainers = new Dictionary<Spells, SpellContainer>();

            foreach (var spell in spells["data"])
            {
                spellContainers.Add(
                    SpellContainer.SpellContainerName(spell),
                    SpellContainer.ToSpellContainer(spell)
                );
            }
        }

        private void resetWorld()
        {
            if (world.CountEntities(new QueryDescription()) > 0)
            {
                List<Entity> entities = new List<Entity>();
                world.GetEntities(new QueryDescription(), entities);
                foreach (var entity in entities)
                {
                    world.TryDestroy(entity);
                }
            }

            if (physicsWorld.GetBodyCount() > 0)
            {
                var physicsBody = physicsWorld.GetBodyList();
                while (physicsBody != null)
                {
                    var nextPhysicsBody = physicsBody.GetNext();
                    physicsWorld.DestroyBody(physicsBody);
                    physicsBody = nextPhysicsBody;
                };
            }
        }

        private void initializeSystems()
        {
            var renderHud = new RenderHudSystem(world, _graphics, fonts);

            updateSystems = new List<IUpdateSystem>
            {
                new PlayerInputSystem(world),
                new TargetingSystem(world),
                new EnemyAISystem(world),
                new AnimationSetSystem(world),
                new AnimationUpdateSystem(world),
                new CollisionSystem(world, physicsWorld),
                new SpellEffectSystem(world),
                new PickupSystem(world),
                new EnemySpawnSystem(world, textures, physicsWorld, _graphics, enemyContainers, spellContainers, mapContainer),
                new AttackSystem(world, textures, physicsWorld, spellContainers),
                new AttackSpellCleanupSystem(world),
                new DeathSystem(world, textures, physicsWorld, spellContainers),
                renderHud,
            };

            renderSystems = new List<IRenderSystem>
            {
                new RenderSpriteSystem(world, _graphics),
                new RenderPickupSystem(world, _graphics),
                new RenderMapSystem(world, _graphics),
                renderHud,
            };
        }

        private void loadMap()
        {
            mapContainer = mapContainers[gameSettings.MapName];

            var mapEntity = world.Create<Map, MapInfo>();
            mapEntity.Set(new Map(), new MapInfo(Path.Combine(Content.RootDirectory, "Maps", mapContainer.Folder, mapContainer.MapFilename), Path.Combine(Content.RootDirectory, "Maps", mapContainer.Folder), physicsWorld, mapEntity, mapContainer.Spawnables));

            foreach (var tilesetImage in mapContainer.TilesetImages)
            {
                textures.Add(tilesetImage.ToLower(), Content.Load<Texture2D>(Path.Combine("Maps", mapContainer.Folder, tilesetImage)));
            }

            foreach(var wave in mapContainer.EnemyWaves)
            {
                foreach(var enemy in wave.Enemies)
                {
                    if(!textures.ContainsKey(enemy.Type))
                    {
                        textures.Add(enemy.Type, Content.Load<Texture2D>(Path.Combine("Enemies", enemy.Type)));
                    }
                }
            }
        }

        private void placePlayer()
        {
            textures.Add(playerContainers[gameSettings.PlayerName].Texture, Content.Load<Texture2D>(Path.Combine("Player", playerContainers[gameSettings.PlayerName].Texture)));

            var body = new BodyDef();
            body.position = new System.Numerics.Vector2(mapContainer.Start.X, mapContainer.Start.Y) / PhysicsConstants.PhysicsToPixelsRatio;
            body.fixedRotation = true;

            player = world.Create<Player, EntityStatus, Position, Velocity, Speed, AttackSpeed, SpellDamage, SpellEffectChance, Pierce, AreaOfEffect, Animation, SpriteSheet, Target, Spell1, Spell2, Health, KillCount, Body>();

            player.Set(
                new Player() { Level = 1, ExperienceToNextLevel = ExperienceHelper.ExperienceRequiredForLevel(2), TotalExperience = 0 },
                new EntityStatus(),
                new Position() { XY = new Vector2(mapContainer.Start.X, mapContainer.Start.Y) },
                new Velocity() { Vector = Vector2.Zero },
                new Speed() { speed = playerContainers[gameSettings.PlayerName].Speed },
                new AttackSpeed(1f),
                new SpellDamage(1f),
                new SpellEffectChance(1f),
                new Pierce(0),
                new AreaOfEffect(1),
                PlayerFactory.GetPlayerAnimation(playerContainers[gameSettings.PlayerName]),
                PlayerFactory.GetPlayerSpriteSheet(playerContainers[gameSettings.PlayerName], textures),
                new Target(),
                SpellFactory.CreateSpell<Spell1>(spellContainers[playerContainers[gameSettings.PlayerName].StartingSpell]),
                SpellFactory.CreateSpell<Spell2>(spellContainers[playerContainers[gameSettings.PlayerName].SecondarySpell]),
                new Health() { Current = playerContainers[gameSettings.PlayerName].Health, Max = playerContainers[gameSettings.PlayerName].Health },
                new KillCount() { Count = 0 },
                BodyFactory.CreateCircularBody(player, 16, physicsWorld, body, 99)
            );
        }

        public override string Update(GameTime gameTime, params object[] values)
        {
            string retVal = string.Empty;
            var kState = Keyboard.GetState();
            var gState = GamePad.GetState(PlayerIndex.One);

            if (gameState == GameState.LevelUp)
            {
                stateChangeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (stateChangeTime > .1f)
                {
                    if (kState.IsKeyDown(Keys.Enter) || gState.Buttons.A == ButtonState.Pressed)
                    {
                        gameState = GameState.Running;
                        PickupHelper.ProcessPickup(ref player, selectedLevelUpChoice);
                        stateChangeTime = 0f;
                    }
                    else if (kState.IsKeyDown(Keys.Left) || gState.DPad.Left == ButtonState.Pressed || gState.ThumbSticks.Left.X < -0.5f)
                    {
                        if (selectedLevelUpChoice != levelUpChoices[0])
                        {
                            int index = levelUpChoices.IndexOf(levelUpChoices.Where(a => a == selectedLevelUpChoice).First()) - 1;
                            selectedLevelUpChoice = levelUpChoices[index];
                        }
                        stateChangeTime = 0f;
                    }
                    else if (kState.IsKeyDown(Keys.Right) || gState.DPad.Right == ButtonState.Pressed || gState.ThumbSticks.Left.X > 0.5f)
                    {
                        if (selectedLevelUpChoice != levelUpChoices.Last())
                        {
                            int index = levelUpChoices.IndexOf(levelUpChoices.Where(a => a == selectedLevelUpChoice).First()) + 1;
                            selectedLevelUpChoice = levelUpChoices[index];
                        }
                        stateChangeTime = 0f;
                    }
                }
            }
            else
            {
                if (stateChangeTime > .1f && (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.P)))
                {
                    gameState = gameState == GameState.Running ? GameState.Paused : GameState.Running;
                    stateChangeTime = 0f;
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Loaded = false;
                    retVal = "main-menu";
                }
                else if (player.Get<EntityStatus>().State == State.Dead)
                {
                    Loaded = false;
                    retVal = "game-over";
                }
                else
                {
                    if (gameState == GameState.Running)
                    {
                        totalGameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        foreach (var system in updateSystems)
                        {
                            system.Update(gameTime, totalGameTime);
                        }
                    }
                    stateChangeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    Player playerInfo = player.Get<Player>();
                    if (playerInfo.ExperienceToNextLevel <= 0)
                    {
                        stateChangeTime = 0f;
                        playerInfo.Level++;
                        playerInfo.ExperienceToNextLevel += ExperienceHelper.ExperienceRequiredForLevel(playerInfo.Level + 1);
                        player.Set(playerInfo);

                        gameState = GameState.LevelUp;

                        RandomTable<PickupType> pickupTable = new RandomTable<PickupType>()
                            .Add(PickupType.AttackSpeed, 10)
                            .Add(PickupType.Damage, 10)
                            .Add(PickupType.MoveSpeed, 3)
                            .Add(PickupType.SpellEffectChance, 1)
                            .Add(PickupType.Pierce, 1)
                            .Add(PickupType.AreaOfEffect, 1);

                        levelUpChoices.Clear();
                        for(int i = 0; i < 4; i++)
                        {
                            PickupType choice;
                            do
                            {
                                choice = pickupTable.Roll(random);
                            }while(levelUpChoices.Contains(choice));

                            levelUpChoices.Add(choice);
                        }
                        selectedLevelUpChoice = levelUpChoices[0];
                    }
                }
            }

            return retVal;
        }

        public override void Draw(GameTime gameTime, Matrix transformMatrix, params object[] values)
        {
            if (gameState != GameState.LevelUp)
            {
                for (int layer = 1; layer < 5; layer++)
                {
                    foreach (var system in renderSystems)
                    {
                        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: transformMatrix);
                        system.Render(gameTime, _spriteBatch, textures, player, totalGameTime, gameState, layer);
                        _spriteBatch.End();
                    }
                }
            }
            else
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: transformMatrix);

                _spriteBatch.DrawString(
                    fonts["Font"],
                    "Level Up! Select an upgrade:",
                    new Vector2(_graphics.PreferredBackBufferWidth / 6 - 116, _graphics.PreferredBackBufferHeight / 6 - 64),
                    Color.White
                );

                int counter = -136;
                foreach (var levelUpChoice in levelUpChoices)
                {
                    _spriteBatch.Draw(
                        textures["LevelUpChoices"],
                        new Vector2(_graphics.PreferredBackBufferWidth / 6 + counter, _graphics.PreferredBackBufferHeight / 6 + 32),
                        LevelUpChoiceHelper.GetLevelUpChoiceButton(levelUpChoice, levelUpChoice == selectedLevelUpChoice),
                        Color.White,
                        0f,
                        new Vector2(32, 32),
                        1f,
                        SpriteEffects.None,
                        0f
                    );

                    counter += 80;
                }

                _spriteBatch.DrawString(
                    fonts["Font"],
                    PickupHelper.GetPickupDisplayTextForLevelUpChoice(selectedLevelUpChoice),
                    new Vector2(_graphics.PreferredBackBufferWidth / 6 - PickupHelper.GetPickupDisplayTextForLevelUpChoice(selectedLevelUpChoice).Length * 4.5f, _graphics.PreferredBackBufferHeight / 6 + 96),
                    Color.White
                );

                _spriteBatch.End();
            }
        }
    }
}
