using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Components
{
    public struct SpriteSheet
    {
        public string TextureName { get; private set; }
        private int framesPerRow, framesPerColumn;
        private int width, height;

        public SpriteSheet(Texture2D texture, string textureName, int framesPerRow, int framesPerColumn)
        {
            this.TextureName = textureName;
            this.framesPerRow = framesPerRow;
            this.framesPerColumn = framesPerColumn;

            width = texture.Width / framesPerRow;
            height = texture.Height / framesPerColumn;
        }

        private int Column(int currentFrame)
        {
            return currentFrame % framesPerRow;
        }

        private int Row(int currentFrame)
        {
            return currentFrame / framesPerRow;
        }

        public Rectangle SourceRectangle(int currentFrame)
        {
            int column = Column(currentFrame) * width;
            int row = Row(currentFrame) * height;
            return new Rectangle(column, row, width, height);
        }
    }
}
