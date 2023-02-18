using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public class PlayerInputSystem : ArchSystem, IUpdateSystem
    {
        public PlayerInputSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Player>())
        {
        }

        static Vector2 InverseY = new Vector2(1, -1);

        public void Update(GameTime gameTime, float totalElapsedTime)
        {
            KeyboardState kState = Keyboard.GetState();
            GamePadState gState = GamePad.GetState(PlayerIndex.One);

            world.Query(in query, (ref Velocity vel, ref Speed sp) =>
            {
                vel.Vector = Vector2.Zero;
                if (kState.GetPressedKeyCount() > 0)
                {
                    var keys = kState.GetPressedKeys();

                    if (keys.Contains(Keys.Up) || keys.Contains(Keys.Down) || keys.Contains(Keys.Left) || keys.Contains(Keys.Right))
                    { 
                        if (keys.Contains(Keys.Up))
                        {
                            vel.Vector -= Vector2.UnitY;
                        }
                        if (keys.Contains(Keys.Down))
                        {
                            vel.Vector += Vector2.UnitY;
                        }
                        if (keys.Contains(Keys.Left))
                        {
                            vel.Vector -= Vector2.UnitX;
                        }
                        if (keys.Contains(Keys.Right))
                        {
                            vel.Vector += Vector2.UnitX;
                        }

                        vel.Vector = Vector2.Normalize(vel.Vector) * sp.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
                else if(gState.ThumbSticks.Left != Vector2.Zero)
                {
                    vel.Vector = Vector2.Normalize(gState.ThumbSticks.Left) * InverseY * sp.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            });
        }
    }
}
