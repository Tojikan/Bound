using System;
using UnityEditor;
using UnityEngine;

namespace ResolutionMagic
{
    [CustomEditor(typeof(ResolutionManager))]
    public class EditorResMagicEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ResolutionManager targetScript = (ResolutionManager) target;
            if (GUILayout.Button("Refresh Resolution"))
            {
                targetScript.RefreshResolution();
            }
        }
    }
}