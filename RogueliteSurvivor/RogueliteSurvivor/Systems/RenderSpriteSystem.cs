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
    public class RenderSpriteSystem : ArchSystem, IRenderSystem
    {
        GraphicsDeviceManager graphics;
        QueryDescription playerQuery = new QueryDescription()
                                            .WithAll<Player, Position, SpriteSheet, Animation>();
        QueryDescription enemyQuery = new QueryDescription()
                                            .WithAll<Enemy, Position, SpriteSheet, Animation>();
        QueryDescription projectileQuery = new QueryDescription()
                                            .WithAll<Projectile, Position, SpriteSheet, Animation>();

        public RenderSpriteSystem(World world, GraphicsDeviceManager graphics)
            : base(world, new QueryDescription()
                                .WithAll<Position, SpriteSheet, Animation>())
        {
            this.graphics = graphics;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player, float totalElapsedTime, GameState gameState, int layer)
        {
            if (layer == 2)
            {
                Vector2 playerPosition = player.Get<Position>().XY;
                Vector2 offset = new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight / 6);

                world.Query(in playerQuery, (ref Position pos, ref Animation anim, ref SpriteSheet sprite) =>
                {
                    Vector2 position = pos.XY - playerPosition;
                    renderEntity(spriteBatch, textures, sprite, anim, position, offset);
                });

                world.Query(in enemyQuery, (ref Enemy enemy, ref Position pos, ref Animation anim, ref SpriteSheet sprite) =>
                {
                    if (enemy.State != EntityState.Dead)
                    {
                        Vector2 position = pos.XY - playerPosition;
                        renderEntity(spriteBatch, textures, sprite, anim, position, offset);
                    }
                });

                world.Query(in projectileQuery, (ref Projectile projectile, ref Position pos, ref Animation anim, ref SpriteSheet sprite) =>
                {
                    if (projectile.State != EntityState.Dead)
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
                        .05f
                    );
            }
        }
    }
}