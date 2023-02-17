using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Scenes
{
    public interface IScene
    {
        void LoadContent();
        string Update(GameTime gameTime, params object[] values);
        void Draw(GameTime gameTime, params object[] values);
    }
}
