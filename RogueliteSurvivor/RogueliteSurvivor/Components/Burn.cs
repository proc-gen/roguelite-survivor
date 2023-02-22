using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct Burn
    {
        public float TimeLeft { get; set; }
        public float TickRate { get; set; }
        public float NextTick { get; set; }
    }
}
