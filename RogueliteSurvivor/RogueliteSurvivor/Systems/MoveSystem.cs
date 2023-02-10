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
    public class MoveSystem : ArchSystem, IUpdateSystem
    {
        public MoveSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Velocity, Position>())
        {
        }

        public void Update(GameTime gameTime)
        {
            world.Query(in query, (ref Velocity vel, ref Position pos) =>
            {
                pos.XY += vel.Dxy;
            });
        }
    }
}
