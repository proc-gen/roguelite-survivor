using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Physics;
using RogueliteSurvivor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class EnemySpawnSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription playerQuery = new QueryDescription()
                                            .WithAll<Player, Position>();
        Dictionary<string, Texture2D> textures;
        Random random;
        Box2D.NetStandard.Dynamics.World.World physicsWorld;
        GraphicsDeviceManager graphics;
        RandomTable<string> enemyTable;
        RandomTable<PickupType> pickupTable;

        int enemyCount = 20;
        int difficulty = 1;
        int increaseAfterSeconds = 15;

        public EnemySpawnSystem(World world, Dictionary<string, Texture2D> textures, Box2D.NetStandard.Dynamics.World.World physicsWorld, GraphicsDeviceManager graphics)
            : base(world, new QueryDescription()
                                .WithAll<Enemy>())
        { 
            this.textures = textures;
            this.physicsWorld = physicsWorld;
            this.graphics = graphics;

            random = new Random();
            setDifficulty(0);
        }

        public void Update(GameTime gameTime, float totalElapsedTime) 
        {
            int numEnemies = 0;

            if(((int)totalElapsedTime) % increaseAfterSeconds == 0)
            {
                setDifficulty((int) totalElapsedTime);
            }

            Vector2 offset = new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight / 6);
            Position? player = null;
            world.Query(in playerQuery, (ref Position playerPos) =>
            {
                if (!player.HasValue)
                {
                    player = playerPos;
                }
            });

            world.Query(in query, (in Entity entity, ref Enemy enemy, ref Pickup pickup, ref Position position) =>
            {
                if(enemy.State == EntityState.Dead)
                {
                    if (pickup.Type != PickupType.None)
                    {
                        createPickup(pickup, position);
                    }
                    world.Destroy(entity);
                }
                else
                {
                    numEnemies++;
                }
            });

            if(numEnemies < enemyCount)
            {
                for(int i = numEnemies; i < enemyCount; i++)
                {
                    createEnemy(player, offset);
                }
            }
        }

        private System.Numerics.Vector2 getSpawnPosition(Vector2 playerPosition, Vector2 offset)
        {
            int x, y;
            do
            {
                x = random.Next(64, 736);
                y = random.Next(64, 736);
            } while ((x > (playerPosition.X - offset.X) && x < (playerPosition.X + offset.X)) && (y > (playerPosition.Y - offset.Y) && y < (playerPosition.Y + offset.Y)));

            return new System.Numerics.Vector2(x, y);
        }
    
        private void setDifficulty(int time)
        {
            difficulty = (time / increaseAfterSeconds) + 1;

            enemyCount = 20 * difficulty;

            enemyTable = new RandomTable<string>()
                .Add("VampireBat", 10)
                .Add("GhastlyBeholder", difficulty - 1)
                .Add("GraveRevenant", difficulty - 2)
                .Add("BloodLich", difficulty - 3);

            pickupTable = new RandomTable<PickupType>()
                .Add(PickupType.None, 20)
                .Add(PickupType.AttackSpeed, difficulty)
                .Add(PickupType.Damage, difficulty)
                .Add(PickupType.MoveSpeed, difficulty)
                .Add(PickupType.Health, 2 * difficulty);
        }
    
        private void createEnemy(Position? player, Vector2 offset)
        {
            switch (enemyTable.Roll(random))
            {
                case "VampireBat":
                    createVampireBat(player, offset);
                    break;
                case "GhastlyBeholder":
                    createGhastlyBeholder(player, offset);
                    break;
                case "GraveRevenant":
                    createGraveRevenant(player, offset);
                    break;
                case "BloodLich":
                    createBloodLich(player, offset);
                    break;
            }
        }

        private void createVampireBat(Position? player, Vector2 offset)
        {
            var entity = world.Create<Enemy, Position, Velocity, Speed, Animation, SpriteSheet, Target, Health, Damage, AttackSpeed, Body, Pickup>();

            var body = new BodyDef();
            body.position = getSpawnPosition(player.Value.XY, offset);
            body.fixedRotation = true;

            entity.SetRange(
                        new Enemy() { State = EntityState.Alive },
                        new Position() { XY = new Vector2(body.position.X, body.position.Y) },
                        new Velocity() { Vector = Vector2.Zero },
                        new Speed() { speed = 50f },
                        new Animation(0, 3, .1f, 2),
                        new SpriteSheet(textures["VampireBat"], "VampireBat", 4, 2),
                        new Target(),
                        new Health() { Current = 10, Max = 10 },
                        new Damage() { Amount = 2, BaseAmount = 2 },
                        new AttackSpeed() { BaseAttacksPerSecond = 1f, CurrentAttacksPerSecond = 1f, Cooldown = 0 },
                        BodyFactory.CreateCircularBody(entity, 16, physicsWorld, body),
                        createPickupForEnemy()
                    );
        }

        private void createGhastlyBeholder(Position? player, Vector2 offset)
        {
            var entity = world.Create<Enemy, Position, Velocity, Speed, Animation, SpriteSheet, Target, Health, Damage, AttackSpeed, Body, Pickup>();

            var body = new BodyDef();
            body.position = getSpawnPosition(player.Value.XY, offset);
            body.fixedRotation = true;

            entity.SetRange(
                        new Enemy() { State = EntityState.Alive },
                        new Position() { XY = new Vector2(body.position.X, body.position.Y) },
                        new Velocity() { Vector = Vector2.Zero },
                        new Speed() { speed = 100f },
                        new Animation(0, 3, .1f, 2),
                        new SpriteSheet(textures["GhastlyBeholder"], "GhastlyBeholder", 4, 2),
                        new Target(),
                        new Health() { Current = 10, Max = 10 },
                        new Damage() { Amount = 2, BaseAmount = 2 },
                        new AttackSpeed() { BaseAttacksPerSecond = 1f, CurrentAttacksPerSecond = 1f, Cooldown = 0 },
                        BodyFactory.CreateCircularBody(entity, 16, physicsWorld, body),
                        createPickupForEnemy()
                    );
        }

        private void createGraveRevenant(Position? player, Vector2 offset)
        {
            var entity = world.Create<Enemy, Position, Velocity, Speed, Animation, SpriteSheet, Target, Health, Damage, AttackSpeed, Body, Pickup>();

            var body = new BodyDef();
            body.position = getSpawnPosition(player.Value.XY, offset);
            body.fixedRotation = true;

            entity.SetRange(
                        new Enemy() { State = EntityState.Alive },
                        new Position() { XY = new Vector2(body.position.X, body.position.Y) },
                        new Velocity() { Vector = Vector2.Zero },
                        new Speed() { speed = 25f },
                        new Animation(0, 3, .1f, 2),
                        new SpriteSheet(textures["GraveRevenant"], "GraveRevenant", 4, 2),
                        new Target(),
                        new Health() { Current = 10, Max = 10 },
                        new Damage() { Amount = 2, BaseAmount = 2 },
                        new AttackSpeed() { BaseAttacksPerSecond = 1f, CurrentAttacksPerSecond = 1f, Cooldown = 0 },
                        BodyFactory.CreateCircularBody(entity, 16, physicsWorld, body),
                        createPickupForEnemy()
                    );
        }

        private void createBloodLich(Position? player, Vector2 offset)
        {
            var entity = world.Create<Enemy, Position, Velocity, Speed, Animation, SpriteSheet, Target, Health, Damage, AttackSpeed, Body, Pickup>();

            var body = new BodyDef();
            body.position = getSpawnPosition(player.Value.XY, offset);
            body.fixedRotation = true;

            entity.SetRange(
                        new Enemy() { State = EntityState.Alive },
                        new Position() { XY = new Vector2(body.position.X, body.position.Y) },
                        new Velocity() { Vector = Vector2.Zero },
                        new Speed() { speed = 5f },
                        new Animation(0, 9, .1f, 2),
                        new SpriteSheet(textures["BloodLich"], "BloodLich", 10, 2),
                        new Target(),
                        new Health() { Current = 10, Max = 10 },
                        new Damage() { Amount = 2, BaseAmount = 2 },
                        new AttackSpeed() { BaseAttacksPerSecond = 1f, CurrentAttacksPerSecond = 1f, Cooldown = 0 },
                        BodyFactory.CreateCircularBody(entity, 32, physicsWorld, body),
                        createPickupForEnemy()
                    );
        }

        private Pickup createPickupForEnemy()
        {
            var pickup = new Pickup() { Type = pickupTable.Roll(random) };

            switch (pickup.Type)
            {
                case PickupType.AttackSpeed:
                    pickup.PickupAmount = .1f;
                    break;
                case PickupType.Damage:
                    pickup.PickupAmount = 1f;
                    break;
                case PickupType.MoveSpeed:
                    pickup.PickupAmount = 5000f;
                    break;
                case PickupType.Health:
                    pickup.PickupAmount = 5f;
                    break;
            }

            return pickup;
        }

        private void createPickup(Pickup pickup, Position position)
        {
            var entity = world.Create<PickupSprite, Position>();
            
            entity.SetRange(new PickupSprite() { Type = pickup.Type, PickupAmount = pickup.PickupAmount },
                new Position() { XY = new Vector2(position.XY.X, position.XY.Y) }
            );
        }
    }
}
