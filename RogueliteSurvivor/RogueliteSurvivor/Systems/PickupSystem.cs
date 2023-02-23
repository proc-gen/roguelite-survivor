using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace RogueliteSurvivor.Systems
{
    public class PickupSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription playerQuery = new QueryDescription()
                                            .WithAll<Player>();

        public PickupSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<PickupSprite, Position>())
        { }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            Entity? player = null;
            Position? playerPos = null;
            world.Query(in playerQuery, (in Entity entity, ref Position pos) =>
            {
                if (!playerPos.HasValue)
                {
                    player = entity;
                    playerPos = pos;
                }
            });

            world.Query(in query, (in Entity entity, ref PickupSprite sprite, ref Position pos) =>
            {
                if(Vector2.Distance(playerPos.Value.XY, pos.XY) < 16)
                {
                    switch(sprite.Type)
                    {
                        case PickupType.AttackSpeed:
                            processAttackSpeed(entity, player, sprite);
                            break;
                        case PickupType.Damage:
                            processDamage(entity, player, sprite);
                            break;
                        case PickupType.SpellEffectChance:
                            processSpellEffectChance(entity, player, sprite);
                            break;
                        case PickupType.MoveSpeed:
                            var moveSpeed = player.Value.Get<Speed>();
                            moveSpeed.speed += sprite.PickupAmount;
                            player.Value.Set(moveSpeed);
                            world.Destroy(entity);
                            break;
                        case PickupType.Health:
                            var health = player.Value.Get<Health>();
                            if(health.Current < health.Max)
                            {
                                health.Current = int.Min(health.Max, (int)sprite.PickupAmount + health.Current);
                                player.Value.Set(health);
                                world.Destroy(entity);
                            }
                            break;
                    }
                }
                else
                {
                    sprite.Count += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(sprite.Count > sprite.MaxCount)
                    {
                        sprite.Count = 0;
                        sprite.Current += sprite.Increment;

                        if(sprite.Current == sprite.Max || sprite.Current == sprite.Min)
                        {
                            sprite.Increment *= -1;
                        }
                    }
                }
            });
        }

        private void processAttackSpeed(Entity entity, Entity? player, PickupSprite sprite)
        {
            if (player.Value.TryGet(out Spell1 spell1))
            {
                spell1.CurrentAttacksPerSecond += sprite.PickupAmount * spell1.BaseAttacksPerSecond;
                player.Value.Set(spell1);
            }
            if (player.Value.TryGet(out Spell2 spell2))
            {
                spell2.CurrentAttacksPerSecond += sprite.PickupAmount * spell2.BaseAttacksPerSecond;
                player.Value.Set(spell2);
            }
            world.Destroy(entity);
        }

        private void processDamage(Entity entity, Entity? player, PickupSprite sprite)
        {
            if (player.Value.TryGet(out Spell1 spell1))
            {
                spell1.CurrentDamage += sprite.PickupAmount * spell1.BaseDamage;
                player.Value.Set(spell1);
            }
            if (player.Value.TryGet(out Spell2 spell2))
            {
                spell2.CurrentDamage += sprite.PickupAmount * spell2.BaseDamage;
                player.Value.Set(spell2);
            }
            world.Destroy(entity);
        }

        private void processSpellEffectChance(Entity entity, Entity? player, PickupSprite sprite)
        {
            if (player.Value.TryGet(out Spell1 spell1))
            {
                spell1.CurrentEffectChance += sprite.PickupAmount * spell1.BaseEffectChance;
                player.Value.Set(spell1);
            }
            if (player.Value.TryGet(out Spell2 spell2))
            {
                spell2.CurrentEffectChance += sprite.PickupAmount * spell2.BaseEffectChance;
                player.Value.Set(spell2);
            }
            world.Destroy(entity);
        }
    }
}
