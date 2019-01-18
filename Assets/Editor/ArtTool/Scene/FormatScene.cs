using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

public class FormatScene
{
    [MenuItem("Tools/Art/格式化场景")]
    static void FormatCurScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        var gameObjs = scene.GetRootGameObjects();
        foreach (var obj in gameObjs)
        {
            SetGameObject(obj);
            GameObject.Instantiate(obj);
            Object.Destroy(obj);
        }
    }


    static void SetGameObject(GameObject gameObject)
    {
        Shader shader = Shader.Find("Standard");
        var renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            var mats = renderer.sharedMaterials;
            List<Material> list = new List<Material>();
            foreach (var mat in mats)
            {
                var tex = mat.GetTexture("_MainTex");
                Material newMat = new Material(shader);
                newMat.SetTexture("_MainTex", tex);
                list.Add(newMat);
            }
            renderer.sharedMaterials = list.ToArray();
        }
        var scripts = gameObject.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            Object.DestroyImmediate(script);
        }
        var tran = gameObject.transform;
        for (int i = 0; i < tran.childCount; i++)
        {
            var child = tran.GetChild(i);
            SetGameObject(child.gameObject);
        }
    }
    [MenuItem("Tools/MyTest")]
    static void Test()
    {
        LightingDataAsset lightingDataAsset = AssetDatabase.LoadAssetAtPath<LightingDataAsset>("Assets/ABResources/Scenes/SampleScene/LightingData1.asset");
        
        SerializedObject soLightingDataAsset = new SerializedObject(lightingDataAsset);
        var prop = soLightingDataAsset.GetIterator();
        while (prop.Next(true))
        {
            Debug.Log(prop.propertyPath);
        }
        var guids = AssetDatabase.GetDependencies("Assets/ABResources/Scenes/SampleScene/LightingData1.asset");
        foreach (var guid in guids)
        {
            Debug.Log(guid);
        }
        return;
        //return;
        string sPath = "Assets/ABResources/Scenes/SampleScene.unity";
        var scene = AssetDatabase.LoadAssetAtPath(sPath,typeof(Object));

        SerializedObject serializedScene = new SerializedObject(scene);

        Type type = typeof(LightmapEditorSettings);
        var methodGetLightmapSettings = type.GetMethod("GetLightmapSettings", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
   
        Object lighttarget = methodGetLightmapSettings.Invoke(null, null) as Object;
        
        SerializedObject soLightmapSettings = new SerializedObject(lighttarget);

        SerializedProperty spLightingDataAsset = soLightmapSettings.FindProperty("m_LightingDataAsset");
        spLightingDataAsset.objectReferenceValue = lightingDataAsset;

        LightmapSettings.lightmaps = null;

        soLightingDataAsset.ApplyModifiedProperties();
        soLightmapSettings.ApplyModifiedProperties();

        serializedScene.ApplyModifiedProperties();
        scene = serializedScene.targetObject as SceneAsset;
        AssetDatabase.ImportAsset(sPath);
        AssetDatabase.SaveAssets();
    }
}
