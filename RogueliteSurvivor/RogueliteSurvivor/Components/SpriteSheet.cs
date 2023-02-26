using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueliteSurvivor.Components
{
    public struct SpriteSheet
    {
        public string TextureName { get; private set; }
        public int framesPerRow { get; private set; }
        public int framesPerColumn { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }

        public SpriteSheet(Texture2D texture, string textureName, int framesPerRow, int framesPerColumn, float rotation = 0, float scale = 1)
        {
            this.TextureName = textureName;
            this.framesPerRow = framesPerRow;
            this.framesPerColumn = framesPerColumn;

            Width = texture.Width / framesPerRow;
            Height = texture.Height / framesPerColumn;

            Rotation = rotation;
            Scale = scale;
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
            int column = Column(currentFrame) * Width;
            int row = Row(currentFrame) * Height;
            return new Rectangle(column, row, Width, Height);
        }
    }
}
