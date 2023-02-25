using Newtonsoft.Json.Linq;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Containers
{
    public class SpellContainer
    {
        public SpellContainer() { }
        public Spells Spell { get; set; }
        public SpellEffects Effect { get; set; }
        public SpellType Type { get; set; }
        public float BaseEffectChance { get; set; }
        public float CurrentEffectChance { get; set; }
        public float BaseDamage { get; set; }
        public float CurrentDamage { get; set; }
        public float BaseProjectileSpeed { get; set; }
        public float CurrentProjectileSpeed { get; set; }
        public float BaseAttacksPerSecond { get; set; }
        public float CurrentAttacksPerSecond { get; set; }
        public float DamageStartDelay { get; set; }
        public float DamageEndDelay { get; set; }
        public AnimationContainer AliveAnimation { get; set; }
        public SpriteSheetContainer AliveSpriteSheet { get; set; }
        public AnimationContainer HitAnimation { get; set; }
        public SpriteSheetContainer HitSpriteSheet { get; set; }


        public static Spells SpellContainerName(JToken spell)
        {
            return ((string)spell["spell"]).GetSpellFromString();
        }
        public static SpellContainer ToSpellContainer(JToken spell)
        {
            return new SpellContainer()
            {
                Spell = ((string)spell["spell"]).GetSpellFromString(),
                Effect = ((string)spell["effect"]).GetSpellEffectFromString(),
                Type = ((string)spell["type"]).GetSpellTypeFromString(),
                BaseEffectChance = (float)spell["baseEffectChance"],
                CurrentEffectChance = (float)spell["currentEffectChance"],
                BaseDamage = (float)spell["baseDamage"],
                CurrentDamage = (float)spell["currentDamage"],
                BaseProjectileSpeed = (float)spell["baseProjectileSpeed"],
                CurrentProjectileSpeed = (float)spell["currentProjectileSpeed"],
                BaseAttacksPerSecond = (float)spell["baseAttacksPerSecond"],
                CurrentAttacksPerSecond = (float)spell["currentAttacksPerSecond"],
                DamageStartDelay = (float)spell["damageStartDelay"],
                DamageEndDelay = (float)spell["damageEndDelay"],
                AliveAnimation = AnimationContainer.ToAnimationContainer(spell["aliveAnimation"]),
                AliveSpriteSheet = SpriteSheetContainer.ToSpriteSheetContainer(spell["aliveSpriteSheet"]),
                HitAnimation = AnimationContainer.ToAnimationContainer(spell["hitAnimation"]),
                HitSpriteSheet = SpriteSheetContainer.ToSpriteSheetContainer(spell["hitSpriteSheet"])
            };
        }
    }
}
