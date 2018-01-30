using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;
using UnityEngine.Tilemaps;
using System;
using System.IO;

//Class to save current tiles within a game area on screen to a map. 
//Works entirely in the Editor
//Supports saving multiple levels to a single map

[ExecuteInEditMode]
public class SaveMapInEditor : MonoBehaviour
{
    public int levelNumber;                                                             //Current Level we are working on                           
    public string mapName;                                                              //Name of the map to save
    public TileSet tileSet;                                                             //Tileset to store our Scriptable Tiles. Lets us also store the map tiles as ints to save space
    public Tilemap groundLayer;                                                         //Ground layer for tiles. Has no collision in the layer
    public Tilemap wallLayer;                                                           //Wall Layer for tiles. Has collisions. Check Tile Classes if collisions aren't happening
    public BoundsInt GameArea;                                                          //Sets the bounds for our game area and where we save from


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

        //Instantiates a new LevelData class and saves our tiles into the appropriate layers
        LevelData currLevel = new LevelData(SaveLevelTiles(groundLayer), SaveLevelTiles(wallLayer));

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
        MapFile newMap = new MapFile(tileSet, levelList);

        //Gets a path to save our map to
        string path = "Assets/Maps/" + mapName + ".bound";

        //Serializes the mapfile into a string
        string mapData = JsonUtility.ToJson(newMap);

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
}
