using BoundMaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Scriptable Object to hold the current (and previously selected) map meta
[CreateAssetMenu(fileName = "New MapMeta", menuName = "MapData/MapMeta")]
public class CurrentMapSelection : ScriptableObject
{
    public MapMetaObject meta;
    public MapFile mapData;
}
