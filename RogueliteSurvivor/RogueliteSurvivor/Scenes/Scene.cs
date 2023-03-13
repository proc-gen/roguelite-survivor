using Arch.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Containers;

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

        protected ProgressionContainer progressionContainer;

        protected float scaleFactor;

        public bool Loaded { get; protected set; }

        public Scene(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics, World world, Box2D.NetStandard.Dynamics.World.World physicsWorld, ProgressionContainer progressionContainer, float scaleFactor)
        {
            _spriteBatch = spriteBatch;
            Content = contentManager;
            _graphics = graphics;
            this.world = world;
            this.physicsWorld = physicsWorld;
            this.progressionContainer = progressionContainer;

            Loaded = false;
            this.scaleFactor = scaleFactor;
        }

        public abstract void Draw(GameTime gameTime, Matrix transform, params object[] values);
        public abstract void LoadContent();
        public abstract string Update(GameTime gameTime, params object[] values);
        protected float GetWidthOffset(float divisor)
        {
            return _graphics.PreferredBackBufferWidth / (divisor * scaleFactor);
        }
        protected float GetHeightOffset(float divisor)
        {
            return _graphics.PreferredBackBufferHeight / (divisor * scaleFactor);
        }
    }
}
