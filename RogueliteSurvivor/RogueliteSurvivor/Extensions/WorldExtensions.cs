using Arch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Extensions
{
    public static class WorldExtensions
    {
        public static void TryDestroy(this World world, in Entity entity)
        {
            try
            {
                world.Destroy(entity);
            }
            catch { }
        }
    }
}
