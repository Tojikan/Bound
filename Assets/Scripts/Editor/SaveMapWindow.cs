using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BoundMaps;
using System.IO;

//Editor script to create a custom editor for our SaveMap class

[CustomEditor(typeof(SaveMapInEditor))]
public class SaveMapWindow : Editor
{
    public override void OnInspectorGUI()
    {
        //Instructions
        EditorGUILayout.HelpBox("Use this window to save maps. Maps are saved as a list of levels. Set the level number to set which level you're working on." +
            "Set a valid mapname and click Save Map to write all the levels to a file. When crafting a new map, " +
            "remember to clear all levels or else the data remains. Remember to set the tileset, layers, and start/end objects. Recommend to lock this window if you're doing serious editing", MessageType.None);

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
        if (GUILayout.Button("Add New Level"))
        {
            mySaveScript.SaveLevel();
        }
        EditorGUILayout.HelpBox("Saves the current grid into a new level at the specified level number. Will push levels up if the level already exists", MessageType.None);

        if (GUILayout.Button("Overwrite Current Level"))
        {
            mySaveScript.OverwriteLevel();
        }
        EditorGUILayout.HelpBox("Overwrites the current level. ", MessageType.None);

        if (GUILayout.Button("Load Selected Level"))
        {
            mySaveScript.LoadCurrentLevel(mySaveScript.levelNumber);
        }
        EditorGUILayout.HelpBox("Loads selected level number.  Memory resets if you enter play mode. Save map first and then use Load from File in order to recall a map ", MessageType.None);

        //Button to clear our level list

        if (GUILayout.Button("Clear All Levels"))
        {
            mySaveScript.ClearLevels();
        }
        EditorGUILayout.HelpBox("Clears all level memory", MessageType.None);


        //Button to save our map
        if (GUILayout.Button("Save Map"))
        {
            mySaveScript.SaveMap();
        }
        EditorGUILayout.HelpBox("Saves all the levels to a file", MessageType.None);


        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Loads a mapfile for us to edit. Select a mapfile below. Then click the Load From File to load that map's levels into memory ", MessageType.None);
        EditorGUILayout.LabelField("Map Name", Path.GetFileName(mySaveScript.FileToLoad));


        //Selects a map to load to the editor SaveMap list
        if (GUILayout.Button("Select a file to load from"))
        {
            string path = EditorUtility.OpenFilePanel("Open Bound", "/Assets/Maps/", "bound");

            //Make sure we have a path and then set our map to load
            if (path.Length != 0)
            {
                mySaveScript.FileToLoad = path;
            }
        }
  
        //Calls the function to load the list into the memory
        if (GUILayout.Button("Load From File"))
        {
            mySaveScript.LoadMapInEditor(); 
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
}
