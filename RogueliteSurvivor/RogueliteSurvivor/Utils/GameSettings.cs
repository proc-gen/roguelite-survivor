using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Utils
{
    public struct GameSettings
    {
        public AvailableSpells StartingSpell { get; set; }
        public string PlayerTexture { get; set; }
    }
}
