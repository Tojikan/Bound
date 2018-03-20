using BoundMaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapMeta", menuName = "MapData/MapMeta")]
public class CurrentMapSelection : ScriptableObject
{
    public MapMetaObject meta;
}
