using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct Velocity
    {
        public Vector2 Vector { get; set; }

        public System.Numerics.Vector2 VectorPhysics {  get { return new System.Numerics.Vector2(Vector.X, Vector.Y); } }
    }
}
