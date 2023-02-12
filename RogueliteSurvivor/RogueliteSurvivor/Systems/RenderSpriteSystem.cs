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
        public RenderSpriteSystem(World world)
            : base(world, new QueryDescription()
                                .WithAll<Position, SpriteSheet, Animation>())
        {
        }

        public static Vector2 Offset = new Vector2(125, 75);

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player)
        {
            Vector2 playerPosition = player.Get<Position>().XY;
            world.Query(in query, (ref Position pos, ref Animation anim, ref SpriteSheet sprite) =>
            {
                Vector2 position = pos.XY - playerPosition + Offset;

                spriteBatch.Draw(
                    textures[sprite.TextureName], 
                    position, 
                    sprite.SourceRectangle(anim.CurrentFrame), 
                    Color.White, 
                    0f, 
                    new Vector2(0, sprite.Height / 4), 
                    1f, 
                    SpriteEffects.None, 
                    0
                );
            });
        }
    }
}