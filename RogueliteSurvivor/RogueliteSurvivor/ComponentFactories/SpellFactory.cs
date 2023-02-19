using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.ComponentFactories
{
    public static class SpellFactory
    {
        public static Spell CreateSpell(AvailableSpells selectedSpell)
        {
            Spell spell = new Spell() { CurrentSpell = selectedSpell };

            switch(selectedSpell)
            {
                case AvailableSpells.SmallFireball:
                    spell.BaseDamage = spell.CurrentDamage = 8;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 24000f;
                    break;
                case AvailableSpells.IceShard:
                    spell.BaseDamage = spell.CurrentDamage = 5;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 48000f;
                    break;
                case AvailableSpells.LightningBlast:
                    spell.BaseDamage = spell.CurrentDamage = 3;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 144000f;
                    break;
            }

            return spell;
        }
    }
}
