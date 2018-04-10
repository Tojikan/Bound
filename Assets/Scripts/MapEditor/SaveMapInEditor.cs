
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


#if UNITY_EDITOR
/**Prototype Map Editor
 * 
 * Class to save current tiles and map objects within a game area on screen to a map. Currently only works in the editor 
 * Works by calling various save functions to save the components - tiles, obstacles, eventtriggers. This is saved to a MapFile in Memory. 
 * Multiple levels can be added on to this Mapfile. Simply enter the number of the level in the editor window and click on save level to add a new level/overwrite existing level
 * All current data in mapfile memory WILL be erased upon entering play mode. Must click on Map Save BEFORE entering playmode to save it into a file.
 * All of this is set manually currently - lots of fixing and refining to do for later
 * There is limited functionality to test and load maps. Able to load a specific level's tiles into the editor and able to load a map file into memory for editing
 * All obstacles must be child of the container object in order to save. Same thing with EventTriggers.
 * Obstacles are handled a little differently as a prototype for the future - they are saved into a scriptable object and then the object is saved into a mapfile. You can load the obstacles from the scriptable
 * object only. So there seems to be a bit of a problem with reloading obstacles from older levels. They also don't load back as a child of the container

**/
//TO DO: IMPLEMENT A CLEANER WAY OF LOADING/EDITING PREVIOUS MAPS. WILL NEED TO INCORPORATE NEW KNOWLEDGE OF SCRIPTABLE OBJECTS FOR LOADING MAPS
namespace BoundEditor
{
    [ExecuteInEditMode]
    public class SaveMapInEditor : MonoBehaviour
    {
        public string mapName;                                                              //Name of map to save
        [TextArea(2,5)]
        public string mapDescrip;                                                           //Map description text box
        public Sprite mapImage;                                                             //Map Image. Make sure you only drag from the resources folder or else it won't work
        public int levelNumber;                                                             //Current Level we are working on
        public string levelName;                                                            //name of the level

        public TileSet tileSet;                                                             //Tileset scriptable object which contains an array holding references to our scriptable tiles. Lets us store the map tiles as ints to save space
        public ObstacleSet explosionSet;                                                    //Explosion set to store references to our exploder prefabs
        public EventTriggerSet eventSet;                                                    //Drag desired eventtrigger set here
        public Tilemap groundLayer;                                                         //Ground layer for tiles. Use for ground level tiles
        public Tilemap wallLayer;                                                           //Wall Layer for tiles. Use for any higher level tiles
        public MapObjectsData mapObjectsData;                                               //Scriptable Object that contains all of our map object data for obstacles and eventtriggers
        public GameObject explosionContainer;                                               //Container to save obstacles. Saves all the children objects of it. 
        public GameObject objectContainer;                                                  //Container to save map objects. Saves all children objects.
        public GameObject spawnPoint;                                                       //Drag any game object to this in the editor window to set our spawnpoint
        public GameObject finishPoint;                                                      //Drag any game object to this in the editor window to set our finish point
        public GameObject mapInfo;                                                          //Drag reference to the game object that contains mapmetainfo and dialogue object here
        public BoundsInt GameArea;                                                          //Sets the bounds for our game area and where we save from
        public Transform containerTransform;                                                //Drag the Game Object to this in the editor window. This sets the transform under which we save all our obstacles from

        [HideInInspector] public string FileToLoad;                                         //Path to our a map to load if we're editing a file. 

        private string savePath;
        private int music;                                                                  //stores which song for the level
        private MapLoader mapLoader;                                                        //mapLoader component so we can load a level/map without having to go into play mode
        private RenderMap renderMap;                                                        //rendermap component to render the map
        private MapObjectManager obstacleManager;                                           //Obstacle Manager component so we can reload obstacles from previous levels
        private List<LevelData> levelList = new List<LevelData>();                          //List variable that holds a list of all of our levels
   

        //enum to select music
        public enum LevelMusic
        {
            Ending, Level1, Level2, Level3, TitleScreen, NewDayNewDawn, MyWay, NeverBackDown, WeStandTogether, RiseAgain, UnderSiege, InTheNameOfScience 
        }
        //Set music
        public LevelMusic selectMusic;

        private void Awake()
        {
            //Get the reference to all of our components
            mapLoader = GetComponent<MapLoader>();
            renderMap = GetComponent<RenderMap>();
            obstacleManager = GetComponent<MapObjectManager>();
        }

        #region Level/Map save
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
            SaveEventTriggers();

            //Get the start and end dialogues
            Dialogue startDialogue = SaveDialogue(true);
            Dialogue endDialogue = SaveDialogue(false);

            //set music variable
            music = (int)selectMusic;

            //Instantiates a new LevelData class and saves our tiles into the appropriate layers
            //Check if we have level music indicated

