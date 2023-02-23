using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct SingleTarget
    {
        public EntityState State { get; set; }
        public float DamageStartDelay { get; set; }
        public float DamageEndDelay { get; set; }
    }
}
