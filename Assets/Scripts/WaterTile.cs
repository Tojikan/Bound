using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


//ScriptableTile Class to create new water tiles and set the sprite accordingly
//8bit bitmasking scheme. Need at least 48 sprites mapped out to 256 values (some duplicating, use the array to match sprites)

//Create a new asset via AssetMenu. Former code to create a new asset is unnecessary and Unity handles new asset creation under the hood
[CreateAssetMenu(fileName = "NewWaterTile", menuName = "Tiles/TileSet0/WaterTile", order = 1)]
public class WaterTile : Tile
{

    //Adds a field to the watertile asset
    [SerializeField]
    private Sprite[] waterSprites;


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

        //Index for which sprite to display. Default to 0 just in case
        int tileNum = 0;

        //Set the collider for this Tile
        tileData.colliderType = ColliderType.Sprite;

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
        //Uses checkcorners for any corner tiles. Uses checktiles for orthogonal
        //Adds the corresponding value if the checkcorner/checktiles returns True
        bitmask += CheckCorners(tilemap, northWestSide, northSide, westSide) ? 1 : 0;
        bitmask += CheckTile(tilemap, northSide) ? 2 : 0;
        bitmask += CheckCorners(tilemap, northEastSide, northSide, eastSide) ? 4 : 0;
        bitmask += CheckTile(tilemap, westSide) ? 8 : 0;
        bitmask += CheckTile(tilemap, eastSide) ? 16 : 0;
        bitmask += CheckCorners(tilemap, southWestSide, westSide, southSide) ? 32 : 0;
        bitmask += CheckTile(tilemap, southSide) ? 64 : 0;
        bitmask += CheckCorners(tilemap, southEastSide, eastSide, southSide) ? 128 : 0;


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

        //if we do return an index, then set the tileNum
        if (tempNum != -1)
        {
            tileNum = tempNum;
        }

        tileData.sprite = waterSprites[tileNum];
    }

    //Array to store bitmask values. Match the index number to the index in the sprite array. Make sure new tilesets are ordered in the same way
    //Array is ordered around the left to right numbering scheme for neighboring tiles. 
    //Store 48 possible tiles
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

    //For checkcorners. Same thing as GetBitMask but additional boolean check to see if it's surrounded by neighbors so we don't have to consider any unsurrounded corner tiles
    private bool CheckCorners(ITilemap tilemap, Vector3Int position, Vector3Int neighbor1, Vector3Int neighbor2)
    {
        if (tilemap.GetTile(neighbor1) == this && tilemap.GetTile(neighbor2) == this)
        {
            if (tilemap.GetTile(position) == this)
            {
                return true;
            }

        }
        return false;
    }
}