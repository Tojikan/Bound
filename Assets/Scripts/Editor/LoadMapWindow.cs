using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BoundMaps;
using System.IO;


//Editor script for our LoadMap script to set a custom editor window
[CustomEditor(typeof(LoadMap))]
public class LoadMapWindow : Editor
{
    public override void OnInspectorGUI()
    {
        //Instructions
        EditorGUILayout.HelpBox("Use this window select which level to load on entering play mode. Click Select a Map and navigate to the maps folder to select a level. " +
            "You can also set which level to load from by entering level number in the block. If invalid, will default to level 0. Use Reload map to reload map or load a new level", MessageType.None);

        //So we can set our layers
        base.OnInspectorGUI();

        //Set our target
        LoadMap myLoadScript = (LoadMap)target;

        //Click the button to designate a map file. FYI can get you stuck in an infinte loop if you don't enclose it with a button or condition
        if (GUILayout.Button("Select a Map"))
        {
            string path = EditorUtility.OpenFilePanel("Open Bound", "/Assets/Maps/", "bound");

            //Make sure we have a path and then set our map to laod
            if (path.Length != 0)
            {
                myLoadScript.FileToLoad = path;
            }
        }

        EditorGUILayout.Space();

        //Set our level to load
        myLoadScript.LevelToLoad = EditorGUILayout.IntField("Level To Load", myLoadScript.LevelToLoad);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //For information
        EditorGUILayout.LabelField("Map Name", Path.GetFileName(myLoadScript.FileToLoad));
        EditorGUILayout.LabelField("Level", myLoadScript.LevelToLoad.ToString());

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Reload the map. So you can change levels in game without having to leave play mode
        if (GUILayout.Button("Reload Map"))
        {
            myLoadScript.LoadLevel(myLoadScript.LevelToLoad);
        }
        EditorGUILayout.HelpBox("Useful for changing in between levels during PlayMode so you don't have to exit", MessageType.None);
    }
}
