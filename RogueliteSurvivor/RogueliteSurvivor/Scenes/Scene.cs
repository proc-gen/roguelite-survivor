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

        public bool Loaded {  get; protected set; }

        public Scene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            _spriteBatch = spriteBatch;
            Content = contentManager;
            _graphics = graphics;

            Loaded = false;
        }

        public abstract void Draw(GameTime gameTime, params object[] values);
        public abstract void LoadContent();
        public abstract string Update(GameTime gameTime, params object[] values);
    }
}
