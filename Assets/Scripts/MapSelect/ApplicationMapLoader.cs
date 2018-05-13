using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;
using System.Linq;

namespace BoundMenus
{
    //Reads from map directory and creates new buttons
    public class ApplicationMapLoader : MonoBehaviour
    {
        public MapListing mapListingPrefab;                                 //Drag reference to the map listing prefab
        public GameObject mapContainer;                                     //Drag reference to the vertical layout container for all map listings

        void Start()
        {
            LinkedList<MapMetaObject> metas = GetMapMeta("./Assets/Maps");
            GenerateMapListings(metas);
        }

        //Returns a list of all map metas
        public LinkedList<MapMetaObject> GetMapMeta(string directory) {
            var directories = Directory.GetDirectories(directory);

            LinkedList<MapMetaObject> maps = new LinkedList<MapMetaObject>();
            foreach (string dir in directories) {
                string metaLoc = dir + "/meta.json";
                try
                {
                    string metaString = ReadString(metaLoc);
                    MapMetaObject mapMeta = JsonUtility.FromJson<MapMetaObject>(metaString);
                    maps.AddLast(mapMeta);
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                    Debug.Log("Invalid map");
                }
            }
            return maps;
        }

        //Reads a string from a file
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

        //Generates all the map listings at run time
        private void GenerateMapListings(LinkedList<MapMetaObject> metas)
        {
            foreach (MapMetaObject meta in metas)
            {
                //Instantiate new listing, set as child of container, and then populate the metas
                MapListing newListing = Instantiate(mapListingPrefab);
                newListing.transform.SetParent(mapContainer.transform);
                newListing.ConstructListing(meta);
            }
        }
    }
}
