using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueliteSurvivor.Containers
{
    public class ProgressionContainer
    {
        public List<LevelProgressionContainer> LevelProgressions { get; set; }

        public void Save()
        {
            var jObject = JObject.FromObject(this);
            if (!Directory.Exists(Path.Combine("Saves")))
            {
                Directory.CreateDirectory(Path.Combine("Saves"));
            }

            File.WriteAllText(Path.Combine("Saves", "savegame.json"), jObject.ToString());
        }

        public static ProgressionContainer ToProgressionContainer(JToken progression)
        {
            var progressionContainer = new ProgressionContainer();

            progressionContainer.LevelProgressions = new List<LevelProgressionContainer>();
            foreach (var level in progression["LevelProgressions"])
            {
                progressionContainer.LevelProgressions.Add(LevelProgressionContainer.ToLevelProgressionContainer(level));
            }

            return progressionContainer;
        }
    }

    public class LevelProgressionContainer
    {
        public string Name { get; set; }
        public float BestTime { get; set; }
    
        public static LevelProgressionContainer ToLevelProgressionContainer(JToken level)
        {
            var levelProgression = new LevelProgressionContainer()
            {
                Name = (string)level["Name"],
                BestTime = (float)level["BestTime"]
            };

            return levelProgression;
        }
    }

}
