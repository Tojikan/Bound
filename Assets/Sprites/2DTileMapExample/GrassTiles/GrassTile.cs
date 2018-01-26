using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

//GrassTile class for grass terrain
//Based on a 4bit Bitmapping scheme
//TO DO: MAKE IT AN 8BIT. NEED TO DRAW OUT THE TILES. MAKE THE WATERFILES A BITMAP AND THEN JUST COPY IT HERE

//Careful not to mess up the loops. This directlty modifies the Unity Editor, so you can freeze up the Editor if you don't do the loops right



public class GrassTile : Tile {

    //Adds an array field for us to store all our sprites
    [SerializeField]
    private Sprite[] grassSprites;

    //Function to check surrounding tiles. Checks to see if they are grass tiles and if so, refreshes the tile
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        //Loop over neighboring tiles
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                //Gets the position of the neighboring tile
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);

                //Checks if the correct tile type, and then refreshes. 
                if (CheckGrass(tilemap, nPos))
                {
                    tilemap.RefreshTile(nPos);
                }

            }
        }
    }


    //This overrides the GetTileData to let us update each tile according to neighbors
    //If the neighboring tile is same tile, add to our bitmap to map out the tile we use. 
    //4 Bit Bitmap = North - West - South - East : 1 - 2 - 4 - 8

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        //Call GetTileData in order to let us set TileCollider if it's included in the current layer
        base.GetTileData(position, tilemap, ref tileData);

        //Bitmap counter 
        int composition = 0;

        //Positions for the 4 cardinal directions around tile
        Vector3Int eastSide = new Vector3Int(position.x + 1, position.y, position.z);
        Vector3Int southSide = new Vector3Int(position.x, position.y - 1, position.z);
        Vector3Int westSide = new Vector3Int(position.x - 1, position.y, position.z);
        Vector3Int northSide = new Vector3Int(position.x, position.y + 1, position.z);

        //Calculates our bitmap accordingly by adding the 4bit number of a corresponding side if it's the same tile
        composition = (HasGrass(tilemap, northSide) * 1) + (HasGrass(tilemap, westSide) * 2) + (HasGrass(tilemap, southSide) * 4) + (HasGrass(tilemap, eastSide) * 8);

        //Case Switch construct to determine which sprite we use for this tile
        switch(composition)
        {
            case 0:
                tileData.sprite = grassSprites[0];
                break;
            case 1:
                tileData.sprite = grassSprites[1];
                break;
            case 2:
                tileData.sprite = grassSprites[2];
                break;
            case 3:
                tileData.sprite = grassSprites[3];
                break;
            case 4:
                tileData.sprite = grassSprites[4];
                break;
            case 5:
                tileData.sprite = grassSprites[5];
                break;
            case 6:
                tileData.sprite = grassSprites[6];
                break;
            case 7:
                tileData.sprite = grassSprites[7];
                break;
            case 8:
                tileData.sprite = grassSprites[8];
                break;
            case 9:
                tileData.sprite = grassSprites[9];
                break;
            case 10:
                tileData.sprite = grassSprites[10];
                break;
            case 11:
                tileData.sprite = grassSprites[11];
                break;
            case 12:
                tileData.sprite = grassSprites[12];
                break;
            case 13:
                tileData.sprite = grassSprites[13];
                break;
            case 14:
                tileData.sprite = grassSprites[14];
                break;
            case 15:
                //randomizes the center tile so we can different designs
                int randomVal = Random.Range(15, 18);
                tileData.sprite = grassSprites[randomVal];
                break;
        }

    }

    //For when we calculate our bitmap. If the tile is the same tile, returns 1 and adds to our bitmap. Otherwise returns a 0 so it doesn't add to our bitmap.
    private int HasGrass(ITilemap tilemap, Vector3Int position)
    {
        if (tilemap.GetTile(position) == this)
        {
            return 1;
        }

        return 0;
    }

    //Bool function for when we check if neighboring tiles are the same. I realize its basically the same as HasGrass, but I'm too lazy to write more code to cast a bool into an int. 
    private bool CheckGrass(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }






#if UNITY_EDITOR
    //Adds a new menu option for creating a grasstile in assets
    [MenuItem("Assets/Create/Tiles/GrassTile")]
    public static void CreateGrassTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Grasstile", "New Grasstile", "asset", "Save grasstile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GrassTile>(), path);
    }
#endif
}
