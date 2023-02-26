using Arch.Core;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;

namespace RogueliteSurvivor.Systems
{
    public class AnimationSetSystem : ArchSystem, IUpdateSystem
    {
        public AnimationSetSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Animation, Velocity>())
        {
        }

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            world.Query(in query, (ref Animation anim, ref SpriteSheet spriteSheet, ref Velocity vel) =>
            {
                if (anim.NumDirections == 4)
                {
                    if (vel.Vector.Y > 0)
                    {
                        if (anim.FirstFrame != 0)
                        {
                            anim.Reset(0, 2);
                        }
                    }
                    else if (vel.Vector.Y < 0)
                    {
                        if (anim.FirstFrame != 6)
                        {
                            anim.Reset(6, 8);
                        }
                    }
                    else if (vel.Vector.X > 0)
                    {
                        if (anim.FirstFrame != 9)
                        {
                            anim.Reset(9, 11);
                        }
                    }
                    else if (vel.Vector.X < 0)
                    {
                        if (anim.FirstFrame != 3)
                        {
                            anim.Reset(3, 5);
                        }
                    }
                    else if (vel.Vector == Vector2.Zero)
                    {
                        if (anim.FirstFrame != 1)
                        {
                            anim.Reset(1, 1);
                        }
                    }
                }
                else if (anim.NumDirections == 2)
                {
                    if (vel.Vector.X > 0)
                    {
                        if (anim.FirstFrame != 0)
                        {
                            anim.Reset(0, spriteSheet.framesPerRow - 1);
                        }
                    }
                    else
                    {
                        if (anim.FirstFrame != spriteSheet.framesPerRow)
                        {
                            anim.Reset(spriteSheet.framesPerRow, (spriteSheet.framesPerRow * 2) - 1);
                        }
                    }
                }
                else if (anim.NumDirections == 1)
                {
                    //Do nothing
                }
            });
        }
    }
}
