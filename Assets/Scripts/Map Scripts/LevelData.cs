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

        //List of obstacles and their data
        public List<ObstacleData> obstacles = new List<ObstacleData>();

        //Stores our start and end points. 
        public Vector2 startPoint;
        public Vector2 endPoint;

        //Dialogue for the start and end of levels
        public Dialogue startDialogue;
        public Dialogue endDialogue;

        //level music
        public int music;

        //Constructor
        public LevelData(int[] ground, int[] wall, Vector3 start, Vector3 end, List<ObstacleData> bombs)
        {
            groundTiles = ground;
            wallTiles = wall;
            startPoint = start;
            endPoint = end;
            obstacles = bombs;
        }

        //Constructor
        public LevelData(int[] ground, int[] wall, Vector3 start, Vector3 end, List<ObstacleData> bombs, int song)
        {
            groundTiles = ground;
            wallTiles = wall;
            startPoint = start;
            endPoint = end;
            obstacles = bombs;
            music = song;
        }

        //Constructor
        public LevelData(int[] ground, int[] wall, Vector3 start, Vector3 end, List<ObstacleData> bombs, int song, Dialogue startDial)
        {
            groundTiles = ground;
            wallTiles = wall;
            startPoint = start;
            endPoint = end;
            obstacles = bombs;
            music = song;
            startDialogue = startDial;
        }

        //Constructor
        public LevelData(int[] ground, int[] wall, Vector3 start, Vector3 end, List<ObstacleData> bombs, int song, Dialogue startDial, Dialogue endDial)
        {
            groundTiles = ground;
            wallTiles = wall;
            startPoint = start;
            endPoint = end;
            obstacles = bombs;
            music = song;
            startDialogue = startDial;
            endDialogue = endDial;
        }
    }
}
