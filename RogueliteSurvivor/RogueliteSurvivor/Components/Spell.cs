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
        public SpellType Type { get; set; }
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

        public Spell1 ToSpell1()
        {
            return new Spell1()
            {
                CurrentSpell = CurrentSpell,
                Effect = Effect,
                Type = Type,
                BaseEffectChance = BaseEffectChance,
                CurrentEffectChance = CurrentEffectChance,
                BaseDamage = BaseDamage,
                CurrentDamage = CurrentDamage,
                BaseProjectileSpeed = BaseProjectileSpeed,
                CurrentProjectileSpeed = CurrentProjectileSpeed,
                BaseAttacksPerSecond = BaseAttacksPerSecond,
                CurrentAttacksPerSecond = CurrentAttacksPerSecond,
                Cooldown = Cooldown,
            };
        }

        public Spell2 ToSpell2()
        {
            return new Spell2()
            {
                CurrentSpell = CurrentSpell,
                Effect = Effect,
                Type = Type,
                BaseEffectChance = BaseEffectChance,
                CurrentEffectChance = CurrentEffectChance,
                BaseDamage = BaseDamage,
                CurrentDamage = CurrentDamage,
                BaseProjectileSpeed = BaseProjectileSpeed,
                CurrentProjectileSpeed = CurrentProjectileSpeed,
                BaseAttacksPerSecond = BaseAttacksPerSecond,
                CurrentAttacksPerSecond = CurrentAttacksPerSecond,
                Cooldown = Cooldown,
            };
        }
    }

    public struct Spell1
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
        public float BaseAttackSpeed { get; private set; }
        public float CurrentAttackSpeed { get; private set; }

        private float baseAttacksPerSecond, currentAttacksPerSecond;
        public float BaseAttacksPerSecond { get { return baseAttacksPerSecond; } set { BaseAttackSpeed = 1f / value; baseAttacksPerSecond = value; } }
        public float CurrentAttacksPerSecond { get { return currentAttacksPerSecond; } set { CurrentAttackSpeed = 1f / value; currentAttacksPerSecond = value; } }

        public float Cooldown { get; set; }

        public Spell ToSpell()
        {
            return new Spell()
            {
                CurrentSpell = CurrentSpell,
                Effect = Effect,
                Type = Type,
                BaseEffectChance = BaseEffectChance,
                CurrentEffectChance = CurrentEffectChance,
                BaseDamage = BaseDamage,
                CurrentDamage = CurrentDamage,
                BaseProjectileSpeed = BaseProjectileSpeed,
                CurrentProjectileSpeed = CurrentProjectileSpeed,
                BaseAttacksPerSecond = BaseAttacksPerSecond,
                CurrentAttacksPerSecond = CurrentAttacksPerSecond,
                Cooldown = Cooldown,
            };
        }
    }

    public struct Spell2
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
        public float BaseAttackSpeed { get; private set; }
        public float CurrentAttackSpeed { get; private set; }

        private float baseAttacksPerSecond, currentAttacksPerSecond;
        public float BaseAttacksPerSecond { get { return baseAttacksPerSecond; } set { BaseAttackSpeed = 1f / value; baseAttacksPerSecond = value; } }
        public float CurrentAttacksPerSecond { get { return currentAttacksPerSecond; } set { CurrentAttackSpeed = 1f / value; currentAttacksPerSecond = value; } }

        public float Cooldown { get; set; }
        
        public Spell ToSpell()
        {
            return new Spell()
            {
                CurrentSpell = CurrentSpell,
                Effect = Effect,
                Type = Type,
                BaseEffectChance = BaseEffectChance,
                CurrentEffectChance = CurrentEffectChance,
                BaseDamage = BaseDamage,
                CurrentDamage = CurrentDamage,
                BaseProjectileSpeed = BaseProjectileSpeed,
                CurrentProjectileSpeed = CurrentProjectileSpeed,
                BaseAttacksPerSecond = BaseAttacksPerSecond,
                CurrentAttacksPerSecond = CurrentAttacksPerSecond,
                Cooldown = Cooldown,
            };
        }
    }
}
