using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueliteSurvivor.Scenes
{
    public abstract class Scene : IScene
    {
        protected SpriteBatch _spriteBatch;
        protected ContentManager Content;
        protected GraphicsDeviceManager _graphics;

        protected World world;
        protected Box2D.NetStandard.Dynamics.World.World physicsWorld;
        protected System.Numerics.Vector2 gravity = System.Numerics.Vector2.Zero;

        public bool Loaded { get; protected set; }

        public Scene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld)
        {
            _spriteBatch = spriteBatch;
            Content = contentManager;
            _graphics = graphics;
            this.world = world;
            this.physicsWorld = physicsWorld;

            Loaded = false;
        }

        public abstract void Draw(GameTime gameTime, Matrix transform, params object[] values);
        public abstract void LoadContent();
        public abstract string Update(GameTime gameTime, params object[] values);
    }
}
