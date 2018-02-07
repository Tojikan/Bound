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


        //Stores our start and end points. 
        //TO DO: Can reduce map storage space by 3-4 kb by figuring out a better way of storing than Vector2
        public Vector2 startPoint;
        public Vector2 endPoint;

        //Constructor
        public LevelData(int[] ground, int[] wall, Vector3 start, Vector3 end)
        {
            groundTiles = ground;
            wallTiles = wall;
            startPoint = start;
            endPoint = end;

        }

    }

}
