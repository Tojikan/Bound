using BoundMaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BoundEditor
{
    [ExecuteInEditMode]
    public class MapEditorControl : MonoBehaviour
    {
        public MapEditorFunctions editorFunctions;
        public string mapName;                                                              //Name of map to save
        public string mapDescrip;                                                           //Map description text box
        public Sprite mapImage;                                                             //Map Image. Make sure you only drag from the resources folder or else it won't work
        public bool doubleSpeed;                                                            //Set if we're going double timer on this map
        public string levelTitle;                                                           //Type in the title of the level
        public enum LevelMusic
        {
            Ending, Level1, Level2, Level3, TitleScreen
        }                                                           //enum to select music
        public LevelMusic selectMusic;                                                      //Set music for current level
        public Dialogue startDialogue;                                                      //Type in level Start Dialogue in window
        public Dialogue endDialogue;                                                        //Type in level end dialogue in window
        public int levelSelect;

        //Calls into our editor functions script to call methods
        #region Button Commands

        public void OverwriteLevel(int level)
        {
            editorFunctions.OverwriteLevel(level);
        }

        public void AddLevel()
        {
            Debug.Log(editorFunctions);
            editorFunctions.AddLevel();
        }

        public void SaveMapInfo()
        {
            editorFunctions.SaveMapInfo();
        }

        public void WriteMap()
        {
            if (mapName.Length <= 0)
            {
                Debug.Log("Blank map name. Please enter a map name");
                return;
            }
            editorFunctions.WriteToFile();
        }

        public void ClearMapData()
        {
            editorFunctions.ClearContainerData();
        }

        public void RemoveLevel()
        {
            editorFunctions.RemoveLevel(levelSelect);
        }
        #endregion

    }
}
