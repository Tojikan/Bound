using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGet : MonoBehaviour
{
    public Grid gridcheck;
    public Tilemap tilemap;

  

	void Update ()
    {
        Vector3Int cellPos = gridcheck.WorldToCell(transform.position);
        TileBase thisTile = tilemap.GetTile(cellPos);
        Debug.Log(thisTile);
	}
}
