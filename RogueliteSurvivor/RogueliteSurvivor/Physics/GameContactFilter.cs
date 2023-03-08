﻿using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using RogueliteSurvivor.Components;

namespace RogueliteSurvivor.Physics
{
    public class GameContactFilter : ContactFilter
    {
        public override bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
        {
            bool retVal = false;
            Entity a = (Entity)fixtureA.Body.UserData;
            Entity b = (Entity)fixtureB.Body.UserData;

            if (a.Has<Map>() || b.Has<Map>())
            {
                retVal = true;
            }
            else if (a.Has<Enemy>() && b.Has<Enemy>())
            {
                retVal = true;
            }
            else if ((a.Has<Projectile>() && b.Has<Enemy>()) || (b.Has<Projectile>() && a.Has<Enemy>()))
            {
                retVal = true;
            }
            else if ((a.Has<SingleTarget>() && fixtureA.Body.IsAwake() && b.Has<Enemy>()) || (b.Has<SingleTarget>() && fixtureB.Body.IsAwake() && a.Has<Enemy>()))
            {
                retVal = true;
            }
            else if ((a.Has<Player>() && b.Has<Enemy>()) || (b.Has<Player>() && a.Has<Enemy>()))
            {
                retVal = true;
            }

            return retVal;
        }
    }
}
