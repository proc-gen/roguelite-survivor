using Microsoft.Xna.Framework;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Helpers
{
    public static class PickupHelper
    {
        public static Rectangle GetPickupSourceRectangle(PickupType pickupType)
        {
            int x = 0, y = 0;

            switch (pickupType)
            {
                case PickupType.AttackSpeed:
                    x = 80;
                    y = 64;
                    break;
                case PickupType.Damage:
                    x = 80;
                    y = 336;
                    break;
                case PickupType.MoveSpeed:
                    x = 0;
                    y = 64;
                    break;
                case PickupType.Health:
                    x = 208;
                    y = 224;
                    break;
                case PickupType.SpellEffectChance:
                    x = 80;
                    y = 320;
                    break;
            }

            return new Rectangle(x, y, 16, 16);
        }
    }
}
