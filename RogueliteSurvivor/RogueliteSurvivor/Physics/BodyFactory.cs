﻿using Arch.Core;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Physics
{
    public static class BodyFactory
    {
        public static Body CreateCircularBody(Entity entity, int width, Box2D.NetStandard.Dynamics.World.World physicsWorld, BodyDef bodyDef, float density = 1f)
        {
            var bodyShape = new FixtureDef();
            bodyShape.shape = new CircleShape() { Radius = width / 2f / PhysicsConstants.PhysicsToPixelsRatio };
            bodyShape.density = density;
            bodyShape.friction = 0.0f;
            bodyShape.isSensor = density < 1f;
            bodyDef.type = BodyType.Dynamic;

            var physicsBody = physicsWorld.CreateBody(bodyDef);
            physicsBody.CreateFixture(bodyShape);
            physicsBody.SetUserData(entity);

            return physicsBody;
        }
    }
}
