using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Dynamics.World;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Helpers
{
    public static class PickupHelper
    {
        public static Rectangle GetPickupSourceRectangle(PickupType pickupType)
        {
            int x = 0, y = 0;

            switch (pickupType)
            {
                case PickupType.AttackSpeed:
                    x = 80;
                    y = 64;
                    break;
                case PickupType.Damage:
                    x = 80;
                    y = 336;
                    break;
                case PickupType.MoveSpeed:
                    x = 0;
                    y = 64;
                    break;
                case PickupType.Health:
                    x = 208;
                    y = 224;
                    break;
                case PickupType.SpellEffectChance:
                    x = 80;
                    y = 320;
                    break;
                case PickupType.Pierce:
                    x = 96;
                    y = 0;
                    break;
                case PickupType.AreaOfEffect:
                    x = 144;
                    y = 48;
                    break;
            }

            return new Rectangle(x, y, 16, 16);
        }

        public static float GetPickupAmount(PickupType pickupType)
        {
            float pickup = 0f;

            switch (pickupType)
            {
                case PickupType.AttackSpeed:
                    pickup = .1f;
                    break;
                case PickupType.Damage:
                    pickup = .25f;
                    break;
                case PickupType.MoveSpeed:
                    pickup = 5f;
                    break;
                case PickupType.Health:
                    pickup = 5f;
                    break;
                case PickupType.SpellEffectChance:
                    pickup = .25f;
                    break;
                case PickupType.Pierce:
                    pickup = 1f;
                    break;
                case PickupType.AreaOfEffect:
                    pickup = .25f;
                    break;
            }

            return pickup;
        }

        public static string GetPickupDisplayTextForLevelUpChoice(PickupType pickupType)
        {
            string retVal = string.Empty;
            switch (pickupType)
            {
                case PickupType.AttackSpeed:
                    retVal = string.Concat("Attack Speed: +", GetPickupAmount(pickupType).ToString("F"), "x");
                    break;
                case PickupType.Damage:
                    retVal = string.Concat("Spell Damage: +", GetPickupAmount(pickupType).ToString("F"), "x");
                    break;
                case PickupType.SpellEffectChance:
                    retVal = string.Concat("Spell Effect Chance: +", GetPickupAmount(pickupType).ToString("F"), "x");
                    break;
                case PickupType.MoveSpeed:
                    retVal = string.Concat("Move Speed: +", ((int)GetPickupAmount(pickupType)).ToString());
                    break;
                case PickupType.Pierce:
                    retVal = string.Concat("Pierce: +", ((int)GetPickupAmount(pickupType)).ToString());
                    break;
                case PickupType.AreaOfEffect:
                    retVal = string.Concat("Area of Effect: +", GetPickupAmount(pickupType).ToString("F"), "x");
                    break;
                case PickupType.Health:
                    //Not Used
                    break;
            }

            return retVal;
        }

        public static void ProcessPickup(ref Entity player, PickupType pickupType)
        {
            float pickupAmount = GetPickupAmount(pickupType);
            switch(pickupType)
            {
                case PickupType.AttackSpeed:
                    processAttackSpeed(player, pickupAmount);
                    break;
                case PickupType.Damage:
                    processDamage(player, pickupAmount);
                    break;
                case PickupType.SpellEffectChance:
                    processSpellEffectChance(player, pickupAmount);
                    break;
                case PickupType.MoveSpeed:
                    var moveSpeed = player.Get<Speed>();
                    moveSpeed.speed += pickupAmount;
                    player.Set(moveSpeed);
                    break;
                case PickupType.Pierce:
                    var pierce = player.Get<Pierce>();
                    pierce.Num += (int)pickupAmount;
                    player.Set(pierce);
                    break;
                case PickupType.AreaOfEffect:
                    var areaOfAffect = player.Get<AreaOfEffect>();
                    areaOfAffect.Radius += pickupAmount;
                    player.Set(areaOfAffect);
                    break;
                case PickupType.Health:
                    var health = player.Get<Health>();
                    if (health.Current < health.Max)
                    {
                        health.Current = int.Min(health.Max, (int)pickupAmount + health.Current);
                        player.Set(health);
                    }
                    break;
            }
        }

        private static void processAttackSpeed(Entity player, float pickupAmount)
        {
            AttackSpeed attackSpeed = player.Get<AttackSpeed>();
            attackSpeed.CurrentAttackSpeed += attackSpeed.BaseAttackSpeed * pickupAmount;

            if (player.TryGet(out Spell1 spell1))
            {
                spell1.CurrentAttacksPerSecond = attackSpeed.CurrentAttackSpeed * spell1.BaseAttacksPerSecond;
                player.Set(spell1);
            }
            if (player.TryGet(out Spell2 spell2))
            {
                spell2.CurrentAttacksPerSecond = attackSpeed.CurrentAttackSpeed * spell2.BaseAttacksPerSecond;
                player.Set(spell2);
            }

            player.Set(attackSpeed);
        }

        private static void processDamage(Entity player, float pickupAmount)
        {
            SpellDamage spellDamage = player.Get<SpellDamage>();
            spellDamage.CurrentSpellDamage += spellDamage.BaseSpellDamage * pickupAmount;

            if (player.TryGet(out Spell1 spell1))
            {
                spell1.CurrentDamage = spellDamage.CurrentSpellDamage * spell1.BaseDamage;
                player.Set(spell1);
            }
            if (player.TryGet(out Spell2 spell2))
            {
                spell2.CurrentDamage = spellDamage.CurrentSpellDamage * spell2.BaseDamage;
                player.Set(spell2);
            }

            player.Set(spellDamage);
        }

        private static void processSpellEffectChance(Entity player, float pickupAmount)
        {
            SpellEffectChance spellEffectChance = player.Get<SpellEffectChance>();
            spellEffectChance.CurrentSpellEffectChance += spellEffectChance.BaseSpellEffectChance * pickupAmount;

            if (player.TryGet(out Spell1 spell1))
            {
                spell1.CurrentEffectChance = spellEffectChance.CurrentSpellEffectChance * spell1.BaseEffectChance;
                player.Set(spell1);
            }
            if (player.TryGet(out Spell2 spell2))
            {
                spell2.CurrentEffectChance = spellEffectChance.CurrentSpellEffectChance * spell2.BaseEffectChance;
                player.Set(spell2);
            }

            player.Set(spellEffectChance);
        }
    }
}
