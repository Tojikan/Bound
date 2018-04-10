using BoundMaps;
using System.IO;
using UnityEngine;

namespace BoundEngine
{
    //Class to load our map from a file
    public class MapLoader : MonoBehaviour
    {
        //main method to load a map from a given file. Takes in the path to the file, reads and deserializes it, and returns a MapFile. Path should include the file name
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

        //Returns a meta from a path. Make sure path includes the file name
        public MapMetaObject LoadMeta(string path)
        {
            string metaString = ReadString(path);
            MapMetaObject newMeta = JsonUtility.FromJson<MapMetaObject>(metaString);
            return newMeta;
        }

        //Checks the file extension to make sure we're loading a map file
        private bool CheckFileExtension(string file)
        {
            if(Path.GetExtension(file) != ".bound")
            {
                Debug.Log("Invalid File Extension");
                return false;
            }
            return true;
        }

        //Method to read from a file. Returns a string
        private string ReadString(string file)
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

