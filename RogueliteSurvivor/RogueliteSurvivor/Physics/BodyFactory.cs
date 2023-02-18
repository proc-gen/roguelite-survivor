using Arch.Core;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Physics
{
    public static class BodyFactory
    {
        public static Body CreateCircularBody(Entity entity, int width, Box2D.NetStandard.Dynamics.World.World physicsWorld, BodyDef bodyDef, float density = 1)
        {
            var bodyShape = new Box2D.NetStandard.Dynamics.Fixtures.FixtureDef();
            bodyShape.shape = new CircleShape() { Radius = width / 2 };
            bodyShape.density = density;
            bodyShape.friction = 0.0f;
            bodyDef.type = BodyType.Dynamic;

            var physicsBody = physicsWorld.CreateBody(bodyDef);
            physicsBody.CreateFixture(bodyShape);
            physicsBody.SetUserData(entity);

            return physicsBody;
        }
    }
}
