using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using RogueliteSurvivor.Components;
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

                if ((a.Has<Player>() && b.Has<Enemy>()) || (b.Has<Player>() && a.Has<Enemy>()))
                {
                    damagePlayer(a, b);
                }
                else if ((a.Has<Projectile>() && b.Has<Enemy>()) || (b.Has<Projectile>() && a.Has<Enemy>()))
                {
                    Projectile p;
                    ProjectileState state;
                    Damage damage;
                    if (!a.TryGet(out p))
                    {
                        b.TryGet(out p);
                        state = p.State;
                        damage = b.Get<Damage>();
                        setProjectileDead(b, p);
                    }
                    else
                    {
                        state = p.State;
                        damage = a.Get<Damage>();
                        setProjectileDead(a, p);
                    }

                    if (state == ProjectileState.Alive)
                    {
                        damageEnemy(a, b, damage);
                    }
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

        private void damageEnemy(Entity a, Entity b, Damage damage)
        {
            Enemy e;
            if (!a.TryGet(out e))
            {
                b.TryGet(out e);
                setEnemyHealthAndState(b, e, damage);
            }
            else
            {
                setEnemyHealthAndState(a, e, damage);
            }
        }

        private void setEnemyHealthAndState(Entity entity, Enemy enemy, Damage damage)
        {
            if (enemy.State == EnemyState.Alive)
            {
                Health health = entity.Get<Health>();
                health.Current -= damage.Amount;
                if (health.Current < 1)
                {
                    enemy.State = EnemyState.Dead;
                    entity.Set(enemy);
                }
                else
                {
                    Animation anim = entity.Get<Animation>();
                    anim.Overlay = Microsoft.Xna.Framework.Color.Red;
                    entity.SetRange(health, anim);
                }
            }
        }

        private void setProjectileDead(Entity entity, Projectile projectile)
        {
            if (projectile.State == ProjectileState.Alive)
            {
                projectile.State = ProjectileState.Dead;
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
            Health health = entity.Get<Health>();
            Damage damage = other.Get<Damage>();
            health.Current -= damage.Amount;
            Animation anim = entity.Get<Animation>();
            anim.Overlay = Microsoft.Xna.Framework.Color.Red;
            entity.SetRange(health, anim);
        }

        public override void PostSolve(in Contact contact, in ContactImpulse impulse)
        {
            
        }

        public override void PreSolve(in Contact contact, in Manifold oldManifold)
        {
            
        }
    }
}
