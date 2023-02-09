using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Helpers
{
    public class AnimationTimer
    {
        public int firstFrame, lastFrame, currentFrame, numFrames;
        public float count, max;

        public AnimationTimer(int firstFrame, int lastFrame, float max)
        {
            this.firstFrame = firstFrame;
            this.lastFrame = lastFrame;
            this.max = max;

            currentFrame = firstFrame;
            numFrames = lastFrame - firstFrame + 1;
            count = 0;
        }

        public void Reset(int firstFrame, int lastFrame)
        {
            this.firstFrame = firstFrame;
            this.lastFrame = lastFrame;
            numFrames = lastFrame - firstFrame + 1;
            currentFrame = firstFrame;
            count = 0;
        }

        public void Update(GameTime gameTime)
        {
            count += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            if(count > max)
            {
                count = 0;
                currentFrame = currentFrame == lastFrame ? firstFrame : currentFrame + 1;
            }
        }
    }
}
