using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public interface ISpell
    {
        public Spells CurrentSpell { get; set; }
        public SpellEffects Effect { get; set; }
        public SpellType Type { get; set; }
        public float BaseEffectChance { get; set; }
        public float CurrentEffectChance { get; set; }
        public float BaseDamage { get; set; }
        public float CurrentDamage { get; set; }
        public float BaseProjectileSpeed { get; set; }
        public float CurrentProjectileSpeed { get; set; }
        public float BaseAttackSpeed { get; protected set; }
        public float CurrentAttackSpeed { get; protected set; }

        protected float baseAttacksPerSecond { get; set; }
        protected float currentAttacksPerSecond { get; set; }
        public float BaseAttacksPerSecond { get { return baseAttacksPerSecond; } set { BaseAttackSpeed = 1f / value; baseAttacksPerSecond = value; } }
        public float CurrentAttacksPerSecond { get { return currentAttacksPerSecond; } set { CurrentAttackSpeed = 1f / value; currentAttacksPerSecond = value; } }

        public float Cooldown { get; set; }
    }

    public struct Spell1 : ISpell
    {
        public Spells CurrentSpell { get; set; }
        public SpellEffects Effect { get; set; }
        public SpellType Type { get; set; }
        public float BaseEffectChance { get; set; }
        public float CurrentEffectChance { get; set; }
        public float BaseDamage { get; set; }
        public float CurrentDamage { get; set; }
        public float BaseProjectileSpeed { get; set; }
        public float CurrentProjectileSpeed { get; set; }
        public float BaseAttackSpeed { get; set; }
        public float CurrentAttackSpeed { get; set; }

        public float baseAttacksPerSecond { get; set; }
        public float currentAttacksPerSecond { get; set; }
        public float BaseAttacksPerSecond { get { return baseAttacksPerSecond; } set { BaseAttackSpeed = 1f / value; baseAttacksPerSecond = value; } }
        public float CurrentAttacksPerSecond { get { return currentAttacksPerSecond; } set { CurrentAttackSpeed = 1f / value; currentAttacksPerSecond = value; } }

        public float Cooldown { get; set; }
    }

    public struct Spell2 : ISpell
    {
        public Spells CurrentSpell { get; set; }
        public SpellEffects Effect { get; set; }
        public SpellType Type { get; set; }
        public float BaseEffectChance { get; set; }
        public float CurrentEffectChance { get; set; }
        public float BaseDamage { get; set; }
        public float CurrentDamage { get; set; }
        public float BaseProjectileSpeed { get; set; }
        public float CurrentProjectileSpeed { get; set; }
        public float BaseAttackSpeed { get; set; }
        public float CurrentAttackSpeed { get; set; }

        public float baseAttacksPerSecond { get; set; }
        public float currentAttacksPerSecond { get; set; }
        public float BaseAttacksPerSecond { get { return baseAttacksPerSecond; } set { BaseAttackSpeed = 1f / value; baseAttacksPerSecond = value; } }
        public float CurrentAttacksPerSecond { get { return currentAttacksPerSecond; } set { CurrentAttackSpeed = 1f / value; currentAttacksPerSecond = value; } }

        public float Cooldown { get; set; }
    }
}
