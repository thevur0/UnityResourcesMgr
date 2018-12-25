using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    GameObject @object = null;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0,0,200,80), "LoadPrefab1"))
        {
            int iHandle = ResourceMgr.Instance.InstantiatePrefabAsync("Test/Cube.prefab", LoadPrefabCompleted);
        }

        if (GUI.Button(new Rect(200, 0, 200, 80), "LoadPrefab2"))
        {
            int iHandle = ResourceMgr.Instance.InstantiatePrefabAsync("Test/Cube1.prefab", LoadPrefabCompleted1);
        }


        if (GUI.Button(new Rect(0, 80, 200, 80), "LoadShader"))
        {
            int iHandle = ResourceMgr.Instance.LoadAssetAsync<Shader>("Test/Test2.shader", LoadShaderCompleted);
        }

        if (GUI.Button(new Rect(0, 160, 200, 80), "LoadSV"))
        {
            int iHandle = ResourceMgr.Instance.LoadAssetAsync<ShaderVariantCollection>("Test/shadervar.shadervariants", LoadShadervariantCompleted);
        }
    }


    void LoadPrefabCompleted(int iHandle, UnityEngine.Object obj)
    {
        @object = obj as GameObject;
        if (@object != null)
        {
            @object.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
    }
    void LoadPrefabCompleted1(int iHandle, UnityEngine.Object obj)
    {
        GameObject @object1 = obj as GameObject;
        if (@object1 != null)
        {
            @object1.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
    }

    void LoadShaderCompleted(int iHandle, UnityEngine.Object obj)
    {
        Shader shader = obj as Shader;
        if (shader != null && @object != null)
        {
            MeshRenderer meshRenderer = @object.GetComponent<MeshRenderer>();
            meshRenderer.material.shader = shader;
        }
    }

    void LoadShadervariantCompleted(int iHandle, UnityEngine.Object obj)
    {
        ShaderVariantCollection shader = obj as ShaderVariantCollection;
        if (shader != null)
        {
            Debug.Log(shader.shaderCount);
            Debug.Log(shader.variantCount);
            shader.WarmUp();
        }
    }
}
