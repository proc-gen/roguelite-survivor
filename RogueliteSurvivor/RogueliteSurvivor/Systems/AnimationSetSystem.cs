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
    public class AnimationSetSystem : ArchSystem, IUpdateSystem
    {
        public AnimationSetSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Animation, Velocity>())
        {
        }

        public void Update(GameTime gameTime) 
        {
            world.Query(in query, (ref Animation anim, ref Velocity vel) =>
            {
                if(vel.Dxy.Y > 0 && anim.FirstFrame != 0)
                {
                    anim.Reset(0, 2);
                }
                else if(vel.Dxy.Y < 0 && anim.FirstFrame != 6)
                {
                    anim.Reset(6, 8);
                }
                else if(vel.Dxy.X > 0 && anim.FirstFrame != 9)
                {
                    anim.Reset(9, 11);
                }
                else if(vel.Dxy.X < 0 && anim.FirstFrame != 3)
                {
                    anim.Reset(3, 5);
                }
                else if(vel.Dxy == Vector2.Zero && anim.FirstFrame != 1)
                {
                    anim.Reset(1, 1);
                }
            });
        }
    }
}
