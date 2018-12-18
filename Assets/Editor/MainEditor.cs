using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Main))]
public class MainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var spIter = serializedObject.GetIterator();

        SerializedProperty serializedProperty = serializedObject.FindProperty("ResMode");
        if(serializedProperty!=null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedProperty);
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
