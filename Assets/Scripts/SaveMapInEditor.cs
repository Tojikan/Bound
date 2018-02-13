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

//Class to save current tiles within a game area on screen to a map. 
//Works entirely in the Editor
//Supports saving multiple levels to a single map


namespace BoundEditor
{
    [ExecuteInEditMode]
    public class SaveMapInEditor : MonoBehaviour
    {
        public int levelNumber;                                                             //Current Level we are working on                           
        public string mapName;                                                              //Name of the map to save
        public TileSet tileSet;                                                             //Tileset scriptable object which contains an array holding references to our scriptable tiles. Lets us store the map tiles as ints to save space
        public ExplosionSet explosionSet;                                                   //Explosion set to store references to our exploder prefabs
        public Tilemap groundLayer;                                                         //Ground layer for tiles. Use for ground level tiles
        public Tilemap wallLayer;                                                           //Wall Layer for tiles. Use for any higher level tiles
        public ExploderDataObject exploderData;                                             //Scriptable Object that contains all of our exploder data
        public GameObject explosionContainer;                                               //Container to save explosions. Saves all the children class of it. 
        public GameObject spawnPoint;                                                       //Drag any game object to this in the editor window to set our spawnpoint
        public GameObject finishPoint;                                                      //Drag any game object to this in the editor window to set our finish point
        public BoundsInt GameArea;                                                          //Sets the bounds for our game area and where we save from
        public Transform containerTransform;                                                //Drag the Game Object to this in the editor window. This sets the transform under which we save all our explosions from



        [HideInInspector]
        public string FileToLoad;                                                           //Path to our a map to load if we're editing a file. 

        private MapLoader mapLoader;                                                        //mapLoader component so we can load a level/map without having to go into play mode
        private RenderMap renderMap;                                                        //rendermap component to render the map
        private ObstacleManager obstacleManager;                                            //Obstacle Manager component so we can reload obstacles from previous levels
        private List<LevelData> levelList = new List<LevelData>();                          //List variable that holds a list of all of our levels


        private void Awake()
        {
            //Get the reference to all of our components
            mapLoader = GetComponent<MapLoader>();
            renderMap = GetComponent<RenderMap>();
            obstacleManager = GetComponent<ObstacleManager>();
        }


        //Saves the current level we're working on. Note it doesn't write to the file, only stores the tiles into an array
        public void SaveLevel()
        {
            //Check our levels aren't negative
            if (levelNumber < 0)
            {
                Debug.Log("Error: Invalid level");
                return;
            }

            //Check we have a mapname
            if (mapName == "")
            {
                Debug.Log("Error: Invalid Map Name");
                return;
            }

            //Check that we set our layers and tile set
            if (tileSet == null || groundLayer == null || wallLayer == null)
            {
                Debug.Log("Error: Must set all Inspector options");
                return;
            }

            //Check we have a reference to our spawn/finish
            if (!spawnPoint || !finishPoint)
            {
                Debug.Log("No Start or Finish Point reference!!!");
                return;
            }

            //get the position of our start/finish points
            Vector2 end = finishPoint.transform.position;
            Vector2 start = spawnPoint.transform.position;
            
            //Save our exploders into our level object
            SaveExplosionData();

            //Instantiates a new LevelData class and saves our tiles into the appropriate layers
            LevelData currLevel = new LevelData(SaveLevelTiles(groundLayer), SaveLevelTiles(wallLayer), start, end, exploderData.data);

            
            try
            {
                //checks if level exists. If not, insert new.
                if (levelNumber + 1 > levelList.Count)
                {
                    //Saves our level into the list. If you don't start the first level at 0 or if you try to skip over an uncreated level number, it'll cause an error. 
                    levelList.Insert(levelNumber, currLevel);
                    Debug.Log("Successfully added a new level!");
                }
                else 
                {
                    //Overwrites current level
                    levelList[levelNumber] = currLevel;
                    Debug.Log("Level Overwrite Successful!");
                }
       
            }
            catch
            {
                Debug.Log("Level Save Unsuccessful. Did you skip a level?");
            }
        }


