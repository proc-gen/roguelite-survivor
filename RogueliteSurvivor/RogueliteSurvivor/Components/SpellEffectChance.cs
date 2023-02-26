using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct SpellEffectChance
    {
        public SpellEffectChance(float baseSpellEffectChance) 
        {
            BaseSpellEffectChance = baseSpellEffectChance;
            CurrentSpellEffectChance = baseSpellEffectChance;
        }
        public float BaseSpellEffectChance { get; set; }
        public float CurrentSpellEffectChance { get;set; }
    }
}
