using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BoundEditor;


[CustomEditor(typeof(MapEditorControl))]
public class MapEditorWindow : Editor
{
    public override void OnInspectorGUI()
    {


        EditorGUILayout.HelpBox("Use this window to input map info and call map editor functions to save and load maps", MessageType.Info);
        base.OnInspectorGUI();

        //Reference our target
        MapEditorControl controller = (MapEditorControl)target;

        //controller.mapName = EditorGUILayout.TextField("Map Name", controller.mapName);
        //controller.mapDescrip = EditorGUILayout.TextField("Map Description", controller.mapDescrip, GUILayout.Height(50));
        //controller.mapImage = EditorGUILayout.ObjectField("Preview Image", controller.mapImage, typeof(Object), allowSceneObjects: false);
        //controller.doubleSpeed = EditorGUILayout.Toggle("Double Speed", controller.doubleSpeed);

        if (GUILayout.Button("Add New Level"))
            controller.AddLevel();
        if (GUILayout.Button("Overwrite Level"))
        {
            controller.OverwriteLevel(controller.levelSelect);
        }
        EditorGUILayout.HelpBox("This will overwrite the level data in the level set in the levelselect field. Please be careful before overwriting a level", MessageType.Info);
        if (GUILayout.Button("Store Map Information and Meta"))
            controller.SaveMapInfo();
        EditorGUILayout.HelpBox("This will only save your map information such as map title and map meta to the scriptable object. This does not save as a file.", MessageType.Info);
        if (GUILayout.Button("Write Map to File"))
            controller.WriteMap();
        EditorGUILayout.HelpBox("This will write out your map to a file in the maps directory", MessageType.Info);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        if (GUILayout.Button("Clear Map Data Object"))
            controller.ClearMapData();
        EditorGUILayout.HelpBox("This will clear all map object data within the container. If you don't save to file, it will be lost forever.", MessageType.Warning);
        if (GUILayout.Button("Delete Level"))
            controller.RemoveLevel();
        EditorGUILayout.HelpBox("This deletes the currently selected level.", MessageType.Warning);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();


    }
}
 