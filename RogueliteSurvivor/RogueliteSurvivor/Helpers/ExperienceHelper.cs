using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Helpers
{
    public static class ExperienceHelper
    {
        public static int ExperienceRequiredForLevel(int level)
        {
            return (int)(20f + MathF.Pow(level - 1, 2f));
        }
    }
}
