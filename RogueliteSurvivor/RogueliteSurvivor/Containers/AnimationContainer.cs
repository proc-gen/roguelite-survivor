using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Containers
{
    public class AnimationContainer
    {
        public AnimationContainer() { }
        public int FirstFrame { get; set; }
        public int LastFrame { get; set; }
        public float PlaybackSpeed { get; set; }
        public int NumDirections { get; set; }
    }
}
