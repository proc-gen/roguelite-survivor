using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Systems
{
    internal interface IUpdateSystem
    {
        void Update(GameTime gameTime, float totalElapsedTime);
    }
}
