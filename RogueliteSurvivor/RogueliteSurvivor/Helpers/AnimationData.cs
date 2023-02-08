using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Helpers
{
    public class AnimationData
    {
        Texture2D texture;
        int framesPerRow, framesPerColumn;
        
        int width, height;
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public AnimationData(Texture2D texture, int framesPerRow, int framesPerColumn)
        {
            this.texture = texture;
            this.framesPerRow = framesPerRow;
            this.framesPerColumn = framesPerColumn;

            width = texture.Width / framesPerRow;
            height = texture.Height / framesPerColumn;
        }

        public int Column(int currentFrame)
        {
            return currentFrame % framesPerRow;
        }
        public int Row(int currentFrame)
        {
            return currentFrame / framesPerRow;
        }

        public Rectangle SourceRectangle(int currentFrame)
        {
            int column = Column(currentFrame) * Width;
            int row = Row(currentFrame) * Height;
            return new Rectangle(column, row, Width, Height);
        }
    }
}
