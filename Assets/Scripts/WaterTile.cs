using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


//Class for setting a new Water Tile


public class WaterTile : Tile
{

    //Adds a field to the watertile asset
    [SerializeField]
    private Sprite[] waterSprites;

    [SerializeField]
    private Sprite preview;

    //Sets the size of our tile array
    const int tileNumber = 48;

    //Overrides refresh tile to check if every tile around a tile is a water tile or not. Then refreshes.
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);

                if (CheckTile(tilemap, nPos))
                {
                    tilemap.RefreshTile(nPos);
                }
            }
        }
    }

    //Changes the tile accordingly after being refreshed
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //Does this so we can have colliders working
        base.GetTileData(position, tilemap, ref tileData);

        //bitmask integer
        int bitmask = 0;

        //Index for which sprite to display. Default to 0.
        int tileNum = 0;

        //Vector3Ints for cardinal directions
        Vector3Int eastSide = new Vector3Int(position.x + 1, position.y, position.z);
        Vector3Int southSide = new Vector3Int(position.x, position.y - 1, position.z);
        Vector3Int westSide = new Vector3Int(position.x - 1, position.y, position.z);
        Vector3Int northSide = new Vector3Int(position.x, position.y + 1, position.z);

        //Corner direction positions
        Vector3Int northWestSide = new Vector3Int(position.x - 1, position.y + 1, position.z);
        Vector3Int northEastSide = new Vector3Int(position.x + 1, position.y + 1, position.z);
        Vector3Int southWestSide = new Vector3Int(position.x - 1, position.y - 1, position.z);
        Vector3Int southEastSide = new Vector3Int(position.x + 1, position.y - 1, position.z);



        //Calculate our bitmask accordingly. From Top Left to Bottom right around our tile, its 1-2-4-8-16-32-64-128
        //Uses checkcorners for any corner tiles. Uses GetBitmask for cardinal directions
        bitmask = (CheckCorners(tilemap, northWestSide, northSide, westSide) * 1)
                + (GetBitmask(tilemap, northSide) * 2)
                + (CheckCorners(tilemap, northEastSide, northSide, eastSide) * 4)
                + (GetBitmask(tilemap, westSide) * 8)
                + (GetBitmask(tilemap, eastSide) * 16)
                + (CheckCorners(tilemap, southWestSide, westSide, southSide) * 32)
                + (GetBitmask(tilemap, southSide) * 64)
                + (CheckCorners(tilemap, southEastSide, eastSide, southSide) * 128);

        //Checks if surrounded at all sides
        if (bitmask == 255)
        {
            //Randomize between 3 plain tiles with different decorations
            int randomVal = UnityEngine.Random.Range(0, 100);
            int result = 47;
            if (randomVal <= 15)
                result = 46;
            if (randomVal >= 75)
                result = 48;
            tileData.sprite = waterSprites[result];
            return;
        }

        //Gets the index from our values array of the bitmask, and sets our tile sprite
        int tempNum = Array.IndexOf(valuesArray, bitmask);

        if (tempNum != -1)
        {
            tileNum = tempNum;
        }
        
        tileData.sprite = waterSprites[tileNum];
        
    }

    //Array to store bitmask values. Match the index number to the index in the sprite array. Make sure new tilesets are ordered in the same way
    private int[] valuesArray = new int[] 
    {
    0, 2, 8, 10, 11, 16, 18, 22, 24, 26, 27, 30,
    31, 64, 66, 72, 74, 75, 80, 82, 86, 88, 90, 91, 94,
    95, 104, 106, 107, 120, 122, 123, 126 ,127, 208, 210,
    214, 216, 218, 219, 222, 223, 248, 250, 251, 254, 255
    };


    //Checks if water tile
    private bool CheckTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    //For when we calculate our bitmap. If the tile is the same tile, returns 1 and adds to our bitmap. Otherwise returns a 0 so it doesn't add to our bitmap.
    private int GetBitmask(ITilemap tilemap, Vector3Int position)
    {
        if (tilemap.GetTile(position) == this)
        {
            return 1;
        }

        return 0;
    }

    //For checkcorners. Same thing as GetBitMask but additional boolean check to see if it's surrounded by neighbors so we don't have to consider any unsurrounded corner tiles
    private int CheckCorners(ITilemap tilemap, Vector3Int position, Vector3Int neighbor1, Vector3Int neighbor2)
    {
        if (tilemap.GetTile(neighbor1) == this && tilemap.GetTile(neighbor2) == this)
        {
            if (tilemap.GetTile(position) == this)
            {
                return 1;
            }

        }
        return 0;
    }


#if UNITY_EDITOR
    //Adds a new menu for creating a watertile in assets
    [MenuItem("Assets/Create/Tiles/WaterTile")]
    public static void CreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Watertile", "New Watertile", "asset", "Save watertile", "Assets");
        if (path =="")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterTile>(), path);
    }
#endif

}
