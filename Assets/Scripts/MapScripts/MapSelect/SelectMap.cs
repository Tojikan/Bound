using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;
using System;

namespace BoundMenus
{
    //Manages what happens when a map is selected
    public class SelectMap : MonoBehaviour
    {
        private DisplayMap displayMap;                                      //reference to the display component
        public static SelectMap instance;                                   //Singleton instance of this
        public CurrentMapSelection currentSelection;                        //Drag Scriptable Object selection to this, which stores the current selection and also stores the last map selected

        private void Awake()
        {
            //Set singleton instance
            if (instance == null)
                instance = this;
            else if (instance != this)
                DestroyObject(this);
        }

        private void Start()
        {
            //Get component reference and set the map display info according to the previous selected map
            displayMap = GetComponent<DisplayMap>();
            UpdateSelection();
        }

        //Public function that is called when a button event is clicked. Takes a parameter of the map to be selected
        public void SetMapOnClick(MapMetaObject map)
        {
            if (CheckMapMeta())
            {
                SetMapSelection(map);
                UpdateSelection();
            }
        }

        //Update the map info display
        private void UpdateSelection()
        {
            displayMap.SetMapDisplay(currentSelection.meta);
        }

        //TO DO: implement a way to check if the meta file is valid
        private bool CheckMapMeta()
        {
            return true;
        }

        //Sets the current selection object
        private void SetMapSelection(MapMetaObject map)
        {
            currentSelection.meta = map;
        }

    }
}
