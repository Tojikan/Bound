using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BoundEditor;
using System;

//Custom Inspector for Map Editor

[CustomEditor(typeof(MapEditorControl))]
public class MapEditorWindow : Editor
{
    public override void OnInspectorGUI()
    {
        //Reference our target
        MapEditorControl controller = (MapEditorControl)target;


        EditorGUILayout.HelpBox("Use this window to input map info and call Map Editor Functions from the various components. All Map Data is saved into a scriptable object container, which can be overwritten. When the Write Map function is called, the data in the object container will be written into a file", MessageType.Info);
        GUILayout.Label("Level Saving");
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.Space();
        base.OnInspectorGUI();

        #region Level Save Section 
        //Add new level
        if (GUILayout.Button("Add New Level", GUILayout.MinHeight(25)))
            controller.AddLevel();

        //Overwrite existing level based on level select int field
        if (GUILayout.Button("Overwrite Current Level", GUILayout.MinHeight(25)))
        {
            if (EditorUtility.DisplayDialog("Overwrite Current Level", "This will overwrite all data in the currently selected level in the Map Data Container.", "Okay", "Cancel"))
            {
                try
                {
                    controller.OverwriteLevel(controller.levelSelect);
                }
                catch (Exception e)
                {
                    EditorUtility.DisplayDialog("Error", "Unable to overwrite level. The level might not exist. Error Message: " + e, "Ok");
                }
            }
        }

        //Delete all level data on the given level set in the level select int field
        if (GUILayout.Button("Clear Current Level", GUILayout.MinHeight(25)))
        {
            if (EditorUtility.DisplayDialog("Clear Current Level", "This will clear all data in the currently selected level in the Map Data Container.", "Okay", "Cancel"))
            {
                try
                {
                    controller.RemoveLevel();
                }
                catch (Exception e)
                {
                    EditorUtility.DisplayDialog("Error", "Unable to clear level. The level might not exist. Error Message: " + e, "Ok");
                }
            }
        }
        #endregion
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("Map Saving");
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);  

        #region Map Save section
        controller.mapName = EditorGUILayout.TextField("Map Name", controller.mapName);
        controller.mapDescrip = EditorGUILayout.TextField("Map Description", controller.mapDescrip, GUILayout.Height(50));
        controller.mapImage = EditorGUILayout.ObjectField("Preview Image", controller.mapImage, typeof(object), allowSceneObjects: false);
        controller.doubleSpeed = EditorGUILayout.Toggle("Double Speed", controller.doubleSpeed);

        EditorGUILayout.HelpBox("Be sure to save Map Info and Meta before writing a file. This will save your selected tile/obstacle sets and the map meta information. ", MessageType.Warning);
        //Save map meta information and data set names
        if (GUILayout.Button("Save Map Info and Meta", GUILayout.MinHeight(25)))
            controller.SaveMapInfo();
        //Writes the map to a file
        if (GUILayout.Button("Write Map to File", GUILayout.MinHeight(25)))
            controller.WriteMap();
        //Clears the entire map scriptable object
        if (GUILayout.Button("Clear Map Data Object", GUILayout.MinHeight(25)))
        {
            if (EditorUtility.DisplayDialog("Warning!", "This will delete ALL Map Data and Map Meta information from the Map Data Container. Please be sure you have written the map to a file before proceeding", "Ok", "Cancel"))
            {
                controller.ClearMapData();
            }
        }
        #endregion

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("Map and Level Loading");
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();

        #region Loading
        EditorGUILayout.HelpBox("You may load levels from the map data container by typing in your desired level number in the int field and selecting load map ", MessageType.Warning);

        //Renders a level from the scriptable object based on selected level
        if (GUILayout.Button("Load Target Level", GUILayout.MinHeight(25)))
        {
            if (EditorUtility.DisplayDialog("Warning!", "This will overwrite the existing map in the scene. Please save before proceeding", "Ok", "Cancel"))
            {
                controller.LoadSelectedLevel(controller.levelSelect);
            }
        }

        //Load a map and overwrite into the scriptable object
        if (GUILayout.Button("Load a Map", GUILayout.MinHeight(25)))
        {
            string path = EditorUtility.OpenFilePanel("Open Map", "./Assets/Maps/", ".bound");
            Debug.Log(path);
            if (path == "")
            {
                return;
            }
            else if (path != "")
            {
                if (EditorUtility.DisplayDialog("Warning!", "This will overwrite ALL Map Data and Map Meta information from the Map Data Container. Please be sure you have written the map to a file before proceeding", "Ok", "Cancel"))
                {
                    controller.LoadMapIntoEditor(path);
                }
            }
        }
        #endregion
    }
}
 