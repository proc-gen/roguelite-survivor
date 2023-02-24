using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Constants
{
    public enum Spells
    {
        None,
        Fireball,
        FireExplosion,
        IceShard,
        IceSpikes,
        LightningBlast,
        LightningStrike,
        EnemyMelee,
    }

    public static class SpellsExtensions
    {
        public static Spells GetSpellFromString(this string spellString)
        {
            switch(spellString)
            {
                case "None":
                    return Spells.None;
                    break;
                case "Fireball":
                    return Spells.Fireball;
                    break;
                case "FireExplosion":
                    return Spells.FireExplosion;
                    break;
                case "IceShard":
                    return Spells.IceShard;
                    break;
                case "IceSpikes":
                    return Spells.IceSpikes;
                    break;
                case "LightningBlast":
                    return Spells.LightningBlast;
                    break;
                case "LightningStrike":
                    return Spells.LightningStrike;
                    break;
                case "EnemyMelee":
                    return Spells.EnemyMelee;
                    break;
                default:
                    return Spells.None;
            }
        }
    }
}
