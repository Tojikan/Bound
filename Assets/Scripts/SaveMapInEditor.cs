using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using BoundMaps;

//Class to save current tiles within a game area on screen to a map. 
//Works entirely in the Editor
//Supports saving multiple levels to a single map

[ExecuteInEditMode]
public class SaveMapInEditor : MonoBehaviour
{
    public int levelNumber;                                                             //Current Level we are working on                           
    public string mapName;                                                              //Name of the map to save
    public TileSet tileSet;                                                             //Tileset to store our Scriptable Tiles. Lets us also store the map tiles as ints to save space
    public ExplosionSet explosionSet;                                                   //Explosion set to store our explosion types

    public Tilemap groundLayer;                                                         //Ground layer for tiles. Has no collision in the layer
    public Tilemap wallLayer;                                                           //Wall Layer for tiles. Has collisions. Check Tile Classes if collisions aren't happening

    public GameObject explosionContainer;                                               //Container to save explosions. Saves all the children class of it. 
    public GameObject spawnPoint;                                                       //Lazy way of getting a reference to our start point
    public GameObject finishPoint;                                                      //Lazy way of getting a reference to our end point
    public BoundsInt GameArea;                                                          //Sets the bounds for our game area and where we save from
    public Transform containerTransform;                                                //Gets a reference to our container's transform.



    [HideInInspector]
    public string FileToLoad;                                                           //Path to our a map to load if we're editing a file. 

    private LoadMap LoadScript;
    private List<LevelData> levelList = new List<LevelData>();                          //Private variable to store the list of all levels in the map
   




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

        //Instantiates a new LevelData class and saves our tiles into the appropriate layers
        LevelData currLevel = new LevelData(SaveLevelTiles(groundLayer), SaveLevelTiles(wallLayer), start, end, SaveExplosionData());

        //Saves our level into the list. If you don't start the first level at 0 or if you try to skip over an uncreated level number, it'll cause an error. 
        try
        {
            levelList.Insert(levelNumber, currLevel);
            Debug.Log("Level Save Successful!");
        }
        catch
        {
            Debug.Log("Level Save Unsuccessful. Did you skip a level?");
        }
    }


    //Lazy bit of code as it's pretty much just copy pasted but it works so whatever
    //Overwrites the current level we're on
    //TO DO - Incorporate it into one method. Basically we need to figure out a way to check if the list element exists or not. 
    public void OverwriteLevel()
    {
        //Check our levels aren't negative
        if (levelNumber < 0)
        {
            Debug.Log("Error: Invalid level");
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

        //Instantiates a new LevelData class and saves our tiles into the appropriate layers
        LevelData currLevel = new LevelData(SaveLevelTiles(groundLayer), SaveLevelTiles(wallLayer), start, end, SaveExplosionData());

        // 
        try
        {
            levelList[levelNumber] = currLevel;
            Debug.Log("Level Overwrite Successful!");
        }
        catch
        {
            Debug.Log("Not able to overwrite. Does the level currently exist?");
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

    //Saves all of our Explosion Prefabs that are under our obstacle container
    public List<ExplosionData> SaveExplosionData()
    {
        //Get component reference to our container's transform
        containerTransform = explosionContainer.GetComponent<Transform>();

        //Temp list to hold our data
        List<ExplosionData> bombArray = new List<ExplosionData>();

        //Iterates over each child within our container
        foreach (Transform child in containerTransform)
        {
            Exploder childData = child.GetComponent<Exploder>();
            //Matches the explosiontype to our array and gets an int
            int type = Array.IndexOf(explosionSet.ExplosionPrefabs, childData.explosionType);
            //Creates new Explosion Data and initializes it
            ExplosionData newData = new ExplosionData(child.transform.position, childData.loopLength, childData.countdown, type);
            //Adds it to our temp list
            bombArray.Add(newData);
        }

        //returns list
        return bombArray;
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
        catch (Exception e)
        {
            Console.WriteLine("An error occured while saving the map: '{0}'", e);
            Debug.Log("Unable to save map");
        }
   
    }

    //Method to clear the current level list. 
    public void ClearLevels()
    {
        levelList.Clear();
    }

    

    //Method to let us load the level from memory into the editor
    //Essentially like loading a map except from memory instead of a file. 
    //Parameter tells us which level we're loading
    public void LoadCurrentLevel(int levelnum)
    {
        //First check if exists
        if (levelList.ElementAtOrDefault(levelnum) == null)
        {
            Debug.Log("Level does not exist!");
            return;
        }

        //Check we have a start or endpoint game object
        if (!spawnPoint || !finishPoint)
        {
            Debug.Log("No Spawn or Finish object!!");
        }
        else
        {
            spawnPoint.transform.position = levelList[levelnum].startPoint;
            finishPoint.transform.position = levelList[levelnum].endPoint;
        }

        //Clear our editor game board
        groundLayer.ClearAllTiles();
        wallLayer.ClearAllTiles();

        //Creates arrays for our layers with size equal to our game board
        TileBase[] groundArray = new TileBase[GameArea.size.x * GameArea.size.y];
        TileBase[] wallArray = new TileBase[GameArea.size.x * GameArea.size.y];

        //iterates over the ground array and populates it with tilebases from our file
        for (int index = 0; index < groundArray.Length; index++)
        {
            //If the value is -1, then it means there was no tile in that position
            if (levelList[levelnum].groundTiles[index] != -1)
            {
                //Sets our ground tile array by matching the value stored in the tile data of the current level with our tileset's array
                groundArray[index] = tileSet.tilesArray[levelList[levelnum].groundTiles[index]];
            }
        }

        //iterates over the wall array and populates it with tilebases from our file
        for (int index = 0; index < wallArray.Length; index++)
        {
            //Checks if there is a tile. If it's -1, then no tile
            if (levelList[levelnum].wallTiles[index] != -1)
            {
                //Sets our wall tile array by matching the value stored in the tile data of the current level with our tileset's array
                wallArray[index] = tileSet.tilesArray[levelList[levelnum].wallTiles[index]];
            }
        }

        //Sets the tiles on the map layer by layer
        groundLayer.SetTilesBlock(GameArea, groundArray);
        wallLayer.SetTilesBlock(GameArea, wallArray);


        Debug.Log("Loaded level: " + levelnum);

    }

    //Lets us load a map file into the editor. Basically ripped from our load level script
    public void LoadMapInEditor()
    {
        //Check if we have a file
        if (FileToLoad == "" || FileToLoad == null)
        {
            Debug.Log("No Map Found");
            return;
        }

        //Get reference to our loadscript so we can call a function. I'm not sure if we can put this elsewhere
        LoadScript = GetComponent<LoadMap>();

        //Reads our file
        string loadMap = LoadScript.ReadString(FileToLoad);

        //Converts back to a map file
        MapFile map = JsonUtility.FromJson<MapFile>(loadMap);

        //Sets our level list from the mapfile. 
        levelList = map.levels;
        Debug.Log("Map Loaded");
    }
}
