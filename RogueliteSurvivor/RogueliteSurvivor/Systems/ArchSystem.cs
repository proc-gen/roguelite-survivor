using Arch.Core;

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
