using Microsoft.Xna.Framework;

namespace RogueliteSurvivor.Components
{
    public struct Velocity
    {
        public Vector2 Vector { get; set; }

        public System.Numerics.Vector2 VectorPhysics { get { return new System.Numerics.Vector2(Vector.X, Vector.Y); } }
    }
}
