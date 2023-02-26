using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using System;

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
            world.Query(in query, (in Entity entity, ref Animation anim) =>
            {
                if (entity.IsAlive())
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
                        if (entity.Has<Burn>())
                        {
                            anim.Overlay = Color.Orange;
                        }
                        else if (entity.Has<Slow>())
                        {
                            anim.Overlay = Color.Blue;
                        }
                        else if (entity.Has<Shock>())
                        {
                            anim.Overlay = Color.Yellow;
                        }
                    }
                }
            });
        }
    }
}
