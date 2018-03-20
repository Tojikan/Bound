using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMenus;
using BoundMaps;

namespace BoundMenus
{
    //Button function to select a map meta
    public class MapItemOnSelect : MonoBehaviour
    {
        public MapMetaObject mapMeta;

        public void OnMapSelect()
        {
            SelectMap.instance.SetMapOnClick(mapMeta);
        }
    }
}
