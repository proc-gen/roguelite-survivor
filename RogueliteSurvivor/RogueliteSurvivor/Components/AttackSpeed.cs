namespace RogueliteSurvivor.Components
{
    public struct AttackSpeed
    {
        public AttackSpeed(float baseAttackSpeed)
        {
            BaseAttackSpeed = baseAttackSpeed;
            CurrentAttackSpeed = baseAttackSpeed;
        }

        public float BaseAttackSpeed { get; set; }
        public float CurrentAttackSpeed { get; set; }
    }
}
