//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.Tilemaps;
//using BoundMaps;

////Functionality to save a map to a file
////Gets the current tiles, saves it to an array as an int, then writes to a file

////TO DO
////CLEAN UP
////FIGURE OUT A WAY TO RUN IT FROM WITHIN THE EDITOR AS OPPOSED TO ON START
////BETTER WAY TO MANAGE FILENAMES
//public class SaveMap : MonoBehaviour
//{
//    private TileSet tileSet;                    //our tileset that we are setting tiles from

//    public Tilemap tilemap;                     //tilemap we are creating tiles on
//    public BoundsInt GameArea;                  //The bounding space of our game area
//    public string MapName;                      //Name of the map 
    
//	void Start ()
//    {
//        //Set our tileset path and then load the object
//        string tilesetPath = "Assets/Tiles/TestSet.asset";
//	    tileSet = AssetDatabase.LoadAssetAtPath<TileSet>(tilesetPath);

//        //Reads the current gameboard into a new array
//        int[] temp = GetTileInfo();

//        //Creates a new mapfile and stores our tileSet for the current map and the int array
//        MapFile saveMap = new MapFile(tilesetPath, temp);

//        //Serialize the map file as a string
//        string newMap = JsonUtility.ToJson(saveMap);
    
//        //Writes it into a file
//        WriteFile(newMap);

//    }


//    //Method to write our file. Accepts a string parameter which should be our serialized mapfile
//    void WriteFile(string writeThis)
//    {
//        //Gets the path of the map file to write
//        string path = "Assets/Maps/" + MapName;

//        //Opens up a stream, then writes our parameter into it
//        StreamWriter writer = new StreamWriter(path, false);
//        writer.WriteLine(writeThis);

//        //Close the writer
//        writer.Close();
//    }
	
//    //Method to load our current map into an array
//    private int[] GetTileInfo()
//    {
//        //Declare new array based on game area size
//        int[] tileArray = new int[GameArea.size.x * GameArea.size.y * GameArea.size.z];

//        //counter variable for our array storage
//        int index = 0;

//        //Nested for loop to iterate over each tile in our tilemap
//        for(int column = GameArea.position.y; column < (GameArea.position.y + GameArea.size.y); column++ )
//        {
//            for (int row = GameArea.position.x; row < (GameArea.position.x + GameArea.size.x); row++)
//            {
//                //Gets the current tile and stores it
//                TileBase currentTile = tilemap.GetTile(new Vector3Int(row, column, GameArea.position.z));

//                //Gets an int value for that tile by searching it against the tileset array
//                int tileValue = Array.IndexOf(tileSet.tilesArray, currentTile);
                
//                //Store the int into the tile array
//                tileArray[index] = tileValue;

//                //Increment the index for the next tile
//                index++;
//            }
//        }
//        return tileArray;
//    }


//}
