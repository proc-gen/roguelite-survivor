using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace RogueliteSurvivor.Components
{
    public class MapInfo
    {
        public TiledMap Map { get; set; }
        public Dictionary<int, TiledTileset> Tilesets { get; set; }

        public MapInfo(string mapPath, string tilesetPath) 
        {
            Map = new TiledMap(mapPath);
            Tilesets = Map.GetTiledTilesets(tilesetPath);
        }
    }
}
