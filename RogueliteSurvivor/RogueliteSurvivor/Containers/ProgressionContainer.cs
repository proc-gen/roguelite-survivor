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
        public List<EnemyKillStatsContainer> EnemyKillStats { get; set; }

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

            progressionContainer.EnemyKillStats = new List<EnemyKillStatsContainer>();
            if (progression["EnemyKillStats"] != null)
            {
                foreach (var enemyStats in progression["EnemyKillStats"])
                {
                    progressionContainer.EnemyKillStats.Add(EnemyKillStatsContainer.ToEnemyKillStatsContainer(enemyStats));
                }
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

    public class EnemyKillStatsContainer
    {
        public string Name { get; set; }
        public int Kills { get; set; }
        public int KilledBy { get; set; }

        public static EnemyKillStatsContainer ToEnemyKillStatsContainer(JToken enemyStats)
        {
            var enemyKillStats = new EnemyKillStatsContainer()
            {
                Name = (string)enemyStats["Name"],
                Kills = (int)enemyStats["Kills"],
                KilledBy = (int)enemyStats["KilledBy"]
            };

            return enemyKillStats;
        }
    }

}
