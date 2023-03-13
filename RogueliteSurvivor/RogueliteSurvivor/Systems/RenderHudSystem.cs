using Arch.Core;
using Arch.Core.Extensions;
using Box2D.NetStandard.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Constants;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace RogueliteSurvivor.Systems
{
    public class RenderHudSystem : ArchSystem, IRenderSystem, IUpdateSystem
    {
        static Vector2 HealthLocation = new Vector2(10, 10);
        static Vector2 HudLocation = new Vector2(5, 5);
        static Vector2 TimeLocation = new Vector2(-100, 10);
        static Color OffsetColor = new Color(1, 1, 1, .5f);
        const int Increment = 100;

        GraphicsDeviceManager graphics;
        Dictionary<string, SpriteFont> fonts;
        StatPage statPage = StatPage.Main;
        int statPageInt = 0;
        float stateChangeTime = .11f;
        public RenderHudSystem(World world, GraphicsDeviceManager graphics, Dictionary<string, SpriteFont> fonts)
            : base(world, new QueryDescription()
                                .WithAll<Player>())
        {
            this.graphics = graphics;
            this.fonts = fonts;
        }

        public void Update(GameTime gameTime, float totalElapsedTime, float scaleFactor)
        {
            if (stateChangeTime > InputConstants.ResponseTime)
            {
                KeyboardState kState = Keyboard.GetState();
                GamePadState gState = GamePad.GetState(PlayerIndex.One);

                if (kState.IsKeyDown(Keys.Tab) || gState.Buttons.Y == ButtonState.Pressed)
                {
                    statPageInt = (statPageInt + 1) % System.Enum.GetValues(typeof(StatPage)).Length;
                    statPage = (StatPage)statPageInt;
                    stateChangeTime = 0f;
                }
            }
            else
            {
                stateChangeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch, Dictionary<string, Texture2D> textures, Entity player, float totalElapsedTime, GameState gameState, int layer, float scaleFactor)
        {
            if (layer == 4)
            {
                int counter = 0;
                world.Query(in query, (in Entity entity, ref Health health, ref KillCount killCount, ref AttackSpeed attackSpeed, ref Speed speed
                    , ref SpellDamage spellDamage, ref SpellEffectChance spellEffectChance, ref Pierce pierce, ref AreaOfEffect areaOfEffect, ref Player playerInfo) =>
                {
                    spriteBatch.Draw(
                        textures["StatsBackground"],
                        new Rectangle((int)HudLocation.X, (int)HudLocation.Y, 146, 150),
                        new Rectangle(0, 0, 64, 64),
                        OffsetColor
                    );

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
                        fonts["FontSmall"],
                        string.Concat(health.Current, " / ", health.Max),
                        HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitX * 5 + Vector2.UnitY * 2,
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
                        fonts["FontSmall"],
                        string.Concat("Level: ", playerInfo.Level),
                        HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 16 + Vector2.UnitX * 5,
                        Color.White
                    );

                    spriteBatch.DrawString(
                        fonts["FontSmall"],
                        string.Concat("Enemies Killed: ", killCount.Count),
                        HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 28 + Vector2.UnitX * 5,
                        Color.White
                    );

                    switch (statPage)
                    {
                        case StatPage.Main:
                            renderMainStats(spriteBatch, counter, attackSpeed, speed, spellDamage, spellEffectChance, pierce, areaOfEffect);
                            break;
                        case StatPage.Spell1:
                            if(entity.TryGet(out Spell1 spell1))
                            {
                                renderSpellStats(spriteBatch, counter, spell1, pierce, areaOfEffect);
                            }
                            break;
                        case StatPage.Spell2:
                            if (entity.TryGet(out Spell2 spell2))
                            {
                                renderSpellStats(spriteBatch, counter, spell2, pierce, areaOfEffect);
                            }
                            break;
                    }
                    

                    counter++;
                });

                spriteBatch.Draw(
                        textures["StatsBackground"],
                        new Rectangle((int)TimeLocation.X + (int)GetWidthOffset(graphics, scaleFactor, 1) - 4, (int)TimeLocation.Y - 5, 100, 24),
                        new Rectangle(0, 0, 64, 64),
                        OffsetColor
                    );

                spriteBatch.DrawString(
                        fonts["Font"],
                        string.Concat("Time: ", float.Round(totalElapsedTime, 2)),
                        TimeLocation + Vector2.UnitX * GetWidthOffset(graphics, scaleFactor, 1),
                        Color.White
                    );

                if (gameState == GameState.Paused)
                {
                    spriteBatch.Draw(
                        textures["StatsBackground"],
                        new Rectangle((int)GetWidthOffset(graphics, scaleFactor, 2) - 55, (int)GetHeightOffset(graphics, scaleFactor, 2) - 4, 110, 24),
                        new Rectangle(0, 0, 64, 64),
                        Color.White
                    );

                    spriteBatch.DrawString(
                        fonts["Font"],
                        "Game Paused",
                        new Vector2((int)GetWidthOffset(graphics, scaleFactor, 2) - 50, (int)GetHeightOffset(graphics, scaleFactor, 2)),
                        Color.White
                    );
                }
                else if(gameState == GameState.WantToQuit)
                {
                    spriteBatch.Draw(
                        textures["StatsBackground"],
                        new Rectangle((int)GetWidthOffset(graphics, scaleFactor, 2) - 200, (int)GetHeightOffset(graphics, scaleFactor, 2) - 4, 400, 48),
                        new Rectangle(0, 0, 64, 64),
                        Color.White
                    );

                    spriteBatch.DrawString(
                        fonts["Font"],
                        "Are the undead getting too rough and making you quit?",
                        new Vector2((int)GetWidthOffset(graphics, scaleFactor, 2) - 188, (int)GetHeightOffset(graphics, scaleFactor, 2)),
                        Color.White
                    );

                    spriteBatch.DrawString(
                        fonts["FontSmall"],
                        "Press Enter or A button to leave like a coward",
                        new Vector2((int)GetWidthOffset(graphics, scaleFactor, 2) - 150, (int)GetHeightOffset(graphics, scaleFactor, 2) + 16),
                        Color.White
                    );
                    spriteBatch.DrawString(
                        fonts["FontSmall"],
                        "Press Escape or B button to continue fighting the undead",
                        new Vector2((int)GetWidthOffset(graphics, scaleFactor, 2) - 150, (int)GetHeightOffset(graphics, scaleFactor, 2) + 28),
                        Color.White
                    );
                }

            }
        }

        private void renderMainStats(SpriteBatch spriteBatch, int counter, AttackSpeed attackSpeed, Speed speed, SpellDamage spellDamage, SpellEffectChance spellEffectChance, Pierce pierce, AreaOfEffect areaOfEffect)
        {
            spriteBatch.DrawString(
                fonts["FontSmall"],
                "Base Stats",
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 52 + Vector2.UnitX * 5,
                Color.White
            );

            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat("Attack Speed: ", attackSpeed.CurrentAttackSpeed.ToString("F"), "x"),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 64 + Vector2.UnitX * 5,
                Color.White
            );

            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat("Spell Damage: ", spellDamage.CurrentSpellDamage.ToString("F"), "x"),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 76 + Vector2.UnitX * 5,
                Color.White
            );

            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat("Spell Effect Chance: ", spellEffectChance.CurrentSpellEffectChance.ToString("F"), "x"),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 88 + Vector2.UnitX * 5,
                Color.White
            );

            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat("Move Speed: ", speed.speed),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 100 + Vector2.UnitX * 5,
                Color.White
            );

            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat("Pierce: ", pierce.Num),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 112 + Vector2.UnitX * 5,
                Color.White
            );

            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat("Area of Effect: ", areaOfEffect.Radius, "x"),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 124 + Vector2.UnitX * 5,
                Color.White
            );
        }
    
        private void renderSpellStats(SpriteBatch spriteBatch, int counter, ISpell spell, Pierce pierce, AreaOfEffect areaOfEffect)
        {
            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat(spell.Spell.GetReadableSpellName(), " Stats"),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 52 + Vector2.UnitX * 5,
                Color.White
            );

            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat("Attack Speed: ", spell.CurrentAttacksPerSecond.ToString("F"), "/s"),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 64 + Vector2.UnitX * 5,
                Color.White
            );

            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat("Spell Damage: ", spell.CurrentDamage.ToString("F")),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 76 + Vector2.UnitX * 5,
                Color.White
            );

            spriteBatch.DrawString(
                fonts["FontSmall"],
                string.Concat("Spell Effect Chance: ", string.Format("{0:P0}", spell.CurrentEffectChance)),
                HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 88 + Vector2.UnitX * 5,
                Color.White
            );

            if(spell.Type == SpellType.Projectile)
            {
                spriteBatch.DrawString(
                    fonts["FontSmall"],
                    string.Concat("Pierce: ", pierce.Num),
                    HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 100 + Vector2.UnitX * 5,
                    Color.White
                );
            }
            else if(spell.Type == SpellType.SingleTarget)
            {
                spriteBatch.DrawString(
                    fonts["FontSmall"],
                    string.Concat("Area of Effect: ", areaOfEffect.Radius, "x"),
                    HealthLocation + (Vector2.UnitY * Increment * counter) + Vector2.UnitY * 100 + Vector2.UnitX * 5,
                    Color.White
                );
            }
        }
    }
}