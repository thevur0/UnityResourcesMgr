using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

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
}
