using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct Animation
    {
        public int FirstFrame { get; set; }
        public int LastFrame { get; set; }
        public int CurrentFrame { get; set; }
        public int NumFrames { get; set; }
        public float Count { get; set; }
        public float Max { get; set; }

        public Animation(int firstFrame, int lastFrame, float max)
        {
            Max = max;
            Reset(firstFrame, lastFrame);
        }

        public void Reset(int firstFrame, int lastFrame)
        {
            FirstFrame = firstFrame;
            LastFrame = lastFrame;
            CurrentFrame = firstFrame;
            NumFrames = lastFrame - firstFrame + 1;
            Count = 0;
        }
    }
}
