using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundEngine;

[CreateAssetMenu(fileName = "New_MapObjectSet", menuName = "Map_Objects/Sets/ObjectSet")]
public class MapObjectSet : ScriptableObject
{
    public string setName = "New MapObjectSet";
    public MapObject[] MapObjectPrefabs;
}

