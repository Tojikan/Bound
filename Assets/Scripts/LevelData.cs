using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Public struct to hold tile information for each level
namespace BoundMaps
{

    [Serializable]
    public class LevelData
    {
        //Two layers to store tiles in
        public int[] groundTiles;                                  
        public int[] wallTiles;

        //Constructor
        public LevelData(int[] a, int[] b)
        {
            groundTiles = a;
            wallTiles = b;
        }

    }

}
