using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Extensions;

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
            float radiusMultiplier = 0;
            world.Query(in playerQuery, (in Entity entity, ref Position pos, ref AreaOfEffect areaOfEffect) =>
            {
                if (!playerPos.HasValue)
                {
                    player = entity;
                    playerPos = pos;
                    radiusMultiplier = areaOfEffect.Radius;
                }
            });

            world.Query(in query, (in Entity entity, ref PickupSprite sprite, ref Position pos) =>
            {
                if (Vector2.Distance(playerPos.Value.XY, pos.XY) < (16 * radiusMultiplier))
                {
                    switch (sprite.Type)
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
                            world.TryDestroy(entity);
                            break;
                        case PickupType.Pierce:
                            var pierce = player.Value.Get<Pierce>();
                            pierce.Num += (int)sprite.PickupAmount;
                            player.Value.Set(pierce);
                            world.TryDestroy(entity);
                            break;
                        case PickupType.AreaOfEffect:
                            var areaOfAffect = player.Value.Get<AreaOfEffect>();
                            areaOfAffect.Radius += sprite.PickupAmount;
                            player.Value.Set(areaOfAffect);
                            world.TryDestroy(entity);
                            break;
                        case PickupType.Health:
                            var health = player.Value.Get<Health>();
                            if (health.Current < health.Max)
                            {
                                health.Current = int.Min(health.Max, (int)sprite.PickupAmount + health.Current);
                                player.Value.Set(health);
                                world.TryDestroy(entity);
                            }
                            break;
                    }
                }
                else
                {
                    sprite.Count += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (sprite.Count > sprite.MaxCount)
                    {
                        sprite.Count = 0;
                        sprite.Current += sprite.Increment;

                        if (sprite.Current == sprite.Max || sprite.Current == sprite.Min)
                        {
                            sprite.Increment *= -1;
                        }
                    }
                }
            });
        }

        private void processAttackSpeed(Entity entity, Entity? player, PickupSprite sprite)
        {
            AttackSpeed attackSpeed = player.Value.Get<AttackSpeed>();
            attackSpeed.CurrentAttackSpeed += attackSpeed.BaseAttackSpeed * sprite.PickupAmount;

            if (player.Value.TryGet(out Spell1 spell1))
            {
                spell1.CurrentAttacksPerSecond = attackSpeed.CurrentAttackSpeed * spell1.BaseAttacksPerSecond;
                player.Value.Set(spell1);
            }
            if (player.Value.TryGet(out Spell2 spell2))
            {
                spell2.CurrentAttacksPerSecond = attackSpeed.CurrentAttackSpeed * spell2.BaseAttacksPerSecond;
                player.Value.Set(spell2);
            }

            player.Value.Set(attackSpeed);
            world.TryDestroy(entity);
        }

        private void processDamage(Entity entity, Entity? player, PickupSprite sprite)
        {
            SpellDamage spellDamage = player.Value.Get<SpellDamage>();
            spellDamage.CurrentSpellDamage += spellDamage.BaseSpellDamage * sprite.PickupAmount;

            if (player.Value.TryGet(out Spell1 spell1))
            {
                spell1.CurrentDamage = spellDamage.CurrentSpellDamage * spell1.BaseDamage;
                player.Value.Set(spell1);
            }
            if (player.Value.TryGet(out Spell2 spell2))
            {
                spell2.CurrentDamage = spellDamage.CurrentSpellDamage * spell2.BaseDamage;
                player.Value.Set(spell2);
            }

            player.Value.Set(spellDamage);
            world.TryDestroy(entity);
        }

        private void processSpellEffectChance(Entity entity, Entity? player, PickupSprite sprite)
        {
            SpellEffectChance spellEffectChance = player.Value.Get<SpellEffectChance>();
            spellEffectChance.CurrentSpellEffectChance += spellEffectChance.BaseSpellEffectChance * sprite.PickupAmount;

            if (player.Value.TryGet(out Spell1 spell1))
            {
                spell1.CurrentEffectChance = spellEffectChance.CurrentSpellEffectChance * spell1.BaseEffectChance;
                player.Value.Set(spell1);
            }
            if (player.Value.TryGet(out Spell2 spell2))
            {
                spell2.CurrentEffectChance = spellEffectChance.CurrentSpellEffectChance * spell2.BaseEffectChance;
                player.Value.Set(spell2);
            }

            player.Value.Set(spellEffectChance);
            world.TryDestroy(entity);
        }
    }
}
