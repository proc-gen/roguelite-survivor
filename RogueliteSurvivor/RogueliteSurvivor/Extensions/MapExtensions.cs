using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace RogueliteSurvivor.Extensions
{
    public static class MapExtensions
    {
        public static Dictionary<int, TiledTileset> GetTiledTilesetsCrossPlatform(this TiledMap tiledMap, string src)
        {
            Dictionary<int, TiledTileset> dictionary = new Dictionary<int, TiledTileset>();
            if (tiledMap.Tilesets == null)
            {
                return dictionary;
            }

            TiledMapTileset[] tilesets = tiledMap.Tilesets;
            foreach (TiledMapTileset tiledMapTileset in tilesets)
            {
                string text = Path.Combine(src, tiledMapTileset.source);
                if (tiledMapTileset.source != null)
                {
                    if (!File.Exists(text))
                    {
                        throw new TiledException("Cannot locate tileset '" + text + "'. Please make sure the source folder is correct and it ends with a slash.");
                    }

                    dictionary.Add(tiledMapTileset.firstgid, new TiledTileset(text));
                }
            }

            return dictionary;
        }
    }
}
