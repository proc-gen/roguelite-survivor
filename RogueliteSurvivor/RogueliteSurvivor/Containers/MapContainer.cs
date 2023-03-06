using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using RogueliteSurvivor.Constants;
using System.Collections.Generic;

namespace RogueliteSurvivor.Containers
{
    public class MapContainer
    {
        public MapContainer() { }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string MapFilename { get; set; }
        public string[] TilesetImages { get; set; }
        public Vector2 Start { get; set; }
        public List<SpawnableAreaContainer> Spawnables { get; set; }
        public List<EnemyWavesContainer> EnemyWaves { get; set; }


        public static string MapContainerName(JToken map)
        {
            return (string)map["name"];
        }

        public static MapContainer ToMapContainer(JToken map)
        {
            return new MapContainer()
            {
                Name = (string)map["name"],
                Folder = (string)map["folder"],
                MapFilename = (string)map["mapFilename"],
                TilesetImages = getTilesetImages(map),
                Start = new Vector2((int)map["startingX"], (int)map["startingY"]),
                Spawnables = SpawnableAreaContainer.ToSpawnableAreaContainer(map["spawnableAreas"]),
                EnemyWaves = EnemyWavesContainer.ToEnemyWavesContainers(map["enemyWaves"])
            };
        }

        private static string[] getTilesetImages(JToken map) 
        {
            List<string> tilesetImages = new List<string>();

            foreach(var tilesetImage in map["tilesetImages"])
            {
                tilesetImages.Add((string)tilesetImage);
            }

            return tilesetImages.ToArray();
        }
    }

    public class SpawnableAreaContainer
    {
        public int SpawnMinX { get; set; }
        public int SpawnMaxX { get; set; }
        public int SpawnMinY { get; set; }
        public int SpawnMaxY { get; set; }

        public static List<SpawnableAreaContainer> ToSpawnableAreaContainer(JToken spawnables)
        {
            List<SpawnableAreaContainer> spawnableAreaContainers = new List<SpawnableAreaContainer>();

            foreach(var spawnableArea in spawnables)
            {
                spawnableAreaContainers.Add(
                    new SpawnableAreaContainer()
                    {
                        SpawnMinX = (int)spawnableArea["spawnMinX"],
                        SpawnMaxX = (int)spawnableArea["spawnMaxX"],
                        SpawnMinY = (int)spawnableArea["spawnMinY"],
                        SpawnMaxY = (int)spawnableArea["spawnMaxY"],
                    });
            }

            return spawnableAreaContainers;
        }
    }
}
