using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
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
        
        private List<IUpdateSystem> updateSystems;
        private List<IRenderSystem> renderSystems;
        private Entity player;

        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        private float totalGameTime = 0f;

        public GameScene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld)
            : base(spriteBatch, contentManager, graphics, world, physicsWorld)
        {
        }

        public override void LoadContent()
        {
            textures = new Dictionary<string, Texture2D>
            {
                { "tiles", Content.Load<Texture2D>(Path.Combine("Maps", "Tiles")) },
                { "player", Content.Load<Texture2D>(Path.Combine("Player", "Animated_Mage_Character")) },
                { "VampireBat", Content.Load<Texture2D>(Path.Combine("Enemies", "VampireBat")) },
                { "GhastlyBeholder", Content.Load<Texture2D>(Path.Combine("Enemies", "GhastlyBeholderIdleSide")) },
                { "GraveRevenant", Content.Load<Texture2D>(Path.Combine("Enemies", "GraveRevenantIdleSide")) },
                { "BloodLich", Content.Load<Texture2D>(Path.Combine("Enemies", "BloodLichIdleSIde")) },
                { "SmallFireball", Content.Load<Texture2D>(Path.Combine("Spells", "small-fireball")) },
                { "MediumFireball", Content.Load<Texture2D>(Path.Combine("Spells", "medium-fireball")) },
                { "LargeFireball", Content.Load<Texture2D>(Path.Combine("Spells", "large-fireball")) },
                { "StatBar", Content.Load<Texture2D>(Path.Combine("Hud", "StatBar")) },
                { "HealthBar", Content.Load<Texture2D>(Path.Combine("Hud", "HealthBar")) }
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
            mapEntity.SetRange(new Map(), new MapInfo(Path.Combine(Content.RootDirectory, "Maps", "Demo.tmx"), Path.Combine(Content.RootDirectory, "Maps"), physicsWorld, mapEntity));

            var body = new Box2D.NetStandard.Dynamics.Bodies.BodyDef();
            body.position = new System.Numerics.Vector2(384, 384);
            body.fixedRotation = true;

            player = world.Create<Player, Position, Velocity, Speed, Animation, SpriteSheet, Target, Spell, AttackSpeed, Health, KillCount, Body>();

            player.SetRange(
                new Player() { State = EntityState.Alive },
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

            totalGameTime = 0;

            Task.Delay(3000);

            Loaded = true;
        }

        public override string Update(GameTime gameTime, params object[] values)
        {
            string retVal = string.Empty;

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
                totalGameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                foreach (var system in updateSystems)
                {
                    system.Update(gameTime, totalGameTime);
                }
            }

            return retVal;
        }

        public override void Draw(GameTime gameTime, params object[] values)
        {
            foreach (var system in renderSystems)
            {
                system.Render(gameTime, _spriteBatch, textures, player, totalGameTime);
            }
        }
    }
}
