using Arch.Core;

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
