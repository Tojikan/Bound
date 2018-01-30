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
        //Store the tileset the map uses as a path to the tileset and stores all levels in a list
        public string tileset;                             
        public List<LevelData> levels = new List<LevelData>();
        public int numberOfLevels;

        public MapFile(string set, List<LevelData> list)
        {
            tileset = set;
            levels = list;
            numberOfLevels = list.Count;
        }


    }

}
