using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



//Scriptable Object class to create a new Tileset. Holds an array of our scriptable tiles for use in saving/loading maps
//Create a new object with the Asset menu -> create -> Tiles

[CreateAssetMenu(fileName = "NewTileSet", menuName = "Tiles/New TileSet", order = 2)]
public class TileSet : ScriptableObject
{
    public string tilesetName = "New TileSet";
    public TileBase[] tilesArray;
    
}
