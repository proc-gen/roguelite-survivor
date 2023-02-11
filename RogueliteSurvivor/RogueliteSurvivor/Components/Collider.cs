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
    }
}
