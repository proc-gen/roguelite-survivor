using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct AttackSpeed
    {
        public float BaseAttackSpeed { get; set; }
        public float CurrentAttackSpeed { get; set; }
        public float Cooldown { get; set; }
    }
}
