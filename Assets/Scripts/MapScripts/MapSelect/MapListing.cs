using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMenus;
using BoundMaps;
using UnityEngine.UI;

namespace BoundMenus
{
    //Map listing information
    public class MapListing : MonoBehaviour
    {
        private MapMetaObject mapMeta;                               //Meta information of the object
        public Text listingText;                                    //Drag reference to the text component of the button

        //Loads the map meta in this listing on click
        public void OnMapSelect()
        {
            SelectMap.instance.SetMapOnClick(mapMeta);
        }

        //Called when the map list is generated to populate each listing with information
        public void ConstructListing(MapMetaObject meta)
        {
            //Set the map meta
            mapMeta = meta;
            //Set the text of the button, removing the .bound
            string mapName = meta.name;
            mapName = mapName.Replace(".bound", "");
            listingText.text = mapName;
            //Set the scale to 1. For some reason, it's getting scaled up when instantiated (probably something to do with the vertical layout group
            transform.localScale = Vector3.one;
        }
    }
}
