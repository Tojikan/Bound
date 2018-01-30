using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Public struct to hold tile information for each map
//Be sure to use the BoundMaps namespace
namespace BoundMaps
{

	[Serializable]
    public class MapFile
    {
        public string tilesetPath;                              //string which should be the path to our chosen tileset
        public int[] levelone;                                  //int array which holds our tiles

        //Constructor
        public MapFile(string a, int[] b)
        {
            tilesetPath = a;
            levelone = b;
        }

    }

}
