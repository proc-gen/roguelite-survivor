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
        public SpellEffects Effect { get; set; }
        public float BaseEffectChance { get; set; }
        public float CurrentEffectChance { get; set; }
        public float BaseDamage { get; set; }
        public float CurrentDamage { get; set; }
        public float BaseProjectileSpeed { get; set; }
        public float CurrentProjectileSpeed { get; set; }
        public float BaseAttackSpeed { get; private set; }
        public float CurrentAttackSpeed { get; private set; }

        private float baseAttacksPerSecond, currentAttacksPerSecond;
        public float BaseAttacksPerSecond { get { return baseAttacksPerSecond; } set { BaseAttackSpeed = 1f / value; baseAttacksPerSecond = value; } }
        public float CurrentAttacksPerSecond { get { return currentAttacksPerSecond; } set { CurrentAttackSpeed = 1f / value; currentAttacksPerSecond = value; } }

        public float Cooldown { get; set; }
    }
}
