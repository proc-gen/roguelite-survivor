using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Bodies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct Collider
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Offset { get; set; }
        public Body PhysicsBody { get; set; }

        public Collider(int width, int height, Box2D.NetStandard.Dynamics.World.World physicsWorld, BodyDef bodyDef, float density = 1)
        {
            Width = width;
            Height = height;
            Offset = new Vector2(width / 2, height / 2);

            var bodyShape = new Box2D.NetStandard.Dynamics.Fixtures.FixtureDef();
            bodyShape.shape = new PolygonShape(width / 2, height / 2);
            bodyShape.density = density;
            bodyShape.friction = 0.0f;
            bodyDef.type = BodyType.Dynamic;

            PhysicsBody = physicsWorld.CreateBody(bodyDef);
            PhysicsBody.CreateFixture(bodyShape);
        }
    }
}