        //Method to save our tiles into an array
        public int[] SaveLevelTiles(Tilemap tilemap)
        {
            //Declare new array based on game area size
            int[] tileArray = new int[GameArea.size.x * GameArea.size.y];

            //counter variable for our array storage
            int index = 0;

            //Nested for loop to iterate over each tile in our tilemap
            for (int column = GameArea.position.y; column < (GameArea.position.y + GameArea.size.y); column++)
            {
                for (int row = GameArea.position.x; row < (GameArea.position.x + GameArea.size.x); row++)
                {
                    //Gets the current tile and stores it
                    TileBase currentTile = tilemap.GetTile(new Vector3Int(row, column, 0));

                    //Gets an int value for that tile by searching it against the tileset array
                    int tileValue = Array.IndexOf(tileSet.tilesArray, currentTile);

                    //Store the int into the tile array
                    tileArray[index] = tileValue;

                    //Increment the index for the next tile
                    index++;
                }
            }

            return tileArray;

        }

        //Saves all of our Explosion Prefabs that are under our obstacle container into our scriptable object
        public void SaveExplosionData()
        {
            //Get component reference to our container's transform
            containerTransform = explosionContainer.GetComponent<Transform>();

            //Temp list to hold our data
            List<ExplosionData> bombArray = new List<ExplosionData>();

            //Iterates over each child within our container
            foreach (Transform child in containerTransform)
            {
                //Get a reference to the exploder script
                ExploderObstacle childData = child.GetComponent<ExploderObstacle>();


                //Matches the explosiontype to our array and gets an int
                //Currently Deprecated as exploders have changed. Currently we have to type it in
                //TO DO: Fix it somehow 
                //int type = Array.IndexOf(explosionSet.ExplosionPrefabs, childData.exploderPrefabIndex);
                //Debug.Log(type);
                //Debug.Log(childData.exploderPrefabIndex + " vs " + explosionSet.ExplosionPrefabs[0]);

                //Creates new Explosion Data and initializes it
                ExplosionData newData = new ExplosionData(child.transform.position, childData.loopLength, childData.countdown, childData.exploderPrefabIndex);

                //Adds it to our temp list
                bombArray.Add(newData);
            }

            //Sets our scriptable object to the bomb array to save it
            exploderData.data = bombArray;
            Debug.Log("Exploders Saved!");
        }

        //Method to save map
        public void SaveMap()
        {
            //Checks to make sure we have levels in the list
            if (levelList.Count == 0)
            {
                Debug.Log("No levels found");
                return;
            }

            //Create a mapfile and pass in the tileset and our level list
            //Must cast the tileSet name into a string. We call the tileSet by path because JsonUtility only gives us an instance ID which may not work
            MapFile newMap = new MapFile(tileSet.name.ToString(), explosionSet.name.ToString(), levelList);

            //Gets a path to save our map to
            string path = "Assets/Maps/" + mapName + ".bound";

            //Serializes the mapfile into a string
            string mapData = EditorJsonUtility.ToJson(newMap);

            //Tries to write our data into the path. If unable to write for whatever reason, we just get a debug
            try
            {
                StreamWriter writer = new StreamWriter(path, false);
                writer.WriteLine(mapData);
                writer.Close();
                Debug.Log("SaveMap successful!");
            }
            catch 
            {
                Debug.Log("Unable to save map");
            }

        }

        //Method to clear the current level list. 
        public void ClearLevels()
        {
            levelList.Clear();
        }

        //Method to update our scene with the exploders that are stored in the exploder data object;
        public void UpdateExploders()
        {
            obstacleManager.CreateExploders(exploderData.data, explosionSet);
        }


        //Method to let us load the level from memory into the editor
        public void LoadCurrentLevel(int levelnum)
        {
            renderMap.LoadTiles(levelList[levelnum], tileSet, GameArea);
            Debug.Log("Loaded level: " + levelnum);

        }

        //Lets us load a map file into the editor. 
        public void LoadMapInEditor()
        {
            
            MapFile loadedMap = mapLoader.LoadMap(FileToLoad);
            levelList = loadedMap.levels;
            Debug.Log("Map Loaded");
        }
    }
}
