using Arch.Core;
using Microsoft.Xna.Framework;

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

        protected float GetWidthOffset(GraphicsDeviceManager graphics, float scaleFactor, float divisor)
        {
            return graphics.PreferredBackBufferWidth / (divisor * scaleFactor);
        }
        protected float GetHeightOffset(GraphicsDeviceManager graphics, float scaleFactor, float divisor)
        {
            return graphics.PreferredBackBufferHeight / (divisor * scaleFactor);
        }
    }
}
