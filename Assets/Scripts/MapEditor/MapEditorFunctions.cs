using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using BoundMaps;
using BoundEngine;

namespace BoundEditor
{
    //Class that handles all the logic and functionalities for saving and loading maps into the Map Editor. We set the references all in this class so the controller class is cleaner.
    [ExecuteInEditMode]
    public class MapEditorFunctions : MonoBehaviour
    {
        public MapDataContainer mapDataObject;                                              //Drag a reference to the scriptable object in which the map data is stored in. 
        public TileSet tileSet;                                                             //Tileset scriptable object which contains an array holding references to our scriptable tiles. Lets us store the map tiles as ints to save space
        public Tilemap groundLayer;                                                         //Ground layer for tiles. Use for ground level tiles
        public Tilemap wallLayer;                                                           //Wall Layer for tiles. Use for any higher level tiles
        public ObstacleSet obstacleSet;                                                     //Explosion set to store references to our exploder prefabs
        public EventTriggerSet eventSet;                                                    //Drag desired eventtrigger set here
        public GameObject obstacleContainer;                                                //Container to save obstacles. Saves all the children objects of it. 
        public GameObject objectContainer;                                                  //Container to save map objects. Saves all children objects.
        public GameObject spawnPoint;                                                       //Drag any game object to this in the editor window to set our spawnpoint
        public GameObject finishPoint;                                                      //Drag any game object to this in the editor window to set our finish point
        public BoundsInt GameArea;                                                          //Sets the bounds for our game area and where we save from  
        public string mainDirectory = "./Assets/Maps/";                                     //main directory to save maps to
        public string fileExtension = ".bound";                                             //The file extension to save the map under

        private MapLoader mapLoader;                                                        //mapLoader component so we can load a level/map without having to go into play mode
        private RenderMap renderMap;                                                        //rendermap component to render the map
        private ObjectManager objectManager;                                                //Obstacle Manager component so we can reload obstacles from previous levels
        private MapEditorControl editorWindow;                                              //The interface window to handle method calls and map string inputs

        void Awake()
        {
            mapLoader = GetComponent<MapLoader>();
            renderMap = GetComponent<RenderMap>();
            objectManager = GetComponent<ObjectManager>();
            editorWindow = GetComponent<MapEditorControl>();

        }

        //Saves map information such as map meta and data sets
        #region Map Data Saving
        //Saves all the maps data set names and other map data
        public void SaveMapInfo()
        {
            string newDirectory = GetNewDirectory(editorWindow.mapName);
            SaveMapMeta(newDirectory);

            mapDataObject.mapData.obstacleSet = obstacleSet.name;
            mapDataObject.mapData.tileset = tileSet.name;
            mapDataObject.mapData.eventTriggerSet = eventSet.name;
            mapDataObject.mapData.doubleSpeed = editorWindow.doubleSpeed;


        }


        //Saves current map meta information from the editor window to the scriptable object
        private void SaveMapMeta(string directory)
        {
            //String that goes to the .bound file with the map data in it
            string fileLoc = directory + editorWindow.mapName + fileExtension;

            mapDataObject.meta.name = editorWindow.mapName;
            mapDataObject.meta.fileLocation = fileLoc; 
            mapDataObject.meta.description = editorWindow.mapDescrip;
            mapDataObject.meta.imagePath = editorWindow.mapImage.name;
            Debug.Log("Saving map meta information to Map Data Object");
        }

        //Clears the map data container object
        public void ClearContainerData()
        {
            mapDataObject.ClearMeta();
            mapDataObject.ClearMapData();
            Debug.Log("Cleared all map data");
        }


        #endregion

        //methods that save levels
        #region Level Saving 

        //Overwrite a given level in the map scriptable object
        public void OverwriteLevel(int levelNumber)
        {
            LevelData level = SaveLevel();
            try
            {
                mapDataObject.mapData.levels[levelNumber] = level;
                Debug.Log("Overwrote level " + levelNumber + "with current data");
            }
            catch (Exception e)
            {
                Debug.LogError("Unable to write new level: " + e);
            }
        }


        //Saves the current level data and adds it to the map scriptable object
        public void AddLevel()
        {
            LevelData level = SaveLevel();
            try
            {
                mapDataObject.mapData.levels.Add(level);
                Debug.Log("Added a new level");
            }
            catch (Exception e)
            {
                Debug.LogError("Unable to add new level: " + e);
            }
        }

        //Returns the leveldata of the current information on the map
        private LevelData SaveLevel()
        {
            LevelData newLevel = new LevelData(SaveTiles(groundLayer), SaveTiles(wallLayer), GetStartEndLocations(true), GetStartEndLocations(false),
                                                SaveObstacles(), GetSongNumber() , SaveDialogue(true), SaveDialogue(false), GetLevelTitle(), SaveEventTriggers());

            return newLevel;
        }

