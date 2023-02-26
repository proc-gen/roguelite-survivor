using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct AttackSpeed
    {
        public AttackSpeed(float baseAttackSpeed) 
        { 
            BaseAttackSpeed = baseAttackSpeed;
            CurrentAttackSpeed = baseAttackSpeed;
        }

        public float BaseAttackSpeed { get; set; }
        public float CurrentAttackSpeed { get; set; }
    }
}
