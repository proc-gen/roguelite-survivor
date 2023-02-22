using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.ComponentFactories
{
    public static class SpellFactory
    {
        public static Spell CreateSpell(Spells selectedSpell)
        {
            Spell spell = new Spell() { CurrentSpell = selectedSpell };

            switch(selectedSpell)
            {
                case Spells.Fireball:
                    spell.BaseDamage = spell.CurrentDamage = 8;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 200f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = 2;
                    spell.Cooldown = 0f;
                    break;
                case Spells.IceShard:
                    spell.BaseDamage = spell.CurrentDamage = 5;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 300f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = 3;
                    spell.Cooldown = 0f;
                    break;
                case Spells.LightningBlast:
                    spell.BaseDamage = spell.CurrentDamage = 3;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 400f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = 4;
                    spell.Cooldown = 0f;
                    break;
                case Spells.EnemyMelee:
                    spell.BaseDamage = spell.CurrentDamage = 3;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 0f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = .5f;
                    spell.Cooldown = 0f;
                    break;
            }

            return spell;
        }
    }
}