        //Remove a specific level from map data container
        public void RemoveLevel(int level)
        {
            mapDataObject.mapData.levels.Remove(mapDataObject.mapData.levels[level]);
            Debug.Log("Removed Level " + level);
        }

        //Clears all level data 
        public void ClearLevels()
        {
            mapDataObject.mapData.levels.Clear();
        }

        #endregion

        //Private methods that save all of the tiles, obstacles, event triggers, and spawn/endpoints of a map
        #region Component Saves

        //Method to save our tiles into an array
        private int[] SaveTiles(Tilemap tilemap)
        {
            //Declare new array based on game area size
            int[] tileArray = new int[GameArea.size.x * GameArea.size.y];

            //counter variable for our array storage
            int index = 0;

            //iterate over each tile in our tilemap
            for (int column = GameArea.position.y; column < (GameArea.position.y + GameArea.size.y); column++)
            {
                for (int row = GameArea.position.x; row < (GameArea.position.x + GameArea.size.x); row++)
                {
                    //Gets the current tile and stores it
                    TileBase currentTile = tilemap.GetTile(new Vector3Int(row, column, 0));

                    //Gets an int value for that tile by searching it against the tileset array
                    int tileValue = Array.IndexOf(tileSet.tilesArray, currentTile);

                    tileArray[index] = tileValue;
                    index++;
                }
            }
            Debug.Log("Got Tile Data over bounded region at position: " + GameArea.position + " with size: " + GameArea.size);
            return tileArray;
        }

        //Saves all of our Explosion Prefabs that are under our obstacle container into our scriptable object
        private List<ObstacleData> SaveObstacles()
        {
            Transform obstacleParent = obstacleContainer.GetComponent<Transform>();
            List<ObstacleData> obstacleList = new List<ObstacleData>();

            foreach (Transform child in obstacleParent)
            {
                Obstacle childData = child.GetComponent<Obstacle>();

                //Get an int by referencing child's sprite with the designated obstacleset.
                int type = Array.IndexOf(obstacleSet.obstacleSprites, child.GetComponent<SpriteRenderer>().sprite);

                //Creates new Explosion Data and initializes it
                ObstacleData newData = new ObstacleData(child.transform.position, childData.loopLength, childData.triggerTime, type, (int)childData.SelectSFXPlayer);
                obstacleList.Add(newData);
            }
            Debug.Log("Got " + obstacleList.Count + " Obstacles.");
            return obstacleList;
        }


        //Method to save all EventTriggers into a list
        private List<EventTriggerData> SaveEventTriggers()
        {
            Transform objTransform = objectContainer.GetComponent<Transform>();
            List<EventTriggerData> objList = new List<EventTriggerData>();


            foreach (Transform child in objTransform)
            {
                EventTrigger childData = child.gameObject.GetComponent<EventTrigger>();

                //Get an int by referencing child's sprite with the eventset
                int type = Array.IndexOf(eventSet.sprites, child.GetComponent<SpriteRenderer>().sprite);

                bool state = childData.startsEnabled;

                EventTriggerData newObjectData = new EventTriggerData(child.transform.position, type, state);
                objList.Add(newObjectData);
            }
            Debug.Log("Got " + objList.Count + " Event Triggers.");
            return objList;
        }

        //Gets the position of start and end points. Set the bool to true to return the start point
        private Vector2 GetStartEndLocations(bool start)
        {
            if (start)
            {
                Debug.Log("Got spawn point at " + spawnPoint.transform.position);
                return spawnPoint.transform.position;
            }
            else
            {
                Debug.Log("Got finish point at " + finishPoint.transform.position);
                return finishPoint.transform.position;
            }
        }

        //Save the dialogue that is written in the MapEditorWindow component. Set bool to start in order to save as start or false to save as end dialogue
        private Dialogue SaveDialogue(bool start)
        {
            if (start)
            {
                Debug.Log("Got Start dialogue with total " + editorWindow.startDialogue.sentences.Length + " dialogues");
                return editorWindow.startDialogue;
            }
            else
            {
                Debug.Log("Got End Dialogue with total " + editorWindow.endDialogue.sentences.Length + " dialogues");
                return editorWindow.endDialogue;
            }
        }

        private string GetLevelTitle()
        {
            return editorWindow.levelTitle;
        }

        private int GetSongNumber()
        {
            Debug.Log("Getting Music Number for " + editorWindow.selectMusic);
            return (int)editorWindow.selectMusic;
        }


        #endregion

        //For writing the map into a file
        #region FileWriting
        //public method to call the writing functions
        public void WriteToFile()
        {
            MapWriter();
            MetaWriter();
            Debug.Log("Map was successfully created!");
        }

