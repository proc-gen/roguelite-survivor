using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public enum EnemyState
    {
        Alive,
        Dead
    }

    public struct Enemy
    {
        public EnemyState State { get; set; }
    }
}
