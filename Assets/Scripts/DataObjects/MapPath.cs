using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DEPRECATED
//Literally just a string for a map path.
[CreateAssetMenu(fileName = "New MapPath", menuName = "MapData/MapPath" )]
public class MapPath : ScriptableObject
{
    public string mapfilePath;
}
