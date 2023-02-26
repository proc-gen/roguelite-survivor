namespace RogueliteSurvivor.Components
{
    public struct SpellEffectChance
    {
        public SpellEffectChance(float baseSpellEffectChance)
        {
            BaseSpellEffectChance = baseSpellEffectChance;
            CurrentSpellEffectChance = baseSpellEffectChance;
        }
        public float BaseSpellEffectChance { get; set; }
        public float CurrentSpellEffectChance { get; set; }
    }
}