            LevelData currLevel = new LevelData(SaveLevelTiles(groundLayer), SaveLevelTiles(wallLayer), start, end, mapObjectsData.obstacleData, 
                                                music, startDialogue, endDialogue, levelName, mapObjectsData.eventData);
            
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
            MapFile newMap = new MapFile(tileSet.name.ToString(), explosionSet.name.ToString(), levelList, eventSet.name.ToString());

            //Gets and creates a new map directory
            string newDirectory = GetNewDirectory(mapName);

            //Gets a path to save our map to
            string mapPath = newDirectory + mapName + ".bound";

            //Save the map meta
            SaveMapMeta(newDirectory, mapPath);

            //Serializes the mapfile into a string
            string mapData = EditorJsonUtility.ToJson(newMap);

            //Tries to write our data into the path. If unable to write for whatever reason, we just get a debug
            try
            {
                FileWriter(mapPath, mapData);
                Debug.Log("SaveMap successful!");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Unable to save map");
            }

        }

        //Generates a map meta file from the info set in the editor window. Accepts a parameter of the subdirectory the map will be located and the path of the .bound file itself
        private void SaveMapMeta(string directory, string path)
        {
            //Sets the data
            MapMetaObject newMeta = new MapMetaObject(mapName, path, mapDescrip, mapImage.name);

            //Converts to string
            string metaInfo = EditorJsonUtility.ToJson(newMeta);

            //Appends the meta.json at the end for saving
            directory = directory + "meta.json";
            try
            {
                FileWriter(directory, metaInfo);
                Debug.Log("Successfully saved meta to " + directory);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("Unable to save meta information");
            }         
        }

        //Creates a new folder given a map name and returns the path of that folder
        private string GetNewDirectory(string mapName)
        {
            string path = savePath + mapName + "/";
            Directory.CreateDirectory(path);
            return path;
        }
        #endregion

        #region Component Save Functions

        //Saves the dialogue from the dialogueobject component. Accepts bool parameter - true to return the start dialogue or false to return the end dialogue.
        private Dialogue SaveDialogue(bool startDial)
        {
            DialogueObject component = mapInfo.GetComponent<DialogueObject>();
            if (startDial)
                return component.startDialogue;
            else
                return component.endDialogue;
        }

        //Method to save all EventTriggers into a list
        private void SaveEventTriggers()
        {
            //get container transform
            Transform objTransform = objectContainer.GetComponent<Transform>();
            //temp list variable
            List<EventTriggerData> objList = new List<EventTriggerData>();
            //iterate over each child object
            foreach (Transform child in objTransform)
            {
                EventTrigger childData = child.gameObject.GetComponent<EventTrigger>();
                //Get an int by referencing child's sprite with the eventset
                int type = Array.IndexOf(eventSet.sprites, child.GetComponent<SpriteRenderer>().sprite);
                //get the start state
                bool state = childData.startsEnabled;
                //Create new event trigger data and add it to the list
                EventTriggerData newObjectData = new EventTriggerData(child.transform.position, type, state);
                objList.Add(newObjectData);
            }
            Debug.Log("Map Items saved!");
            mapObjectsData.eventData = objList;
        }


        //Method to save our tiles into an array
        private int[] SaveLevelTiles(Tilemap tilemap)
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
            List<ObstacleData> bombArray = new List<ObstacleData>();

            //Iterates over each child within our container
            foreach (Transform child in containerTransform)
            {
                //Get a reference to the obstacle
                Obstacle childData = child.GetComponent<Obstacle>();

                //Matches the sprite to the sprite array and gets the index
                //make sure the sprite matches the sprite in the obstacleset sprite array
                int type = Array.IndexOf(explosionSet.obstacleSprites, child.GetComponent<SpriteRenderer>().sprite);

                //Creates new Explosion Data and initializes it
                ObstacleData newData = new ObstacleData(child.transform.position, childData.loopLength, childData.triggerTime, type, (int)childData.SelectSFXPlayer);

                //Adds it to our temp list
                bombArray.Add(newData);
            }

            //Sets our scriptable object to the bomb array to save it
            mapObjectsData.obstacleData = bombArray;
            Debug.Log("Exploders Saved!");
        }

        #endregion


        #region Other functionalities

        //Writes info to a given path
        private void FileWriter(string path, string info)
        {
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(info);
            writer.Close();
        }            
        
        //Method to clear the current level list. 
        public void ClearLevels()
        {
            levelList.Clear();
        }

        //Method to update our scene with the exploders that are stored in the exploder data object;
        public void UpdateExploders()
        {
            obstacleManager.CreateObstacles(mapObjectsData.obstacleData, explosionSet);
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
        #endregion
    }
}
#endif
