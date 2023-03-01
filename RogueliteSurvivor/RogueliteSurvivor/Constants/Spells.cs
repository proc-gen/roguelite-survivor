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
                case "Fireball":
                    return Spells.Fireball;
                case "FireExplosion":
                    return Spells.FireExplosion;
                case "IceShard":
                    return Spells.IceShard;
                case "IceSpikes":
                    return Spells.IceSpikes;
                case "LightningBlast":
                    return Spells.LightningBlast;
                case "LightningStrike":
                    return Spells.LightningStrike;
                case "EnemyMelee":
                    return Spells.EnemyMelee;
                default:
                    return Spells.None;
            }
        }

        public static string GetReadableSpellName(this Spells spell)
        {
            switch (spell)
            {
                case Spells.Fireball:
                    return "Fireball";
                case Spells.FireExplosion:
                    return "Fire Explosion";
                case Spells.IceShard:
                    return "Ice Shard";
                case Spells.IceSpikes:
                    return "Ice Spikes";
                case Spells.LightningBlast:
                    return "Lightning Blast";
                case Spells.LightningStrike:
                    return "Lightning Strike";
                default:
                    return "None";
            }
        }
    }
}
