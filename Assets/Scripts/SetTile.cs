using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SetTile : MonoBehaviour
{
    public Tilemap TestSet;
    public TileBase Tile;
    public Vector3Int position;

    // Use this for initialization
    //Test file to try different ways on how we can set a tile by script using the SetTile command.
    void Start ()
    {
        string path = "Assets/Tiles/NewWaterTile.asset";
        Tile = AssetDatabase.LoadAssetAtPath<WaterTile>(path);
        TestSet.SetTile(position, Tile);
        TestSet.SetTile(position + new Vector3Int(1,0,0), Tile);
    }
}
