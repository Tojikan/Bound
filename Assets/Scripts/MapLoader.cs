using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;
using BoundMaps;

namespace BoundEngine
{
    //Class to load our map from the file
    public class MapLoader : MonoBehaviour
    {
        //main method to load a map from a given file. Takes in the path to the file, reads and deserializes it, and returns a MapFile. 
        public MapFile LoadMap(string FileToLoad)
        {
            //Checks to see if we have a file to load
            if (FileToLoad == "" || FileToLoad == null)
            {
                Debug.Log("No Map Selected");
                return null;
            }

            if (CheckFileExtension(FileToLoad) == false)
            {
                Debug.Log("Invalid Map");
                return null;
            }
              
            //Variable to hold our map
            MapFile map;

            //Loads the map information from designated map file into a string
            string loadMap = ReadString(FileToLoad);

            //Attempt to load a map
            try
            {
                //Deserialize the mapfile string into a mapfile class
                map = JsonUtility.FromJson<MapFile>(loadMap);
            }
            catch
            {
                Debug.Log("Invalid map");
                return null;
            }

            return map;
         
        }

        //Checks the file extension to make sure we're loading a map file
        public bool CheckFileExtension(string file)
        {
            if(Path.GetExtension(file) != ".bound")
            {
                return false;
            }
            return true;
        }


        //Returns a reference to a tileset. Takes in the name of a tileset.
        public TileSet GetTileSet(string path)
        {
            //Sets our tileset path by getting the info from the mapfile and setting a path
            Object temp = Resources.Load("Tiles/Assets/" + path);
            return (TileSet)temp;
        }



        //Method to read from a file. Returns a string
        public string ReadString(string file)
        {
            //Open up a stream to the file
            StreamReader reader = new StreamReader(file);

            //Reads it into a string
            string mapInfo = reader.ReadToEnd();

            //Remember to close the stream
            reader.Close();

            return mapInfo;
        }

    }
}

