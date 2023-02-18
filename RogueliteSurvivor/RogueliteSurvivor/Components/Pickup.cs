using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct Pickup
    {
        public PickupType Type { get; set; }
        public float PickupAmount { get; set; }
    }
}
