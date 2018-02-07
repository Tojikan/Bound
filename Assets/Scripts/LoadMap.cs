using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;
using BoundMaps;

//Loads tiles from a file
//Takes a file, deserializes it, and then sets the tiles
//Runs in PlayMode

public class LoadMap : MonoBehaviour
{
    public Tilemap groundTiles;                                     //Layer for our ground tiles
    public Tilemap wallTiles;                                       //Layer for our wall tiles
    public GameObject player;                                       //Change position of player
    public GameObject spawnPoint;                                   //Lazy way to get a reference to our start point
    public GameObject finishPoint;                                  //Lazy way to get a reference to our finish point
    public BoundsInt GameArea;                                      //Bounding area for our game. Only loads within this area. MATCH IT TO OUR SAVEMAPEDITOR or else it might get buggy

    [HideInInspector]
    public string FileToLoad;                                       //Path to our designated map. Can be manipulated by the LoadMap EditorScript

    [HideInInspector]
    public int LevelToLoad;                                         //Sets which level we want to load. Can be maniuplated by the LoadMap Editor Script

    private TileSet mapTileset;                                     //Tileset to load tiles from. Set by our map file
    private MapFile map;                                            //The map file we're loading


    //Used for the initial setup of a level. Loads a map from file. 
    public void SetUpLevel()
    {
        //Checks to see if we have a file to load
        if (FileToLoad == "" || FileToLoad == null)
        {
            Debug.Log("No Map Selected");
            return;
        }

        //Loads the map information from designated map file into a string
        string loadMap = ReadString(FileToLoad);

        //Deserialize the mapfile string into a mapfile class
        map = JsonUtility.FromJson<MapFile>(loadMap);


        //Sets our tileset path by getting the info from the mapfile and setting a path
        Object temp = Resources.Load("Tiles/" + map.tileset);
        mapTileset = (TileSet)temp;

        //Clears any current tiles on the game board
        groundTiles.ClearAllTiles();
        wallTiles.ClearAllTiles();

        //If selected level doesn't exist, then we just start from the beginning
        if (LevelToLoad > map.numberOfLevels - 1 || LevelToLoad < 0)
        {
            LevelToLoad = 0;
            Debug.Log("Level does not exist! Set to 0");
        }
        //Load a level
        LoadLevel(0);
    }

    //Gets the end level from the map we've loaded
    public int GetEndLevel()
    {
        return map.numberOfLevels;
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

    //Method to load our level. Takes in an parameter to determine which level to load
    public void LoadLevel(int levelnum)
    {
        //Check if we have the tile layers set
        if (groundTiles == null || wallTiles == null)
        {
            Debug.Log("Error: Must set tile layers!");
            return;
        }

        //Check if our tileset is set
        if (mapTileset == null)
        {
            Debug.Log("Error: No Tile Set Found!!");
            return;
        }

        if (levelnum > map.numberOfLevels - 1)
        {
            levelnum = 0;
            Debug.Log("Level does not exist. Loading first level");
        }

        //Check we have a start or endpoint game object
        if (!spawnPoint || !finishPoint)
        {
            Debug.Log("No Spawn or Finish object!!");
        }
        else
        {
            spawnPoint.transform.position = map.levels[levelnum].startPoint;
            finishPoint.transform.position = map.levels[levelnum].endPoint;
        }

        //Creates arrays for our layers with size equal to our game board
        TileBase[] groundArray = new TileBase[GameArea.size.x * GameArea.size.y * GameArea.size.z];
        TileBase[] wallArray = new TileBase[GameArea.size.x * GameArea.size.y * GameArea.size.z];

        //iterates over the ground array and populates it with tilebases from our file
        for (int index = 0; index < groundArray.Length; index++)
        {
            
            //If the value is -1, then it means there was no tile in that position
            if (map.levels[levelnum].groundTiles[index] != -1)
            {
                //Sets our ground tile array by matching the value stored in the tile data of the current level with our tileset's array
                groundArray[index] = mapTileset.tilesArray[map.levels[levelnum].groundTiles[index]];
            }
        }

        //iterates over the wall array and populates it with tilebases from our file
        for (int index = 0; index < wallArray.Length; index++)
        {
            //Checks if there is a tile. If it's -1, then no tile
            if (map.levels[levelnum].wallTiles[index] != -1)
            {
                //Sets our wall tile array by matching the value stored in the tile data of the current level with our tileset's array
                wallArray[index] = mapTileset.tilesArray[map.levels[levelnum].wallTiles[index]];
            }
        }

        //Sets the tiles on the map layer by layer
        groundTiles.SetTilesBlock(GameArea, groundArray);
        wallTiles.SetTilesBlock(GameArea, wallArray);

        //Move our player to the spawn point
        player.transform.position = spawnPoint.transform.position;

        Debug.Log("Loaded map and level: " + levelnum);
        return;
    }

}

