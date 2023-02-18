using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct AttackSpeed
    {
        public float BaseAttackSpeed { get; private set; }
        public float CurrentAttackSpeed { get; private set; }

        private float baseAttacksPerSecond, currentAttacksPerSecond;
        public float BaseAttacksPerSecond { get { return baseAttacksPerSecond; } set { BaseAttackSpeed = 1f / value; baseAttacksPerSecond = value; } }
        public float CurrentAttacksPerSecond { get { return currentAttacksPerSecond; } set { CurrentAttackSpeed = 1f / value; currentAttacksPerSecond = value; } }

        public float Cooldown { get; set; }
    }
}
