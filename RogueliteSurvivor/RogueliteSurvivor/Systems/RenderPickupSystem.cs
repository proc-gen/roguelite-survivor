using Arch.Core;
using Arch.Core.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using RogueliteSurvivor.Helpers;
using System;
using System.Collections.Generic;

namespace RogueliteSurvivor.Systems
{
    public class RenderPickupSystem : ArchSystem, IRenderSystem
    {
        GraphicsDeviceManager graphics;
        public RenderPickupSystem(World world, GraphicsDeviceManager graphics)
            : base(world, new QueryDescription()
                                .WithAll<Position, PickupSprite>())
        {
            this.graphics = graphics;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player, float totalElapsedTime, GameState gameState, int layer)
        {
            if (layer == 1)
            {
                Vector2 playerPosition = player.Get<Position>().XY;
                Vector2 offset = new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight / 6);

                world.Query(in query, (ref Position pos, ref PickupSprite sprite) =>
                {
                    Vector2 position = pos.XY - playerPosition;
                    if (MathF.Abs(position.X) < offset.X && MathF.Abs(position.Y) < offset.Y)
                    {
                        spriteBatch.Draw(
                            textures["pickups"],
                            position + offset + (Vector2.UnitY * sprite.Current),
                            PickupHelper.GetPickupSourceRectangle(sprite.Type),
                            Color.White,
                            0f,
                            new Vector2(8, 8),
                            1f,
                            SpriteEffects.None,
                            .1f
                        );
                    }
                });
            }
        }

    }
}