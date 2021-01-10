using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DatabaseManager))]
public class DatabaseManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DatabaseManager databaseManager = (DatabaseManager) target;
        
        if (GUILayout.Button("Reset To Default"))
        {
            databaseManager.GenerateNewSaveData();
        }
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Save"))
        {
            databaseManager.Save();
        }
        
        EditorGUILayout.Space();
        
        DrawDefaultInspector();
    }
}
