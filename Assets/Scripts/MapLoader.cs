using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using jasonNamespace;
using System.IO;

//Tests how we can set tiles in a block. So far, successful. Alternates between two. 

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
        jasonNamespace.Jason Fred = new Jason(1, 2f, "hi", new int[] { 1, 2 });
        jasonNamespace.Jason newJason = new Jason(5, 5.2f, "Eat a dick", new int[] { 1, 2, 3 }, Fred);
        string JasonString = JsonUtility.ToJson(newJason);

        WriteString("cookie " + JasonString);
        //string fromFile = ReadString();
        //jasonNamespace.Jason cookieMonster = JsonUtility.FromJson<jasonNamespace.Jason>(fromFile);

        //Debug.Log(cookieMonster.NestedJason);
    }

    static void WriteString(string writeThis)
    {
        string path = "Assets/test.txt";

        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(writeThis);

        writer.Close();
    }

    static string ReadString()
    {
        string path = "Assets/test.txt";

        StreamReader reader = new StreamReader(path);

        string newThingie = reader.ReadToEnd();
        reader.Close();

        return newThingie; 
    }
}

