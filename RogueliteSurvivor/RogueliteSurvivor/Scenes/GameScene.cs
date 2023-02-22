using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.ComponentFactories;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Physics;
using RogueliteSurvivor.Systems;
using RogueliteSurvivor.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public GameScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld)
            : base(spriteBatch, contentManager, graphics, world, physicsWorld)
        {
        }

        public void SetGameSettings(GameSettings gameSettings)
        {
            this.gameSettings = gameSettings;
        }

        public override void LoadContent()
        {
            textures = new Dictionary<string, Texture2D>
            {
                { "tiles", Content.Load<Texture2D>(Path.Combine("Maps", "Tiles")) },

                { "player", Content.Load<Texture2D>(Path.Combine("Player", "Animated_Mage_Character")) },
                { "player_blue", Content.Load<Texture2D>(Path.Combine("Player", "Animated_Mage_Character_blue")) },
                { "player_yellow", Content.Load<Texture2D>(Path.Combine("Player", "Animated_Mage_Character_yellow")) },

                { "VampireBat", Content.Load<Texture2D>(Path.Combine("Enemies", "VampireBat")) },
                { "GhastlyBeholder", Content.Load<Texture2D>(Path.Combine("Enemies", "GhastlyBeholderIdleSide")) },
                { "GraveRevenant", Content.Load<Texture2D>(Path.Combine("Enemies", "GraveRevenantIdleSide")) },
                { "BloodLich", Content.Load<Texture2D>(Path.Combine("Enemies", "BloodLichIdleSIde")) },

                { "Fireball", Content.Load<Texture2D>(Path.Combine("Spells", "fireball")) },
                { "IceShard", Content.Load<Texture2D>(Path.Combine("Spells", "ice-shard")) },
                { "LightningBlast", Content.Load<Texture2D>(Path.Combine("Spells", "lightning-blast")) },

                { "StatBar", Content.Load<Texture2D>(Path.Combine("Hud", "StatBar")) },
                { "HealthBar", Content.Load<Texture2D>(Path.Combine("Hud", "HealthBar")) },

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
            };

            if(world.CountEntities(new QueryDescription()) > 0)
            {
                List<Entity> entities = new List<Entity>();
                world.GetEntities(new QueryDescription(), entities);
                foreach(var entity in entities)
                {
                    world.Destroy(entity);
                }
            }

            if(physicsWorld.GetBodyCount() > 0)
            {
                var physicsBody = physicsWorld.GetBodyList();
                while(physicsBody != null) 
                {
                    var nextPhysicsBody = physicsBody.GetNext();
                    physicsWorld.DestroyBody(physicsBody);
                    physicsBody = nextPhysicsBody;
                };
            }

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
                new EnemySpawnSystem(world, textures, physicsWorld, _graphics),
                new AttackSystem(world, textures, physicsWorld),
                new ProjectileCleanupSystem(world, physicsWorld),
                new DeathSystem(world, textures, physicsWorld),
            };

            renderSystems = new List<IRenderSystem>
            {
                new RenderMapSystem(world, _graphics),
                new RenderPickupSystem(world, _graphics),
                new RenderSpriteSystem(world, _graphics),
                new RenderHudSystem(world, _graphics, fonts),
            };

            var mapEntity = world.Create<Map, MapInfo>();
            mapEntity.SetRange(new Map(), new MapInfo(Path.Combine(Content.RootDirectory, "Maps", "Demo.tmx"), Path.Combine(Content.RootDirectory, "Maps"), physicsWorld, mapEntity));

            var body = new BodyDef();
            body.position = new System.Numerics.Vector2(384, 384) / PhysicsConstants.PhysicsToPixelsRatio;
            body.fixedRotation = true;

            player = world.Create<Player, Position, Velocity, Speed, Animation, SpriteSheet, Target, Spell, Health, KillCount, Body>();

            player.SetRange(
                new Player() { State = EntityState.Alive },
                new Position() { XY = new Vector2(384, 384) },
                new Velocity() { Vector = Vector2.Zero },
                new Speed() { speed = 100f },
                new Animation(1, 1, .1f, 4),
                new SpriteSheet(textures[gameSettings.PlayerTexture], gameSettings.PlayerTexture, 3, 8),
                new Target(),
                SpellFactory.CreateSpell(gameSettings.StartingSpell),
                new Health() { Current = 100, Max = 100 },
                new KillCount() { Count = 0 },
                BodyFactory.CreateCircularBody(player, 16, physicsWorld, body, 99)
            );

            totalGameTime = 0;
            gameState = GameState.Running;

            Loaded = true;
        }

        public override string Update(GameTime gameTime, params object[] values)
        {
            string retVal = string.Empty;

            if(stateChangeTime > .1f && (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.P)))
            {
                gameState = gameState == GameState.Running ? GameState.Paused : GameState.Running;
                stateChangeTime = 0f;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Loaded = false;
                retVal = "main-menu";
            }
            else if(player.Get<Player>().State == EntityState.Dead)
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
            }

            return retVal;
        }

        public override void Draw(GameTime gameTime, Matrix transformMatrix, params object[] values)
        {
            for (int layer = 1; layer < 3; layer++)
            {
                foreach (var system in renderSystems)
                {
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: transformMatrix);
                    system.Render(gameTime, _spriteBatch, textures, player, totalGameTime, gameState, layer);
                    _spriteBatch.End();

                }
            }
        }
    }
}
