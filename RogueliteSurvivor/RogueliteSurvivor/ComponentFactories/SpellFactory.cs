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
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 24000f;
                    break;
                case Spells.IceShard:
                    spell.BaseDamage = spell.CurrentDamage = 5;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 48000f;
                    break;
                case Spells.LightningBlast:
                    spell.BaseDamage = spell.CurrentDamage = 3;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 144000f;
                    break;
            }

            return spell;
        }
    }
}
