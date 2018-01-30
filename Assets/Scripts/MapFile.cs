using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Public struct to hold tile information for each map

namespace BoundMaps
{

	[Serializable]
    public class MapFile
    {
        //Store the tileset the map uses and stores all levels in a list
        public TileSet tileset;                             
        public List<LevelData> levels = new List<LevelData>();

        public MapFile(TileSet set, List<LevelData> list)
        {
            tileset = set;
            levels = list;
        }


    }

}
