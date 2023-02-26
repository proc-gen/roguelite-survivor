using RogueliteSurvivor.Constants;

namespace RogueliteSurvivor.Components
{
    public struct PickupSprite
    {
        public PickupType Type { get; set; }
        public float PickupAmount { get; set; }
        public int Max { get; private set; }
        public int Min { get; private set; }
        public int Current { get; set; }
        public int Increment { get; set; }
        public float Count { get; set; }
        public float MaxCount { get; set; }

        public PickupSprite()
        {
            Max = 8;
            Min = -8;
            Current = 0;
            Increment = 1;
            Count = 0;
            MaxCount = .1f;
        }
    }
}
