using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Struct that holds all of the data for a map. All tiles and map objects are saved as an int that refers back to an array. Various arrays are saved as Scriptable Objects 
//When a map is loaded, will reference the index back to those arrays using Resource.Load on the array name to generate the map.
namespace BoundMaps
{
    [Serializable]
    public class MapFile
    {

        public string tileset;                                                  //Store the tileset name for Resource.Load
        public string obstacleSet;                                              //Store the explosionSet name
        public string eventTriggerSet;                                          //Store the eventTriggerSet name
        public int numberOfLevels;                                              //Gets us the number of levels there are by counting the list
        public bool doubleSpeed;                                                //Double speed?
        public List<LevelData> levels = new List<LevelData>();                  //Stores all levels in a list



        //Constructor
        public MapFile(string tiles, string bombs, List<LevelData> list)
        {
            tileset = tiles;
            obstacleSet = bombs;
            levels = list;
            numberOfLevels = list.Count;
            eventTriggerSet = null;
            doubleSpeed = true;
        }

        //Constructor after adding EventTriggers
        public MapFile(string tiles, string bombs, List<LevelData> list, string eventTriggerSet)
        {
            tileset = tiles;
            obstacleSet = bombs;
            levels = list;
            numberOfLevels = list.Count;
            this.eventTriggerSet = eventTriggerSet;
            doubleSpeed = false;
        }

        //Constructor after adding EventTriggers
        public MapFile(string tiles, string bombs, List<LevelData> list, string eventTriggerSet, bool doubleSpeed)
        {
            tileset = tiles;
            obstacleSet = bombs;
            levels = list;
            numberOfLevels = list.Count;
            this.eventTriggerSet = eventTriggerSet;
            this.doubleSpeed = doubleSpeed;
        }
    }
}