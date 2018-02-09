using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Struct to store our data about bomb locations on a map
namespace BoundMaps
{

    [Serializable]
    public class ExplosionData
    {
        public Vector2 position;                            //Position of each bomb
        public int loopLength;                              //Length of each loop
        public int explodeTime;                             //Timing of the explosion
        public int explodeType;                             //Type of Bomb based on our bomb set


        //Constructor
        public ExplosionData(Vector2 point, int loop, int time, int explode)
        {
            position = point;
            loopLength = loop;
            explodeTime = time;
            explodeType = explode;
        }
    }

}
