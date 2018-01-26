using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
{

    public TileBase tileA;
    public TileBase tileB;
    public Tilemap TestTilemap;

    public BoundsInt area;

    void Start()
    {
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];
        for (int index = 0; index < tileArray.Length; index++)
        {
            tileArray[index] = index % 2 == 0 ? tileA : tileB;
        }
        TestTilemap.SetTilesBlock(area, tileArray);
    }
}
