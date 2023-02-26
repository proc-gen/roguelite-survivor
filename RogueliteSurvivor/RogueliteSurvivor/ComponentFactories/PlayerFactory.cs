using Microsoft.Xna.Framework.Graphics;
using RogueliteSurvivor.Components;
using RogueliteSurvivor.Containers;
using System.Collections.Generic;

namespace RogueliteSurvivor.ComponentFactories
{
    public static class PlayerFactory
    {
        public static Animation GetPlayerAnimation(PlayerContainer player)
        {
            return new Animation(
                player.Animation.FirstFrame,
                player.Animation.LastFrame,
                player.Animation.PlaybackSpeed,
                player.Animation.NumDirections,
                player.Animation.Repeatable
            );
        }

        public static SpriteSheet GetPlayerSpriteSheet(PlayerContainer player, Dictionary<string, Texture2D> textures)
        {
            return new SpriteSheet(
                textures[player.Texture],
                player.Texture,
                player.SpriteSheet.FramesPerRow,
                player.SpriteSheet.FramesPerColumn,
                0f,
                player.SpriteSheet.Scale
            );
        }
    }
}
