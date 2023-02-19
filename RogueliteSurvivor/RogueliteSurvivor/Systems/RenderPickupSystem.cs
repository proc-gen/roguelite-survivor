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
using RogueliteSurvivor.Constants;

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

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player, float totalElapsedTime, GameState gameState)
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
                        getSourceRectangle(sprite.Type),
                        Color.White,
                        0f,
                        new Vector2(8, 8),
                        1f,
                        SpriteEffects.None,
                        0
                    );
                }
            });
        }

        private Rectangle getSourceRectangle(PickupType pickupType)
        {
            int x = 0, y = 0;

            switch (pickupType)
            {
                case PickupType.AttackSpeed:
                    x = 80;
                    y = 64;
                    break;
                case PickupType.Damage:
                    x = 80;
                    y = 336;
                    break;
                case PickupType.MoveSpeed:
                    x = 0;
                    y = 64;
                    break;
                case PickupType.Health:
                    x = 208;
                    y = 224;
                    break;
            }

            return new Rectangle(x, y, 16, 16);
        }
    }
}