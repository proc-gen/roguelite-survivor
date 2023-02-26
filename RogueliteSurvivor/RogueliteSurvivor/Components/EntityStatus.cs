using RogueliteSurvivor.Constants;

namespace RogueliteSurvivor.Components
{
    public struct EntityStatus
    {
        public EntityStatus()
        {
            State = State.Alive;
        }
        public State State { get; set; }

    }
}
