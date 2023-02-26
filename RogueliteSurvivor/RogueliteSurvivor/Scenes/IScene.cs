using Microsoft.Xna.Framework;

namespace RogueliteSurvivor.Scenes
{
    public interface IScene
    {
        void LoadContent();
        string Update(GameTime gameTime, params object[] values);
        void Draw(GameTime gameTime, Matrix transform, params object[] values);
    }
}
