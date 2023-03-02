using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace RogueliteSurvivor.Containers
{
    public class EnemyWavesContainer
    {
        public EnemyWavesContainer() { }
        public bool Repeat { get; set; }
        public int Start { get; set; }
        public int MaxEnemies { get; set; }
        public List<EnemyWeightContainer> Enemies { get; set; }

        public static List<EnemyWavesContainer> ToEnemyWavesContainers(JToken enemyWavesContainers)
        {
            List<EnemyWavesContainer> enemyWaves = new List<EnemyWavesContainer>();

            foreach(var enemyWave in enemyWavesContainers)
            {
                enemyWaves.Add(new EnemyWavesContainer()
                {
                    Repeat = (bool)enemyWave["repeat"],
                    Start = (int)enemyWave["start"],
                    MaxEnemies = (int)enemyWave["maxEnemies"],
                    Enemies = EnemyWeightContainer.ToEnemyWeightContainers(enemyWave["enemies"])
                });
            }

            return enemyWaves;
        }
    }

    public class EnemyWeightContainer
    {
        public EnemyWeightContainer() { }
        public string Type { get; set; }
        public int Weight { get; set; }

        public static List<EnemyWeightContainer> ToEnemyWeightContainers(JToken enemyWeightContainers) 
        {
            List<EnemyWeightContainer> enemyWeights = new List<EnemyWeightContainer>();

            foreach(var enemyWeightContainer in enemyWeightContainers)
            {
                enemyWeights.Add(new EnemyWeightContainer()
                {
                    Type = (string)enemyWeightContainer["type"],
                    Weight = (int)enemyWeightContainer["weight"]
                });
            }

            return enemyWeights;
        }
    }
}
