using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Scenes
{
    public abstract class Scene : IScene
    {
        protected SpriteBatch _spriteBatch;
        protected ContentManager Content;
        protected GraphicsDeviceManager _graphics;

        public Scene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            _spriteBatch = spriteBatch;
            Content = contentManager;
            _graphics = graphics;
        }

        public abstract void Draw(GameTime gameTime);
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
    }
}
