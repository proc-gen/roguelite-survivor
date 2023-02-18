using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public enum AvailableSpells
    {
        SmallFireball,
        MediumFireball,
        LargeFireball,
    }

    public struct Spell
    {
        public AvailableSpells CurrentSpell { get; set; } 
        public int BaseDamage { get; set; }
        public int CurrentDamage { get; set; }
    }
}
