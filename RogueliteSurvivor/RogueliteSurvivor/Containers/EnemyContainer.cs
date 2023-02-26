using Newtonsoft.Json.Linq;
using RogueliteSurvivor.Constants;

namespace RogueliteSurvivor.Containers
{
    public class EnemyContainer
    {
        public EnemyContainer() { }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public float Speed { get; set; }
        public Spells Spell { get; set; }
        public int Width { get; set; }
        public SpriteSheetContainer SpriteSheet { get; set; }
        public AnimationContainer Animation { get; set; }


        public static string EnemyContainerName(JToken enemy)
        {
            return (string)enemy["name"];
        }

        public static EnemyContainer ToEnemyContainer(JToken enemy)
        {
            return new EnemyContainer()
            {
                Name = (string)enemy["name"],
                Health = (int)enemy["health"],
                Damage = (int)enemy["damage"],
                Speed = (float)enemy["speed"],
                Spell = ((string)enemy["spell"]).GetSpellFromString(),
                Width = (int)enemy["width"],
                Animation = AnimationContainer.ToAnimationContainer(enemy["animation"]),
                SpriteSheet = SpriteSheetContainer.ToSpriteSheetContainer(enemy["spriteSheet"])
            };
        }
    }
}
