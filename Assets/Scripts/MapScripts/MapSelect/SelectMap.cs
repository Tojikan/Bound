using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundMaps;
using System;

namespace BoundMenus
{
    public class SelectMap : MonoBehaviour
    {
        private DisplayMap displayMap;
        public CurrentMapSelection currentSelection;

        private void Start()
        {
            displayMap = GetComponent<DisplayMap>();
            UpdateSelection();
        }

        public void SetMapOnClick(MapMetaObject map)
        {
            if (CheckMapMeta())
            {
                SaveSelection(map);
                UpdateSelection();
            }
        }

        private void SetMapPath(string location)
        {

        }

        private void UpdateSelection()
        {
            SetMapPath(currentSelection.meta.fileLocation);
            displayMap.SetMapDisplay(currentSelection.meta);
            Debug.Log(currentSelection.meta);
        }


        //TO DO: implement a way to check if the meta file is valid
        private bool CheckMapMeta()
        {
            return true;
        }

        private void SaveSelection(MapMetaObject map)
        {
            currentSelection.meta = map;
        }

    }
}
