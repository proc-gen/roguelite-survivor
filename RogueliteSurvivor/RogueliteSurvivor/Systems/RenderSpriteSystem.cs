using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;

namespace RogueliteSurvivor.Systems
{
    public class RenderSpriteSystem : ArchSystem, IRenderSystem
    {
        GraphicsDeviceManager graphics;

        public RenderSpriteSystem(World world, GraphicsDeviceManager graphics)
            : base(world, new QueryDescription()
                                .WithAll<EntityStatus, Position, SpriteSheet, Animation>())
        {
            this.graphics = graphics;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player, float totalElapsedTime, GameState gameState, int layer)
        {
            if (layer == 1)
            {
                Vector2 playerPosition = player.Get<Position>().XY;
                Vector2 offset = new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight / 6);

                world.Query(in query, (ref EntityStatus entityStatus, ref Position pos, ref Animation anim, ref SpriteSheet sprite) =>
                {
                    if (entityStatus.State != State.Dead)
                    {
                        Vector2 position = pos.XY - playerPosition;
                        renderEntity(spriteBatch, textures, sprite, anim, position, offset);
                    }
                });
            }
        }

        private void renderEntity(SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, SpriteSheet sprite, Animation anim, Vector2 position, Vector2 offset)
        {
            if (MathF.Abs(position.X) < offset.X && MathF.Abs(position.Y) < offset.Y)
            {
                spriteBatch.Draw(
                        textures[sprite.TextureName],
                        position + offset,
                        sprite.SourceRectangle(anim.CurrentFrame),
                        anim.Overlay,
                        sprite.Rotation,
                        new Vector2(sprite.Width / 2, sprite.Height / 2),
                        sprite.Scale,
                        SpriteEffects.None,
                        .075f
                    );
            }
        }
    }
}