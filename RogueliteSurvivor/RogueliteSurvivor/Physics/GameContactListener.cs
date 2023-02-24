using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Physics
{
    public class GameContactListener : ContactListener
    {
        public override void BeginContact(in Contact contact)
        {
            checkContact(contact);
        }

        public override void EndContact(in Contact contact)
        {

        }

        private void checkContact(Contact contact)
        {
            if (contact.GetFixtureA().Body.UserData != null && contact.GetFixtureB().Body.UserData != null)
            {
                Entity a = (Entity)contact.GetFixtureA().Body.UserData;
                Entity b = (Entity)contact.GetFixtureB().Body.UserData;

                if (a.IsAlive() && b.IsAlive())
                {
                    if ((a.Has<Player>() && b.Has<Enemy>()) || (b.Has<Player>() && a.Has<Enemy>()))
                    {
                        damagePlayer(a, b);
                    }
                    else if ((a.Has<Projectile>() && b.Has<Enemy>()) || (b.Has<Projectile>() && a.Has<Enemy>()))
                    {
                        Projectile p;
                        EntityState state;
                        Damage damage;
                        Owner owner;
                        if (!a.TryGet(out p))
                        {
                            b.TryGet(out p);
                            state = p.State;
                            damage = b.Get<Damage>();
                            owner = b.Get<Owner>();
                            setProjectileDead(b, p);
                        }
                        else
                        {
                            state = p.State;
                            damage = a.Get<Damage>();
                            owner = a.Get<Owner>();
                            setProjectileDead(a, p);
                        }

                        if (state == EntityState.Alive)
                        {
                            damageEnemy(a, b, damage, owner);
                        }
                    }
                    else if ((a.Has<SingleTarget>() && b.Has<Enemy>()) || (b.Has<SingleTarget>() && a.Has<Enemy>()))
                    {
                        SingleTarget p;
                        Damage damage;
                        Owner owner;
                        if (!a.TryGet(out p))
                        {
                            b.TryGet(out p);
                            damage = b.Get<Damage>();
                            owner = b.Get<Owner>();
                        }
                        else
                        {
                            damage = a.Get<Damage>();
                            owner = a.Get<Owner>();
                        }

                        damageEnemy(a, b, damage, owner);
                    }
                    else if ((a.Has<Projectile>() && b.Has<Map>()) || (b.Has<Projectile>() && a.Has<Map>()))
                    {
                        if (!a.TryGet(out Projectile p))
                        {
                            b.TryGet(out p);
                            setProjectileDead(b, p);
                        }
                        else
                        {
                            setProjectileDead(a, p);
                        }
                    }
                }
            }
        }

        private void damageEnemy(Entity a, Entity b, Damage damage, Owner owner)
        {
            Enemy e;
            if (!a.TryGet(out e))
            {
                b.TryGet(out e);
                setEnemyHealthAndState(b, e, damage, owner);
            }
            else
            {
                setEnemyHealthAndState(a, e, damage, owner);
            }
        }

        private void setEnemyHealthAndState(Entity entity, Enemy enemy, Damage damage, Owner owner)
        {
            if (enemy.State == EntityState.Alive)
            {
                Health health = entity.Get<Health>();
                health.Current -= (int)damage.Amount;
                if (health.Current < 1)
                {
                    enemy.State = EntityState.ReadyToDie;
                    entity.Set(enemy);
                    KillCount killCount = owner.Entity.Get<KillCount>();
                    killCount.Count++;
                    owner.Entity.Set(killCount);
                }
                else
                {
                    Animation anim = entity.Get<Animation>();
                    anim.Overlay = Microsoft.Xna.Framework.Color.Red;
                    entity.SetRange(health, anim);
                    if(damage.SpellEffect != SpellEffects.None)
                    {
                        switch(damage.SpellEffect)
                        {
                            case SpellEffects.Burn:
                                if (!entity.Has<Burn>())
                                {
                                    entity.Add<Burn>();
                                }
                                entity.Set(new Burn() { TimeLeft = 5f, TickRate = .5f, NextTick = .5f });
                                break;
                            case SpellEffects.Slow:
                                if (!entity.Has<Slow>())
                                {
                                    entity.Add<Slow>();
                                }
                                entity.Set(new Slow() { TimeLeft = 5f });
                                break;
                            case SpellEffects.Shock:
                                if (!entity.Has<Shock>())
                                {
                                    entity.Add<Shock>();
                                }
                                entity.Set(new Shock() { TimeLeft = 1f });
                                break;
                        }
                    }
                }
            }
        }

        private void setProjectileDead(Entity entity, Projectile projectile)
        {
            if (projectile.State == EntityState.Alive)
            {
                projectile.State = EntityState.ReadyToDie;
                entity.Set(projectile);
            }
        }

        private void damagePlayer(Entity a, Entity b)
        {
            if (!a.TryGet(out Player e))
            {
                b.TryGet(out e);
                setPlayerHealthAndState(b, a, e);
            }
            else
            {
                setPlayerHealthAndState(a, b, e);
            }
        }

        private void setPlayerHealthAndState(Entity entity, Entity other, Player player)
        {   
            var attackSpeed = other.Get<Spell1>();
            if(attackSpeed.Cooldown > attackSpeed.CurrentAttackSpeed)
            {
                attackSpeed.Cooldown -= attackSpeed.CurrentAttackSpeed;
                Health health = entity.Get<Health>();
                Damage damage = other.Get<Damage>();
                health.Current -= (int)damage.Amount;
                Animation anim = entity.Get<Animation>();
                anim.Overlay = Microsoft.Xna.Framework.Color.Red;
                
                if(health.Current < 1)
                {
                    player.State = EntityState.Dead;
                }

                entity.SetRange(health, anim, player);
                other.Set(attackSpeed);
            }
        }

        public override void PostSolve(in Contact contact, in ContactImpulse impulse)
        {
            
        }

        public override void PreSolve(in Contact contact, in Manifold oldManifold)
        {
            
        }
    }
}
