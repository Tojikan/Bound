using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BoundMaps;
using System.IO;
using BoundEditor;

//Editor script to create a custom editor for our SaveMap class

[CustomEditor(typeof(SaveMapInEditor))]
public class SaveMapWindow : Editor
{
    public override void OnInspectorGUI()
    {
        //Instructions
        EditorGUILayout.HelpBox("This component will let you save levels into memories and then combine all levels into a mapfile. Enter the level number into the field to select which level to save/edit. All levels only exist in memory (play mode will clear it). To store it, click the Save Map to save it all to a file. ", MessageType.None);

        //Calls the base so we can set the tiles/layers 
        base.OnInspectorGUI();

        //Reference our target
        SaveMapInEditor mySaveScript = (SaveMapInEditor)target;

        //A label for nonwriteable information purposes
        EditorGUILayout.LabelField("Map Name", mySaveScript.mapName);
        EditorGUILayout.LabelField("Level", mySaveScript.levelNumber.ToString());

        EditorGUILayout.Space();
        EditorGUILayout.Space();



        EditorGUILayout.HelpBox("Saves the current grid into a leveldata at the current level number. Will create a new level or overwrite existing if it doesn't exist ", MessageType.None);
        //Button to save our level. Clicking a button returns true
        if (GUILayout.Button("Save Current Level"))
        {
            mySaveScript.SaveLevel();
        }


        EditorGUILayout.HelpBox("Loads the current level number into the editor", MessageType.None);
        if (GUILayout.Button("Load Selected Level"))
        {
            mySaveScript.LoadCurrentLevel(mySaveScript.levelNumber);
        }


        //Button to clear our level list
        EditorGUILayout.HelpBox("Warning! Clears all level memory", MessageType.None);
        if (GUILayout.Button("Clear All Levels"))
        {
            mySaveScript.ClearLevels();
        }


        EditorGUILayout.HelpBox("Saves all level memory into a file with the given Map Name. ", MessageType.None);
        //Button to save our map
        if (GUILayout.Button("Save Map To a File"))
        {
            mySaveScript.SaveMap();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Loads a previous mapfile to edit ", MessageType.None);
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

        //calls the function to load the list into the memory
        if (GUILayout.Button("load from file"))
        {
            mySaveScript.LoadMapInEditor();
        }
    }
}
