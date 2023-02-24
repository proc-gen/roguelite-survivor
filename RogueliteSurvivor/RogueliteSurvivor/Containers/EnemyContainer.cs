using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Containers
{
    public class EnemyContainer
    {
        public EnemyContainer() { }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public float Speed { get; set; }
        public Spells Spell { get; set; }
        public int Width { get; set; }
        public SpriteSheetContainer SpriteSheet { get; set; }
        public AnimationContainer Animation { get; set; }
    }
}
