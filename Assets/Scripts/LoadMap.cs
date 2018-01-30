using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;
using BoundMaps;

//Loads a map from a given file
//Loads a struct file saved into a text via json, then uses it to populate the map tiler

//TO DO - CLEAN UP. Figure out a better way for loading the map path
//A button to click to make it work rather than on start?
//How to load multiple sections? One Big array or multiple structs in a file
public class LoadMap : MonoBehaviour
{
    public Tilemap tilemap;                         //tilemap to load from
    public BoundsInt area;                          //Area of the gamemap
    public string FileToLoad;                       //name of file

    private TileSet tileSet;                        //tileSet to load tiles from
    private string tileSetPath;                     //Path of tileset

    void Start()
    {
        //Sets our map path
        string mapPath = "Assets/Maps/" + FileToLoad;

        //Loads the map information from a file into a string
        string loadMap = ReadString();

        //Deserialize the mapfile string into a new mapfile struct
        MapFile newLevel = JsonUtility.FromJson<MapFile>(loadMap);

        //Sets our tileset path by getting the info from the mapfile
        tileSetPath = newLevel.tilesetPath;
        tileSet = AssetDatabase.LoadAssetAtPath<TileSet>(tileSetPath);

        //Creates a new array with size equal to our game board
        TileBase[] newLevelTileArray = new TileBase[area.size.x * area.size.y * area.size.z];

        //Sets a new tile into the tile array by reading from the mapfile to get the index to our tileset
        for (int index = 0; index < newLevel.levelone.Length; index++)
        {
            newLevelTileArray[index] = tileSet.tilesArray[newLevel.levelone[index]];
        }

        //Sets the tiles on the map
        tilemap.SetTilesBlock(area, newLevelTileArray);
    }

    //Method to read from a file
    string ReadString()
    {
        //Path of the file
        string path = "Assets/Maps/" + FileToLoad;

        //Open up a stream to the file
        StreamReader reader = new StreamReader(path);

        //Reads it into a string
        string mapInfo = reader.ReadToEnd();

        //Remember to close the stream
        reader.Close();

        return mapInfo;
    }
}
