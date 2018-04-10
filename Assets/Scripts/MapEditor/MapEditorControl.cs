using BoundMaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BoundEngine;

namespace BoundEditor
{
    [ExecuteInEditMode]
    public class MapEditorControl : MonoBehaviour
    {
        public MapEditorFunctions editorFunctions;
        [HideInInspector] public string mapName;                                            //Name of map to save
        [HideInInspector] public string mapDescrip;                                         //Map description text box
        [HideInInspector] public Object mapImage;                                           //Map Image. Make sure you only drag from the resources folder or else it won't work
        [HideInInspector] public bool doubleSpeed;                                          //Set if we're going double timer on this map
        public string levelTitle;                                                           //Type in the title of the level
        public enum LevelMusic
        {
            Ending, Level1, Level2, Level3, TitleScreen
        }                                                           //enum to select music
        public LevelMusic selectMusic;                                                      //Set music for current level
        public Dialogue startDialogue;                                                      //Type in level Start Dialogue in window
        public Dialogue endDialogue;                                                        //Type in level end dialogue in window
        public int levelSelect;                                                             //Type in the level you would like to select for overwriting/clearing    

        //Calls into our editor functions script to call methods
        #region Level Button Commands

        public void OverwriteLevel(int level)
        {
            editorFunctions.OverwriteLevel(level);
        }

        public void AddLevel()
        {
            editorFunctions.AddLevel();
        }
        public void RemoveLevel()
        {
            editorFunctions.RemoveLevel(levelSelect);
        }

        public void ClearAllLevels()
        {
            editorFunctions.ClearLevels();
        }

        #endregion

        #region Map Button Commands
        public void SaveMapInfo()
        {
            editorFunctions.SaveMapInfo();
        }

        public void WriteMap()
        {
            if (mapName.Length <= 0)
            {
                Debug.LogError("Blank map name. Please enter a map name");
                return;
            }
            editorFunctions.WriteToFile();
        }

        public void ClearMapData()
        {
            editorFunctions.ClearContainerData();
        }


        #endregion

        #region Loading Button Commands
        public void LoadMapIntoEditor(string path)
        {
            editorFunctions.LoadMapInEditor(path);
            Debug.Log("Loaded map at: " + path);
        }

        public void LoadSelectedLevel(int level)
        {
            editorFunctions.RenderLevelInEditor(level);
            Debug.Log("Loaded level: " + level);
        }
        #endregion
    }
}
