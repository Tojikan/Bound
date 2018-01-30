using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BoundMaps;

//Editor script to create a custom editor for our SaveMap calss

[CustomEditor(typeof(SaveMapInEditor))]
public class SaveMapWindow : Editor
{
    public override void OnInspectorGUI()
    {
        //Instructions
        EditorGUILayout.HelpBox("Use this window to save maps. Maps are saved as a list of levels. Set the level number to set which level you're working on." +
            "Set a valid mapname and click Save Map to write all the levels to a file. When crafting a new map, " +
            "remember to clear all levels or else the data remains. Remember to set the tileset and the layers.", MessageType.None);

        //Calls the base so we can set the tiles/layers 
        base.OnInspectorGUI();

        //Reference our target
        SaveMapInEditor mySaveScript = (SaveMapInEditor)target;

        //A label for nonwriteable information purposes
        EditorGUILayout.LabelField("Map Name", mySaveScript.mapName);
        EditorGUILayout.LabelField("Level", mySaveScript.levelNumber.ToString());

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Button to save our level. Clicking a button returns true
        if (GUILayout.Button("Save Current Level"))
        {
            mySaveScript.SaveLevel();
        }

        //Button to save our map
        EditorGUILayout.Space();
        if (GUILayout.Button("Save Map"))
        {
            mySaveScript.SaveMap();
        }

        //Button to clear our level list
        EditorGUILayout.Space();
        if (GUILayout.Button("Clear All Levels"))
        {
            mySaveScript.ClearLevels();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
}
