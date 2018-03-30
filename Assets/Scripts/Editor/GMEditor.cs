using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//Custom Editor options for our Game Manager
//Mainly used to let us skip levels
[CustomEditor(typeof(GameManager))]
public class GMEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager myGame = (GameManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Lets us skip levels for testing purposes
        EditorGUILayout.HelpBox("Lets us skip levels for testing purposes. Enter the level you want to skip around and then press to move to the level after that.", MessageType.None);
        myGame.currentLevel = EditorGUILayout.IntField("Level To Load", myGame.currentLevel);

        if (GUILayout.Button("Skip to"))
        {
            myGame.LevelSkip();
            myGame.LoadNextLevel();
        }
    }
}
