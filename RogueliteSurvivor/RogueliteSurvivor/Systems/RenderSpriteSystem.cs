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

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player)
        {
            world.Query(in query, (ref Animation anim, ref SpriteSheet sprite) =>
            {
                spriteBatch.Draw(
                    textures[sprite.TextureName], 
                    new Vector2(125, 75), 
                    sprite.SourceRectangle(anim.CurrentFrame), 
                    Color.White, 
                    0f, 
                    new Vector2(0, 4), 
                    1f, 
                    SpriteEffects.None, 
                    0
                );
            });
        }
    }
}