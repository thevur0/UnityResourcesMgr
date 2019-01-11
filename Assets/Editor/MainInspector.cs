using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Main))]
public class MainInspector : Editor
{
    ResourceSetting m_ResourceSetting = null;
    SerializedObject m_So = null;

    private void Awake()
    {
        m_ResourceSetting = AssetDatabase.LoadAssetAtPath<ResourceSetting>("Assets/Resources/ResSetting.asset");
        m_So = new SerializedObject(m_ResourceSetting);
    }
    public override void OnInspectorGUI()
    {
        
        EditorGUILayout.BeginVertical();
        SerializedProperty sp = m_So.FindProperty("ResMode");
        EditorGUILayout.PropertyField(sp);
        sp = m_So.FindProperty("m_ResourcePath");
        EditorGUILayout.PropertyField(sp);

        EditorGUILayout.LabelField("Quality", UnityUtils.CurQualityName());
        
        EditorGUILayout.EndVertical();
        m_So.ApplyModifiedProperties();

    }


    [MenuItem("Tools/MainScene",false,0)]
    static void MainScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
    }
}
