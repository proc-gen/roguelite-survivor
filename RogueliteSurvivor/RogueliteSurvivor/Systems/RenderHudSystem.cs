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
    public class RenderHudSystem : ArchSystem, IRenderSystem
    {
        GraphicsDeviceManager graphics;
        Dictionary<string, SpriteFont> fonts;
        public RenderHudSystem(World world, GraphicsDeviceManager graphics, Dictionary<string, SpriteFont> fonts)
            : base(world, new QueryDescription()
                                .WithAll<Player>())
        {
            this.graphics = graphics;
            this.fonts = fonts;
        }

        static Vector2 HealthLocation = new Vector2(10, 10);
        static Vector2 TimeLocation = new Vector2(-100, 10);
        const int Increment = 64;

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player, float totalElapsedTime, GameState gameState)
        {
            int counter = 0;
            world.Query(in query, (ref Health health, ref KillCount killCount) =>
            {
                spriteBatch.Draw(
                    textures["HealthBar"],
                    HealthLocation + (Vector2.UnitY * Increment * counter),
                    new Rectangle(0, 0, (int)(textures["HealthBar"].Width * ((float)health.Current / health.Max)), textures["HealthBar"].Height),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0
                );

                spriteBatch.DrawString(
                    fonts["Font"], 
                    string.Concat(health.Current, " / ", health.Max), 
                    HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitX * 5, 
                    Color.White
                );

                spriteBatch.Draw(
                    textures["StatBar"],
                    HealthLocation + (Vector2.UnitY * Increment * counter),
                    new Rectangle(0, 0, textures["StatBar"].Width, textures["StatBar"].Height),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0
                );

                spriteBatch.DrawString(
                    fonts["Font"],
                    string.Concat("Enemies Killed: ", killCount.Count),
                    HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * Increment / 2,
                    Color.White
                );

                counter++;
            });

            spriteBatch.DrawString(
                    fonts["Font"],
                    string.Concat("Time: ", float.Round(totalElapsedTime, 2)),
                    TimeLocation + Vector2.UnitX * (graphics.PreferredBackBufferWidth / 3),
                    Color.White
                );

            if(gameState == GameState.Paused)
            {
                spriteBatch.DrawString(
                    fonts["Font"],
                    "Game Paused",
                    new Vector2(graphics.PreferredBackBufferWidth / 6 - 50, graphics.PreferredBackBufferHeight / 6),
                    Color.White
                );
            }
        }
    }
}