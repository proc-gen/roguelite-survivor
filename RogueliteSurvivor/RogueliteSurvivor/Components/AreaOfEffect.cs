using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct AreaOfEffect
    {
        public AreaOfEffect(float radius) { Radius = radius; }
        public float Radius { get; set; }
    }
}
