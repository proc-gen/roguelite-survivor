using Arch.Core;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class AnimationUpdateSystem : ArchSystem, IUpdateSystem
    {
        public AnimationUpdateSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Animation>())
        {
        }

        public void Update(GameTime gameTime, float totalElapsedTime) 
        {
            world.Query(in query, (ref Animation anim) =>
            {
                anim.Count += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                if (anim.Count > anim.Max)
                {
                    anim.Count = 0;
                    if (anim.Repeatable)
                    {
                        anim.CurrentFrame = anim.CurrentFrame == anim.LastFrame ? anim.FirstFrame : anim.CurrentFrame + 1;
                    }
                    else
                    {
                        anim.CurrentFrame = anim.CurrentFrame == anim.LastFrame ? anim.LastFrame : anim.CurrentFrame + 1;
                    }
                    anim.Overlay = Color.White;
                }
            });
        }
    }
}
