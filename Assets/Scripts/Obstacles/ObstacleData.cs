using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Struct to store our data about bomb locations on a map
namespace BoundMaps
{

    [Serializable]
    public class ObstacleData
    {
        public Vector2 position;                            //Position of each bomb
        public int loopLength;                              //Length of each loop
        public int triggerTime;                             //Timing of the explosion
        public int obstacleType;                             //Type of Bomb based on our bomb set
        public int audioPlayer;                             //Which Audio Player to use


        //Constructor. If no audio player, default to 1
        public ObstacleData(Vector2 point, int loop, int time, int explode)
        {
            position = point;
            loopLength = loop;
            triggerTime = time;
            obstacleType = explode;
            audioPlayer = 1;
        }
        //Overload
        public ObstacleData(Vector2 point, int loop, int time, int explode, int audio)
        {
            position = point;
            loopLength = loop;
            triggerTime = time;
            obstacleType = explode;
            audioPlayer = audio;
        }
    }

}
