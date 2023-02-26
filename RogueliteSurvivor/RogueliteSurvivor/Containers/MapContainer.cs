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
        public int SpawnMinX { get; set; }
        public int SpawnMaxX { get; set; }
        public int SpawnMinY { get; set; }
        public int SpawnMaxY { get; set; }


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
                SpawnMinX = (int)map["spawnMinX"],
                SpawnMaxX = (int)map["spawnMaxX"],
                SpawnMinY = (int)map["spawnMinY"],
                SpawnMaxY = (int)map["spawnMaxY"],
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
}
