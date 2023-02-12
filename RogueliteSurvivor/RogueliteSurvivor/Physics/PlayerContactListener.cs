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
    public class PlayerContactListener : ContactListener
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
                    Enemy e;
                    if (!a.TryGet(out e))
                    {
                        b.TryGet(out e);
                        setEnemyDead(b, e);
                    }
                    else
                    {
                        setEnemyDead(a, e);
                    }
                }
            }
        }

        private void setEnemyDead(Entity entity, Enemy enemy)
        {
            if (enemy.State == EnemyState.Alive)
            {
                enemy.State = EnemyState.Dead;
                entity.Set(enemy);
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
