using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public static T CreateSpell<T>(Spells selectedSpell) 
            where T : ISpell, new()
        {
            T spell = new T() { CurrentSpell = selectedSpell };

            switch (selectedSpell)
            {
                case Spells.Fireball:
                    spell.BaseDamage = spell.CurrentDamage = 8;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 200f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = 2;
                    spell.BaseEffectChance = spell.CurrentEffectChance = .1f;
                    spell.Cooldown = 0f;
                    spell.Effect = SpellEffects.Burn;
                    spell.Type = SpellType.Projectile;
                    break;
                case Spells.FireExplosion:
                    spell.BaseDamage = spell.CurrentDamage = 10;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 0f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = .5f;
                    spell.BaseEffectChance = spell.CurrentEffectChance = .3f;
                    spell.Cooldown = 0f;
                    spell.Effect = SpellEffects.Burn;
                    spell.Type = SpellType.SingleTarget;
                    break;
                case Spells.IceShard:
                    spell.BaseDamage = spell.CurrentDamage = 5;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 300f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = 3;
                    spell.BaseEffectChance = spell.CurrentEffectChance = .1f;
                    spell.Cooldown = 0f;
                    spell.Effect = SpellEffects.Slow;
                    spell.Type = SpellType.Projectile;
                    break;
                case Spells.IceSpikes:
                    spell.BaseDamage = spell.CurrentDamage = 10;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 0f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = .5f;
                    spell.BaseEffectChance = spell.CurrentEffectChance = .3f;
                    spell.Cooldown = 0f;
                    spell.Effect = SpellEffects.Slow;
                    spell.Type = SpellType.SingleTarget;
                    break;
                case Spells.LightningBlast:
                    spell.BaseDamage = spell.CurrentDamage = 3;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 400f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = 4;
                    spell.BaseEffectChance = spell.CurrentEffectChance = .1f;
                    spell.Cooldown = 0f;
                    spell.Effect = SpellEffects.Shock;
                    spell.Type = SpellType.Projectile;
                    break;
                case Spells.LightningStrike:
                    spell.BaseDamage = spell.CurrentDamage = 10;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 0f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = .5f;
                    spell.BaseEffectChance = spell.CurrentEffectChance = .3f;
                    spell.Cooldown = 0f;
                    spell.Effect = SpellEffects.Shock;
                    spell.Type = SpellType.SingleTarget;
                    break;
                case Spells.EnemyMelee:
                    spell.BaseDamage = spell.CurrentDamage = 3;
                    spell.BaseProjectileSpeed = spell.CurrentProjectileSpeed = 0f;
                    spell.BaseAttacksPerSecond = spell.CurrentAttacksPerSecond = .5f;
                    spell.BaseEffectChance = spell.CurrentEffectChance = 0f;
                    spell.Cooldown = 0f;
                    spell.Effect = SpellEffects.None;
                    spell.Type = SpellType.None;
                    break;
            }

            return spell;
        }

        public static Animation GetSpellAnimation(Spells currentSpell)
        {
            Animation? animation = null;
            switch (currentSpell)
            {
                case Spells.Fireball:
                    animation = new Animation(0, 3, .1f, 1);
                    break;
                case Spells.FireExplosion:
                    animation = new Animation(0, 11, .1f, 1, false);
                    break;
                case Spells.IceShard:
                    animation = new Animation(0, 9, .1f, 1);
                    break;
                case Spells.IceSpikes:
                    animation = new Animation(0, 25, .05f, 1, false);
                    break;
                case Spells.LightningBlast:
                    animation = new Animation(0, 4, .1f, 1);
                    break;
                case Spells.LightningStrike:
                    animation = new Animation(0, 12, .05f, 1, false);
                    break;
            }
            return animation.Value;
        }

        public static SpriteSheet GetSpellSpriteSheet(Dictionary<string, Texture2D> textures, Spells currentSpell, Vector2 currentPosition, Vector2 targetPosition)
        {
            SpriteSheet? spriteSheet = null;
            switch (currentSpell)
            {
                case Spells.Fireball:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 4, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
                case Spells.FireExplosion:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 12, 1, 0f, 1f);
                    break;
                case Spells.IceShard:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 10, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
                case Spells.IceSpikes:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 26, 1, 0f, 1f);
                    break;
                case Spells.LightningBlast:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 5, 1, MathF.Atan2(targetPosition.Y - currentPosition.Y, targetPosition.X - currentPosition.X), .5f);
                    break;
                case Spells.LightningStrike:
                    spriteSheet = new SpriteSheet(textures[currentSpell.ToString()], currentSpell.ToString(), 13, 1, 0f, 1f);
                    break;
            }
            return spriteSheet.Value;
        }

        public static SingleTarget CreateSingleTargetSpell(Spells selectedSpell)
        {
            SingleTarget target = new SingleTarget() { State = EntityState.Alive };
            switch (selectedSpell)
            {
                case Spells.FireExplosion:
                    target.DamageStartDelay = .2f;
                    target.DamageEndDelay = .7f;
                    break;
                case Spells.IceSpikes:
                    target.DamageStartDelay = .1f;
                    target.DamageEndDelay = .5f;
                    break;
                case Spells.LightningStrike:
                    target.DamageStartDelay = .25f;
                    target.DamageEndDelay = .4f;
                    break;
            }
            return target;
        }
    }
}
