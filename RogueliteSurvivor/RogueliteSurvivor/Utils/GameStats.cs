using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Utils
{
    public class GameStats
    {
        public float PlayTime { get; set; }
        public string Killer { get; set; }
        public int EnemiesKilled { get; set; }
        public Dictionary<string, int> Kills { get; set; }
    }
}
