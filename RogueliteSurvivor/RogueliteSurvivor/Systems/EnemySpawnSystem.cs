using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.ComponentFactories;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Extensions;
using RogueliteSurvivor.Physics;
using RogueliteSurvivor.Utils;
using System;
using System.Collections.Generic;

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
        Dictionary<string, EnemyContainer> enemyContainers;
        Dictionary<Spells, SpellContainer> spellContainers;
        MapContainer mapContainer;

        int enemyCount = 20;
        int difficulty = 1;
        int increaseAfterSeconds = 15;
        int lastSet = 0;

        public EnemySpawnSystem(World world, Dictionary<string, Texture2D> textures, Box2D.NetStandard.Dynamics.World.World physicsWorld, GraphicsDeviceManager graphics, Dictionary<string, EnemyContainer> enemyContainers, Dictionary<Spells, SpellContainer> spellContainers, MapContainer mapContainer)
            : base(world, new QueryDescription()
                                .WithAll<Enemy>())
        {
            this.textures = textures;
            this.physicsWorld = physicsWorld;
            this.graphics = graphics;
            this.enemyContainers = enemyContainers;
            this.spellContainers = spellContainers;
            this.mapContainer = mapContainer;

            random = new Random();
            setDifficulty(0);
        }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            int numEnemies = 0;

            if (((int)totalElapsedTime) % increaseAfterSeconds == 0 && lastSet != (int)totalElapsedTime)
            {
                setDifficulty((int)totalElapsedTime);
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

            world.Query(in query, (in Entity entity, ref Enemy enemy, ref Pickup pickup, ref Position position, ref EntityStatus entityStatus) =>
            {
                if (entityStatus.State == State.Dead)
                {
                    if (entity.IsAlive())
                    {
                        if (pickup.Type != PickupType.None)
                        {
                            createPickup(pickup, position);
                        }

                        world.TryDestroy(entity);
                    }
                }
                else
                {
                    numEnemies++;
                }
            });

            if (numEnemies < enemyCount)
            {
                for (int i = numEnemies; i < enemyCount; i++)
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
                x = random.Next(int.Max(mapContainer.SpawnMinX, (int)playerPosition.X - 200), int.Min(mapContainer.SpawnMaxX, (int)playerPosition.X + 200));
                y = random.Next(int.Max(mapContainer.SpawnMinY, (int)playerPosition.Y - 200), int.Min(mapContainer.SpawnMaxY, (int)playerPosition.Y + 200));
            } while ((x > (playerPosition.X - offset.X) && x < (playerPosition.X + offset.X)) && (y > (playerPosition.Y - offset.Y) && y < (playerPosition.Y + offset.Y)));

            return new System.Numerics.Vector2(x, y);
        }

        private void setDifficulty(int time)
        {
            lastSet = time;
            difficulty = (time / increaseAfterSeconds) + 1;

            enemyCount = 20 * difficulty;

            enemyTable = new RandomTable<string>()
                .Add("VampireBat", 10 + difficulty)
                .Add("GhastlyBeholder", difficulty - 1)
                .Add("GraveRevenant", difficulty - 2)
                .Add("BloodLich", difficulty - 3);

            pickupTable = new RandomTable<PickupType>()
                .Add(PickupType.None, 20 + difficulty)
                .Add(PickupType.AttackSpeed, difficulty)
                .Add(PickupType.Damage, difficulty)
                .Add(PickupType.MoveSpeed, difficulty)
                .Add(PickupType.Health, difficulty)
                .Add(PickupType.SpellEffectChance, difficulty);
        }

        private void createEnemy(Position? player, Vector2 offset)
        {
            createEnemyFromContainer(enemyTable.Roll(random), player, offset);
        }

        private void createEnemyFromContainer(string enemyType, Position? player, Vector2 offset)
        {
            if (!string.IsNullOrEmpty(enemyType))
            {
                EnemyContainer container = enemyContainers[enemyType];

                var entity = world.Create<Enemy, EntityStatus, Position, Velocity, Speed, Animation, SpriteSheet, Target, Health, Damage, Spell1, Body, Pickup>();

                var body = new BodyDef();
                body.position = getSpawnPosition(player.Value.XY, offset) / PhysicsConstants.PhysicsToPixelsRatio;
                body.fixedRotation = true;

                entity.SetRange(
                            new Enemy(),
                            new EntityStatus(),
                            new Position() { XY = new Vector2(body.position.X, body.position.Y) },
                            new Velocity() { Vector = Vector2.Zero },
                            new Speed() { speed = container.Speed },
                            new Animation(container.Animation.FirstFrame, container.Animation.LastFrame, container.Animation.PlaybackSpeed, container.Animation.NumDirections),
                            new SpriteSheet(textures[container.Name], container.Name, container.SpriteSheet.FramesPerRow, container.SpriteSheet.FramesPerColumn),
                            new Target(),
                            new Health() { Current = container.Health, Max = container.Health },
                            new Damage() { Amount = container.Damage, BaseAmount = container.Damage },
                            SpellFactory.CreateSpell<Spell1>(spellContainers[container.Spell]),
                            BodyFactory.CreateCircularBody(entity, container.Width, physicsWorld, body),
                            createPickupForEnemy()
                        );
            }
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
                    pickup.PickupAmount = .25f;
                    break;
                case PickupType.MoveSpeed:
                    pickup.PickupAmount = 5f;
                    break;
                case PickupType.Health:
                    pickup.PickupAmount = 5f;
                    break;
                case PickupType.SpellEffectChance:
                    pickup.PickupAmount = .25f;
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
