using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//Custom Editor options for our Game Manager
//Mainly used to let us skip levels
[CustomEditor(typeof(GameManager))]
public class GMEditor : Editor
{
    public MapPath mapHolder;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager myGame = (GameManager)target;
        mapHolder = myGame.pathToMap;


        //Selects a map to load
        EditorGUILayout.LabelField("Map Name", System.IO.Path.GetFileName(path: mapHolder.mapfilePath));
        if (GUILayout.Button("Select a file to load from"))
        {
            string path = EditorUtility.OpenFilePanel("Open Bound", "/Assets/Maps/", "bound");

            //Make sure we have a path and then set our map to load
            if (path.Length != 0)
            {
                mapHolder.mapfilePath = path;
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Lets us skip levels for testing purposes. Enter the level you want to skip around and then press to move to the level after that.", MessageType.None);
        myGame.currentLevel = EditorGUILayout.IntField("Level To Load", myGame.currentLevel);

        if (GUILayout.Button("Skip to"))
        {
            myGame.LoadNextLevel();
        }
    }
}
