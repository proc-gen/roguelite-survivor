using Arch.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RogueliteSurvivor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arch.Core.Extensions;

namespace RogueliteSurvivor.Systems
{
    public class RenderSpriteSystem : ArchSystem, IRenderSystem
    {
        GraphicsDeviceManager graphics;
        public RenderSpriteSystem(World world, GraphicsDeviceManager graphics)
            : base(world, new QueryDescription()
                                .WithAll<Position, SpriteSheet, Animation>())
        {
            this.graphics = graphics;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player)
        {
            Vector2 playerPosition = player.Get<Position>().XY;
            Vector2 offset = new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight / 6);
            world.Query(in query, (in Entity entity, ref Position pos, ref Animation anim, ref SpriteSheet sprite) =>
            {
                Vector2 position = pos.XY - playerPosition;
                bool alive = true;
                if(entity.TryGet(out Enemy enemy))
                {
                    alive = enemy.State == EnemyState.Alive;
                }
                if (alive && MathF.Abs(position.X) < offset.X && MathF.Abs(position.Y) < offset.Y)
                {
                    spriteBatch.Draw(
                        textures[sprite.TextureName],
                        position + offset,
                        sprite.SourceRectangle(anim.CurrentFrame),
                        Color.White,
                        0f,
                        new Vector2(sprite.Width / 2, sprite.Height / 2),
                        1f,
                        SpriteEffects.None,
                        0
                    );
                }
            });
        }
    }
}