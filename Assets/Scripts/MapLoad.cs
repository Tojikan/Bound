using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

//Tests how we can set tiles in a block. 
//Figured out how to call a reference to a scriptable tile. Treat as a scriptable object and call it via the AssetDatabase class
//Then populate an array with tiles and use SetTilesBlock. Works!

public class MapLoad : MonoBehaviour
{

    private TileBase tileA;
    private TileBase tileB;
    public Tilemap TestTilemap;

    public BoundsInt area;

    void Start()
    {

        string pathA = "Assets/Tiles/NewWaterTile.asset";
        tileA = AssetDatabase.LoadAssetAtPath<WaterTile>(pathA);
        string pathB = "Assets/Tiles/NewGrassTile.asset";
        tileB = AssetDatabase.LoadAssetAtPath<GrassTile>(pathB);

        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        Debug.Log(tileArray.Length);
        for (int index = 0; index < tileArray.Length; index++)
        {
            if (index < tileArray.Length/2)
                tileArray[index] = tileA;


            else
                tileArray[index] = tileB;
        }


        TestTilemap.SetTilesBlock(area, tileArray);
    }
}
