using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.ComponentFactories
{
    public static class SpellFactory
    {
        public static T CreateSpell<T>(SpellContainer spellContainer)
            where T : ISpell, new()
        {
            T spell = new T() { 
                Spell = spellContainer.Spell,
                BaseDamage = spellContainer.BaseDamage,
                CurrentDamage = spellContainer.CurrentDamage,
                BaseProjectileSpeed = spellContainer.BaseProjectileSpeed,
                CurrentProjectileSpeed = spellContainer.CurrentProjectileSpeed,
                BaseAttacksPerSecond = spellContainer.BaseAttacksPerSecond,
                CurrentAttacksPerSecond = spellContainer.CurrentAttacksPerSecond,
                BaseEffectChance = spellContainer.BaseEffectChance,
                CurrentEffectChance = spellContainer.CurrentEffectChance,
                Cooldown = 0f,
                Effect = spellContainer.Effect,
                Type = spellContainer.Type,
            };

            return spell;
        }

        public static Animation GetSpellAliveAnimation(SpellContainer spellContainer)
        {
            return new Animation(
                spellContainer.AliveAnimation.FirstFrame, 
                spellContainer.AliveAnimation.LastFrame,
                spellContainer.AliveAnimation.PlaybackSpeed,
                spellContainer.AliveAnimation.NumDirections,
                spellContainer.AliveAnimation.Repeatable);
        }

        public static SpriteSheet GetSpellAliveSpriteSheet(Dictionary<string, Texture2D> textures, SpellContainer spellContainer, Vector2 currentPosition, Vector2 targetPosition)
        {
            return new SpriteSheet(
                textures[spellContainer.Spell.ToString()],
                spellContainer.Spell.ToString(),
                spellContainer.AliveSpriteSheet.FramesPerRow,
                spellContainer.AliveSpriteSheet.FramesPerColumn,
                spellContainer.AliveSpriteSheet.Rotation == "none" ? 0 : MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X),
                spellContainer.AliveSpriteSheet.Scale
            );
        }

        public static SingleTarget CreateSingleTarget(SpellContainer spellContainer)
        {
            return new SingleTarget()
            {
                State = EntityState.Alive,
                DamageStartDelay = spellContainer.DamageStartDelay,
                DamageEndDelay = spellContainer.DamageEndDelay,
            };
        }
    }
}
