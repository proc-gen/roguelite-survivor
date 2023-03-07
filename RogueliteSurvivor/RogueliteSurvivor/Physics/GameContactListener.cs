using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;

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

                
                if ((a.Has<Player>() && b.Has<Enemy>()) || (b.Has<Player>() && a.Has<Enemy>()))
                {
                    damagePlayer(a, b);
                }
                else if ((a.Has<Projectile>() && b.Has<Enemy>()) || (b.Has<Projectile>() && a.Has<Enemy>()))
                {
                    EntityStatus state;
                    Damage damage;
                    Owner owner;
                    if (a.Has<Projectile>())
                    {
                        state = b.Get<EntityStatus>();
                        damage = a.Get<Damage>();
                        owner = a.Get<Owner>();
                        updateProjectile(a, state);
                    }
                    else
                    {
                        state = b.Get<EntityStatus>();
                        damage = b.Get<Damage>();
                        owner = b.Get<Owner>();
                        updateProjectile(b, state);
                    }

                    if (state.State == State.Alive)
                    {
                        damageEnemy(a, b, damage, owner);
                    }
                }
                else if ((a.Has<SingleTarget>() && b.Has<Enemy>()) || (b.Has<SingleTarget>() && a.Has<Enemy>()))
                {
                    Damage damage;
                    Owner owner;
                    if (a.Has<SingleTarget>())
                    {
                        damage = a.Get<Damage>();
                        owner = a.Get<Owner>();
                    }
                    else
                    {
                        damage = b.Get<Damage>();
                        owner = b.Get<Owner>();
                    }

                    damageEnemy(a, b, damage, owner);
                }
                else if ((a.Has<Projectile>() && b.Has<Map>()) || (b.Has<Projectile>() && a.Has<Map>()))
                {
                    if (a.Has<Projectile>())
                    {
                        setEntityDead(a, a.Get<EntityStatus>());
                    }
                    else
                    {
                        setEntityDead(b, b.Get<EntityStatus>());
                    }
                }
                
            }
        }

        private void damageEnemy(Entity a, Entity b, Damage damage, Owner owner)
        {
            if (a.Has<Enemy>())
            {
                setEnemyHealthAndState(a, a.Get<EntityStatus>(), damage, owner);
            }
            else
            {
                setEnemyHealthAndState(b, b.Get<EntityStatus>(), damage, owner);
            }
        }

        private void setEnemyHealthAndState(Entity entity, EntityStatus entityStatus, Damage damage, Owner owner)
        {
            if (entityStatus.State == State.Alive)
            {
                Health health = entity.Get<Health>();
                health.Current -= (int)damage.Amount;
                if (health.Current < 1)
                {
                    entityStatus.State = State.ReadyToDie;
                    entity.Set(entityStatus);
                    Experience enemyExperience = entity.Get<Experience>();
                    KillCount killCount = owner.Entity.Get<KillCount>();
                    Player playerExperience = owner.Entity.Get<Player>();
                    killCount.Count++;
                    playerExperience.TotalExperience += enemyExperience.Amount;
                    playerExperience.ExperienceToNextLevel -= enemyExperience.Amount;
                    owner.Entity.Set(killCount, playerExperience);
                }
                else
                {
                    Animation anim = entity.Get<Animation>();
                    anim.Overlay = Microsoft.Xna.Framework.Color.Red;
                    
                    entity.Set(health, anim);
                    if (damage.SpellEffect != SpellEffects.None)
                    {
                        switch (damage.SpellEffect)
                        {
                            case SpellEffects.Burn:
                                if (!entity.Has<Burn>())
                                {
                                    entity.Add(new Burn() { TimeLeft = 5f, TickRate = .5f, NextTick = .5f });
                                }
                                else
                                {
                                    entity.Set(new Burn() { TimeLeft = 5f, TickRate = .5f, NextTick = .5f });
                                }
                                break;
                            case SpellEffects.Slow:
                                if (!entity.Has<Slow>())
                                {
                                    entity.Add(new Slow() { TimeLeft = 5f });
                                }
                                else
                                {
                                    entity.Set(new Slow() { TimeLeft = 5f });
                                }
                                break;
                            case SpellEffects.Shock:
                                if (!entity.Has<Shock>())
                                {
                                    entity.Add(new Shock() { TimeLeft = 1f });
                                }
                                else
                                {
                                    entity.Set(new Shock() { TimeLeft = 1f });
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void updateProjectile(Entity entity, EntityStatus entityStatus)
        {
            var pierce = entity.Get<Pierce>();
            if(pierce.Num > 0)
            {
                pierce.Num--;
                entity.Set(pierce);
            }
            else
            {
                setEntityDead(entity, entityStatus);
            }
        }

        private void setEntityDead(Entity entity, EntityStatus entityStatus)
        {
            if (entityStatus.State == State.Alive)
            {
                entityStatus.State = State.ReadyToDie;
                entity.Set(entityStatus);
            }
        }

        private void damagePlayer(Entity a, Entity b)
        {
            if (a.Has<Player>())
            {
                setPlayerHealthAndState(a, b, a.Get<EntityStatus>());
            }
            else
            {
                setPlayerHealthAndState(b, a, b.Get<EntityStatus>());
            }
        }

        private void setPlayerHealthAndState(Entity entity, Entity other, EntityStatus entityStatus)
        {
            var attackSpeed = other.Get<Spell1>();
            if (attackSpeed.Cooldown > attackSpeed.CurrentAttackSpeed)
            {
                attackSpeed.Cooldown -= attackSpeed.CurrentAttackSpeed;
                Health health = entity.Get<Health>();
                Damage damage = other.Get<Damage>();
                health.Current -= (int)damage.Amount;
                Animation anim = entity.Get<Animation>();
                anim.Overlay = Microsoft.Xna.Framework.Color.Red;

                if (health.Current < 1)
                {
                    entityStatus.State = State.Dead;
                }

                entity.Set(health, anim, entityStatus);
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
