using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct Spell
    {
        public Spells CurrentSpell { get; set; } 
        public int BaseDamage { get; set; }
        public int CurrentDamage { get; set; }
        public float BaseProjectileSpeed { get; set; }
        public float CurrentProjectileSpeed { get; set; }
    }
}
