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
        
        public string tileset;                                                  //Store the tileset the map uses as a path to the tileset
        public string obstacleSet;                                             //Store the explosionSet asset as a path 
        public List<LevelData> levels = new List<LevelData>();                  //Stores all levels in a list
        public int numberOfLevels;                                              //Gets us the number of levels there are by counting the list

        //Constructor
        public MapFile(string tiles, string bombs, List<LevelData> list)
        {
            tileset = tiles;
            obstacleSet = bombs;
            levels = list;
            numberOfLevels = list.Count;
        }


    }

}
