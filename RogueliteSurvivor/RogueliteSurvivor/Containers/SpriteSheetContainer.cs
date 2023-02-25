using Newtonsoft.Json.Linq;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Containers
{
    public class SpriteSheetContainer
    {
        public SpriteSheetContainer() { }
        public int FramesPerRow { get; set; }
        public int FramesPerColumn { get; set; }
        public string Rotation { get; set; }
        public float Scale { get; set; }

        public static SpriteSheetContainer ToSpriteSheetContainer(JToken spriteSheet)
        {
            return !spriteSheet.HasValues ? null : new SpriteSheetContainer()
            {
                FramesPerRow = (int)spriteSheet["framesPerRow"],
                FramesPerColumn = (int)spriteSheet["framesPerColumn"],
                Rotation = (string)spriteSheet["rotation"],
                Scale = (float)spriteSheet["scale"],
            };
        }
    }
}
