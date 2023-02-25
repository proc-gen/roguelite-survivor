using Newtonsoft.Json.Linq;
using RogueliteSurvivor.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Containers
{
    public class PlayerContainer
    {
        public PlayerContainer() { }
        public string Name { get; set; }
        public string Texture { get; set; }
        public Spells StartingSpell { get; set; }
        public Spells SecondarySpell { get; set; }
        public int Health { get; set; }
        public float Speed { get; set; }
        public AnimationContainer Animation { get; set; }
        public SpriteSheetContainer SpriteSheet { get; set; }
        public static string GetPlayerContainerName(JToken player)
        {
            return (string)player["name"];
        }
        public static PlayerContainer ToPlayerContainer(JToken player)
        {
            return new PlayerContainer()
            {
                Name = (string)player["name"],
                Texture = (string)player["texture"],
                StartingSpell = ((string)player["startingSpell"]).GetSpellFromString(),
                SecondarySpell = ((string)player["secondarySpell"]).GetSpellFromString(),
                Health = (int)player["health"],
                Speed = (float)player["speed"],
                Animation = AnimationContainer.ToAnimationContainer(player["animation"]),
                SpriteSheet = SpriteSheetContainer.ToSpriteSheetContainer(player["spriteSheet"])
            };
        }
    }
}
