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
            checkContact(contact);
        }

        private void checkContact(Contact contact)
        {
            if (contact.GetFixtureA().Body.UserData != null && contact.GetFixtureB().Body.UserData != null)
            {
                Entity a = (Entity)contact.GetFixtureA().Body.UserData;
                Entity b = (Entity)contact.GetFixtureB().Body.UserData;

                if ((a.Has<Player>() && b.Has<Enemy>()) || (b.Has<Player>() && a.Has<Enemy>()))
                {
                    setEnemyDead(a, b);
                }
                else if ((a.Has<Projectile>() && b.Has<Enemy>()) || (b.Has<Projectile>() && a.Has<Enemy>()))
                {
                    Projectile p;
                    ProjectileState state;
                    if (!a.TryGet(out p))
                    {
                        b.TryGet(out p);
                        state = p.State;
                        setProjectileDead(b, p);
                    }
                    else
                    {
                        state = p.State;
                        setProjectileDead(a, p);
                    }

                    if (state == ProjectileState.Alive)
                    {
                        setEnemyDead(a, b);
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

        private void setEnemyDead(Entity a, Entity b)
        {
            Enemy e;
            if (!a.TryGet(out e))
            {
                b.TryGet(out e);
                setEnemyStateDead(b, e);
            }
            else
            {
                setEnemyStateDead(a, e);
            }
        }

        private void setEnemyStateDead(Entity entity, Enemy enemy)
        {
            if (enemy.State == EnemyState.Alive)
            {
                enemy.State = EnemyState.Dead;
                entity.Set(enemy);
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

        public override void PostSolve(in Contact contact, in ContactImpulse impulse)
        {
            
        }

        public override void PreSolve(in Contact contact, in Manifold oldManifold)
        {
            
        }
    }
}
