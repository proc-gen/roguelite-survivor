﻿using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.ComponentFactories;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
using RogueliteSurvivor.Extensions;
using RogueliteSurvivor.Helpers;
using RogueliteSurvivor.Physics;
using RogueliteSurvivor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

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
        int lastSet = -1;
        int maxEnemiesPerUpdate = 200;

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
        }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            int numEnemies = 0;

            Vector2 offset = new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight / 6);
            Position? player = null;
            world.Query(in playerQuery, (ref Position playerPos) =>
            {
                if (!player.HasValue)
                {
                    player = playerPos;
                }
            });

            if (lastSet != (int)totalElapsedTime)
            {
                setDifficulty((int)totalElapsedTime, player, offset);
            }

            world.Query(in query, (in Entity entity, ref Enemy enemy, ref Pickup pickup, ref Position position, ref EntityStatus entityStatus) =>
            {
                if (entityStatus.State == State.Dead)
                {
                    if (pickup.Type != PickupType.None)
                    {
                        createPickup(pickup, position);
                    }

                    world.TryDestroy(entity);
                }
                else
                {
                    numEnemies++;
                }
            });

            if (numEnemies < enemyCount)
            {
                int max = int.Min(enemyCount - numEnemies, maxEnemiesPerUpdate);
                for (int i = 0; i < max; i++)
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
                x = random.Next(int.Max(mapContainer.SpawnMinX, (int)playerPosition.X - 300), int.Min(mapContainer.SpawnMaxX, (int)playerPosition.X + 300));
                y = random.Next(int.Max(mapContainer.SpawnMinY, (int)playerPosition.Y - 300), int.Min(mapContainer.SpawnMaxY, (int)playerPosition.Y + 300));
            } while ((x > (playerPosition.X - offset.X) && x < (playerPosition.X + offset.X)) && (y > (playerPosition.Y - offset.Y) && y < (playerPosition.Y + offset.Y)));

            return new System.Numerics.Vector2(x, y);
        }

        private void setDifficulty(int time, Position? player, Vector2 offset)
        {
            lastSet = time;
            difficulty = (time / increaseAfterSeconds) + 1;

            var enemyWave = mapContainer.EnemyWaves.Where(a => a.Start == time).FirstOrDefault();

            if(enemyWave != null)
            {
                if (enemyWave.Repeat)
                {
                    enemyTable = new RandomTable<string>();
                    foreach(var enemyWeight in enemyWave.Enemies)
                    {
                        enemyTable.Add(enemyWeight.Type, enemyWeight.Weight);
                    }
                    enemyCount = enemyWave.MaxEnemies;
                }
                else
                {
                    var tempTable = new RandomTable<string>();
                    foreach (var enemyWeight in enemyWave.Enemies)
                    {
                        tempTable.Add(enemyWeight.Type, enemyWeight.Weight);
                    }
                    for(int i = 0; i < enemyWave.MaxEnemies; i++)
                    {
                        createEnemyFromContainer(tempTable.Roll(random), player, offset);
                    }
                }
            }

            pickupTable = new RandomTable<PickupType>()
                .Add(PickupType.None, 40 - difficulty)
                .Add(PickupType.Health, 2);
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
                
                var body = new BodyDef();
                body.position = getSpawnPosition(player.Value.XY, offset) / PhysicsConstants.PhysicsToPixelsRatio;
                body.fixedRotation = true;
                
                var entity = world.Create<Enemy, EntityStatus, Position, Velocity, Speed, Animation, SpriteSheet, Target, Health, Damage, Spell1, Body, Pickup, Experience>();
                entity.Set(
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
                createPickupForEnemy(),
                new Experience(container.Experience)
                );
            }
        }

        private Pickup createPickupForEnemy()
        {
            var pickup = new Pickup() { Type = pickupTable.Roll(random) };
            pickup.PickupAmount = PickupHelper.GetPickupAmount(pickup.Type);

            return pickup;
        }

        private void createPickup(Pickup pickup, Position position)
        {
            world.Create(         
                new PickupSprite() { Type = pickup.Type, PickupAmount = pickup.PickupAmount },
                new Position() { XY = new Vector2(position.XY.X, position.XY.Y) }
            );
        }
    }
}
