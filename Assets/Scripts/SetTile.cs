using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SetTile : MonoBehaviour
{
    public Tilemap TestSet;
    public Tile Tile;
    public Vector3Int position;
    public List<Tile> newList;

    private TileBase newTileBase;

    // Use this for initialization
    void Start ()
    {
        TestSet.SetTile(position, Tile);
        WaterTile A = new WaterTile();
        newList.Add(A);
    }

    
    


        
}
