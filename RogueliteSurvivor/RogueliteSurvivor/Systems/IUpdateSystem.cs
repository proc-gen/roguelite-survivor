using Microsoft.Xna.Framework;

namespace RogueliteSurvivor.Systems
{
    internal interface IUpdateSystem
    {
        void Update(GameTime gameTime, float totalElapsedTime);
    }
}
