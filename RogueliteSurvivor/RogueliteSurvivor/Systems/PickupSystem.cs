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
                            var attackSpeed = player.Value.Get<AttackSpeed>();
                            attackSpeed.CurrentAttacksPerSecond += sprite.PickupAmount * attackSpeed.BaseAttacksPerSecond;
                            player.Value.Set(attackSpeed);
                            world.Destroy(entity);
                            break;
                        case PickupType.Damage:
                            var spell = player.Value.Get<Spell>();
                            spell.CurrentDamage += sprite.PickupAmount * spell.BaseDamage;
                            player.Value.Set(spell);
                            world.Destroy(entity);
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
    }
}