        //Attempts to write the map data into a file
        private void MapWriter()
        {
            MapFile newMap;
            try
            {
                newMap = GenerateMapFile();
            }
            catch (Exception e)
            {
                Debug.LogError("Unable to generate a map file. Map Writing Failed");
                Debug.LogError(e);
                return;
            }

            //Get directory and append map names and filetypes
            string mapPath = GetNewDirectory(editorWindow.mapName);
            mapPath = mapPath + editorWindow.mapName + fileExtension;

            //Serializes the mapfile into a string
            string mapData = EditorJsonUtility.ToJson(newMap);

            //Tries to write our data into the path. If unable to write for whatever reason, we just get a debug
            try
            {
                FileWriter(mapPath, mapData);
                Debug.Log("Successfully saved map!");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Unable to save map");
            }
        }

        //Attempts to write the meta information into a meta.json file
        private void MetaWriter()
        {
            MapMetaObject newMeta;
            try
            {
                newMeta = GenerateMetaObject();
            }
            catch (Exception e)
            {
                Debug.LogError("Unable to generate a meta file. Meta Writing Failed");
                Debug.LogError(e);
                return;
            }

            //Get directory and append map names and filetypes
            string metaPath = GetNewDirectory(editorWindow.mapName);
            metaPath = metaPath + "meta.json";

            //Serializes the mapfile into a string
            string metaData = EditorJsonUtility.ToJson(newMeta);

            //Tries to write our data into the path. If unable to write for whatever reason, we just get a debug
            try
            {
                FileWriter(metaPath, metaData);
                Debug.Log("Succesfully saved meta!");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Unable to save map meta");
            }
        }

        //Generates a meta object from the map data container
        private MapMetaObject GenerateMetaObject()
        {
            MapMetaObject newMeta = new MapMetaObject(mapDataObject.meta.name, mapDataObject.meta.fileLocation, mapDataObject.meta.description, mapDataObject.meta.imagePath);
            return newMeta;
        }

        //Generate a map file from the map data container
        private MapFile GenerateMapFile()
        {
            MapFile newMap = new MapFile(mapDataObject.mapData.tileset, mapDataObject.mapData.obstacleSet, mapDataObject.mapData.levels, mapDataObject.mapData.eventTriggerSet, mapDataObject.mapData.doubleSpeed);
            return newMap;
        }

        //Writes info to a given path
        private void FileWriter(string path, string info)
        {
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(info);
            writer.Close();
        }

        //Creates a new folder given a map name and returns the path of that folder. Based on the standard of creating new folders for each new map
        private string GetNewDirectory(string mapName)
        {
            string path = mainDirectory + mapName + "/";
            Directory.CreateDirectory(path);
            return path;
        }
        #endregion


        #region Opening map files back into the Editor

        //Lets us load a map file into the editor. 
        public void LoadMapInEditor(string filePath)
        {
            //Get the map data
            MapFile loadedMap = mapLoader.LoadMap(filePath);
            Debug.Log("Loading Map Data with level count: " + loadedMap.levels.Count);

            //Get the meta 
            string metaLocation = RemoveMapName(filePath);
            metaLocation = metaLocation + "meta.json";
            MapMetaObject loadedMeta = mapLoader.LoadMeta(metaLocation);
            Debug.Log("Getting map meta of map name: " + loadedMeta.name);

            //Set the scriptable object container
            mapDataObject.mapData = loadedMap;
            mapDataObject.meta = loadedMeta;
            Debug.Log("Map Successfully Loaded");
        }

        //Removes the .fileExtension at the end of path so we can get the directory
        private string RemoveMapName(string path)
        {
            int index = path.LastIndexOf("/");
            string metaPath = path.Substring(0, index + 1);
            Debug.Log(metaPath);
            return metaPath;
        }

        //Renders a given level number
        public void RenderLevelInEditor(int levelnum)
        {
            //TO DO: figure out why we have to get component and cant do it in awake. Probably because it's in awake
            renderMap = GetComponent<RenderMap>();
            objectManager = GetComponent<ObjectManager>();
            if (levelnum + 1 > mapDataObject.mapData.numberOfLevels)
            {
                Debug.Log("Attempting to load level but level does not exist!");
                return;
            }
            //Clear current obstacles
            objectManager.SearchAndDestroy();
            //Load tiles and obstacles
            renderMap.LoadTiles(mapDataObject.mapData.levels[levelnum], tileSet, GameArea);
            objectManager.CreateObstacles(mapDataObject.mapData.levels[levelnum].obstacles, obstacleSet);
            objectManager.CreateEventTriggers(mapDataObject.mapData.levels[levelnum].objects, eventSet);
            //Set obstacles and objects as children of their respective containers
            objectManager.SetParents(obstacleContainer.GetComponent<Transform>(), objectContainer.GetComponent<Transform>());
            //Set our start and end points
            SetPoints(levelnum);

        }

        //Set the start and end points
        private void SetPoints(int level)
        {
            spawnPoint.transform.position = mapDataObject.mapData.levels[level].startPoint;
            finishPoint.transform.position = mapDataObject.mapData.levels[level].endPoint;
        }

        #endregion
    }
}
