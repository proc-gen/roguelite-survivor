using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
