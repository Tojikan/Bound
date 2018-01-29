using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSave : MonoBehaviour
{
    private TileSet tileSet;

    public Tilemap tilemap;
    public BoundsInt GameArea;
    
	void Start ()
    {
        string tilesetPath = "Assets/Tiles/TestSet.asset";
	    tileSet = AssetDatabase.LoadAssetAtPath<TileSet>(tilesetPath);
        Debug.Log(tilemap.GetTile(new Vector3Int(0, 0, 0)));


    }
	
    private int[] GetTileInfo()
    {
        int[] tileArray = new int[GameArea.size.x * GameArea.size.y * GameArea.size.z];
        return tileArray;

    }


}
