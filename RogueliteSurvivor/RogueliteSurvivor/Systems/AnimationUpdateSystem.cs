using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using System;

namespace RogueliteSurvivor.Systems
{
    public class AnimationUpdateSystem : ArchSystem, IUpdateSystem
    {
        QueryDescription burnQuery = new QueryDescription().WithAll<Burn>();
        QueryDescription slowQuery = new QueryDescription().WithAll<Slow>();
        QueryDescription shockQuery = new QueryDescription().WithAll<Shock>();

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

            world.Query(in burnQuery, (ref Animation anim) =>
            {
                anim.Overlay = Color.Orange;
            });

            world.Query(in slowQuery, (ref Animation anim) =>
            {
                anim.Overlay = Color.Blue;
            });

            world.Query(in shockQuery, (ref Animation anim) =>
            {
                anim.Overlay = Color.Yellow;
            });
        }
    }
}
