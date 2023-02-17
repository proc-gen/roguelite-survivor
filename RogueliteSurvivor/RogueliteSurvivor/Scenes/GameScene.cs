using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Physics;
using RogueliteSurvivor.Systems;
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
        private World world;
        private List<IUpdateSystem> updateSystems;
        private List<IRenderSystem> renderSystems;
        private Entity player;

        Box2D.NetStandard.Dynamics.World.World physicsWorld;
        System.Numerics.Vector2 gravity = System.Numerics.Vector2.Zero;

        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        public GameScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics) 
            : base(spriteBatch, contentManager, graphics)
        {
        }

        public override void LoadContent()
        {
            textures = new Dictionary<string, Texture2D>
            {
                { "tiles", Content.Load<Texture2D>("Tiles") },
                { "player", Content.Load<Texture2D>("Animated_Mage_Character") },
                { "vampire_bat", Content.Load<Texture2D>("VampireBat") },
                { "SmallFireball", Content.Load<Texture2D>("small-fireball") },
                { "MediumFireball", Content.Load<Texture2D>("medium-fireball") },
                { "LargeFireball", Content.Load<Texture2D>("large-fireball") },
                { "StatBar", Content.Load<Texture2D>("StatBar") },
                { "HealthBar", Content.Load<Texture2D>("HealthBar") }
            };

            fonts = new Dictionary<string, SpriteFont>()
            {
                { "Font", Content.Load<SpriteFont>("Font") },
            };

            world = World.Create();
            physicsWorld = new Box2D.NetStandard.Dynamics.World.World(gravity);
            physicsWorld.SetContactListener(new GameContactListener());

            updateSystems = new List<IUpdateSystem>
            {
                new PlayerInputSystem(world),
                new TargetingSystem(world),
                new EnemyAISystem(world),
                new AnimationSetSystem(world),
                new AnimationUpdateSystem(world),
                new CollisionSystem(world, physicsWorld),
                new EnemySpawnSystem(world, textures, physicsWorld, _graphics),
                new AttackSystem(world, textures, physicsWorld),
                new ProjectileCleanupSystem(world, physicsWorld),
            };

            renderSystems = new List<IRenderSystem>
            {
                new RenderMapSystem(world, _graphics),
                new RenderSpriteSystem(world, _graphics),
                new RenderHudSystem(world, _graphics, fonts),
            };

            var mapEntity = world.Create<Map, MapInfo>();
            mapEntity.SetRange(new Map(), new MapInfo(Path.Combine(Content.RootDirectory, "Demo.tmx"), Content.RootDirectory + "/", physicsWorld, mapEntity));

            var body = new Box2D.NetStandard.Dynamics.Bodies.BodyDef();
            body.position = new System.Numerics.Vector2(384, 384);
            body.fixedRotation = true;

            player = world.Create<Player, Position, Velocity, Speed, Animation, SpriteSheet, Target, Spell, AttackSpeed, Health, KillCount, Body>();

            player.SetRange(
                new Player(),
                new Position() { XY = new Vector2(384, 384) },
                new Velocity() { Vector = Vector2.Zero },
                new Speed() { speed = 16000f },
                new Animation(1, 1, .1f, 4),
                new SpriteSheet(textures["player"], "player", 3, 8),
                new Target(),
                new Spell() { CurrentSpell = AvailableSpells.SmallFireball },
                new AttackSpeed() { BaseAttackSpeed = .5f, CurrentAttackSpeed = .5f, Cooldown = 0f },
                new Health() { Current = 100, Max = 100 },
                new KillCount() { Count = 0 },
                BodyFactory.CreateCircularBody(player, 16, physicsWorld, body, 9999)
            );

            Task.Delay(3000);

            Loaded = true;
        }

        public override string Update(GameTime gameTime, params object[] values)
        {
            string retVal = string.Empty;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                retVal = "main-menu";
            }
            else
            {
                foreach (var system in updateSystems)
                {
                    system.Update(gameTime);
                }
            }

            return retVal;
        }

        public override void Draw(GameTime gameTime, params object[] values)
        {
            foreach (var system in renderSystems)
            {
                system.Render(gameTime, _spriteBatch, textures, player);
            }
        }
    }
}
