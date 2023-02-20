using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct Animation
    {
        public int FirstFrame { get; private set; }
        public int LastFrame { get; private set; }
        public int CurrentFrame { get; set; }
        public int NumFrames { get; private set; }
        public float Count { get; set; }
        public float Max { get; private set; }
        public int NumDirections { get; private set; }
        public Color Overlay { get; set; }
        public bool Repeatable { get; private set; }

        public Animation(int firstFrame, int lastFrame, float max, int numDirections, bool repeatable = true)
        {
            Max = max;
            NumDirections = numDirections;
            Overlay = Color.White;
            Reset(firstFrame, lastFrame, repeatable);
        }

        public void Reset(int firstFrame, int lastFrame, bool repeatable = true)
        {
            FirstFrame = firstFrame;
            LastFrame = lastFrame;
            CurrentFrame = firstFrame;
            NumFrames = lastFrame - firstFrame + 1;
            Repeatable = repeatable;
            Count = 0;
        }

    }
}
