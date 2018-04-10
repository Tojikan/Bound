using BoundMaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Scriptable Object to hold the current (and previously selected) map meta
[CreateAssetMenu(fileName = "New MapDataContainer", menuName = "MapData/MapDataContainer")]
public class MapDataContainer : ScriptableObject
{
    public MapMetaObject meta;
    public MapFile mapData;

    public void ClearMeta()
    {
        meta.fileLocation = "";
        meta.description = "";
        meta.imagePath = "";
        meta.name = "";
    }

    public void ClearMapData()
    {
        mapData.eventTriggerSet = "";
        mapData.obstacleSet = "";
        mapData.tileset = "";
        mapData.levels.Clear();
        mapData.numberOfLevels = 0;
    }
}
