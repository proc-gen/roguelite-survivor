using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct PickupSprite
    {
        public PickupType Type { get; set; }
        public float PickupAmount { get; set; }
        public float Max { get; private set; }
        public float Min { get; private set; }
        public float Current { get; set; }

        public PickupSprite()
        {
            Max = 5f;
            Min = -5f;
            Current = 0;
        }
    }
}
