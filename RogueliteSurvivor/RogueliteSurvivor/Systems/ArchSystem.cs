using Arch.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    public abstract class ArchSystem
    {
        protected World world;
        protected QueryDescription query;

        public ArchSystem(World world, QueryDescription query)
        {
            this.world = world;
            this.query = query;
        }
    }
}
