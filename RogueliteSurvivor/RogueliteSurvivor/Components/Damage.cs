using RogueliteSurvivor.Constants;

namespace RogueliteSurvivor.Components
{
    public struct Damage
    {
        public float Amount { get; set; }
        public float BaseAmount { get; set; }
        public SpellEffects SpellEffect { get; set; }
    }
}
