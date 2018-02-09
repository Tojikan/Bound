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
        public float countdownTime;                         //Location of each bomb
        public int explodeType;                             //Type of Bomb based on our bomb set


        //Constructor
        public ExplosionData(Vector2 point, float countdown, int explode)
        {
            position = point;
            countdownTime = countdown;
            explodeType = explode;
        }
    }

}
