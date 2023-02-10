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

        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            world.Query(in query, (ref Velocity vel, ref Speed sp) =>
            {
                vel.Dxy = Vector2.Zero;
                if (kState.GetPressedKeyCount() > 0)
                {
                    if (kState.IsKeyDown(Keys.Up))
                    {
                        vel.Dxy -= Vector2.UnitY;
                    }
                    if (kState.IsKeyDown(Keys.Down))
                    {
                        vel.Dxy += Vector2.UnitY;
                    }
                    if (kState.IsKeyDown(Keys.Left))
                    {
                        vel.Dxy -= Vector2.UnitX;
                    }
                    if (kState.IsKeyDown(Keys.Right))
                    {
                        vel.Dxy += Vector2.UnitX;
                    }

                    vel.Dxy = Vector2.Normalize(vel.Dxy) * sp.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            });
        }
    }
}
