using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        mCacheCommandBuffer = new CommandBuffer();
        Shader.EnableKeyword("TIME_ON");
        
    }
    CommandBuffer mCacheCommandBuffer= null;
    // Update is called once per frame
    void Update()
    {
        //if(m_mesh != null && m_material!=null && SystemInfo.supportsInstancing && m_material.enableInstancing)
        //{
        //    Matrix4x4 mat = transform.localToWorldMatrix;
        //    mat = mat* Matrix4x4.Scale(new Vector3(8,8,8));
        //    Matrix4x4[] matrix4X4 = new Matrix4x4[] {
        //        mat*Matrix4x4.Translate(new Vector3(0, 0, 0)),
        //        mat*Matrix4x4.Translate(new Vector3(1, 0, 0)),
        //        mat*Matrix4x4.Translate(new Vector3(0, 1, 0)),
        //        mat*Matrix4x4.Translate(new Vector3(0, 0, 1)),
        //    };
        //    //foreach (var a in matrix4X4)
        //    //{
        //    //    Graphics.DrawMesh(m_mesh, a, m_material, 0);
        //    //}
        //    Graphics.DrawMeshInstanced(m_mesh, 0, m_material, matrix4X4);
        //}
            
    }
    GameObject m_object = null;
    public Mesh m_mesh = null;
    public Material m_material = null;
    
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0,0,200,80), "LoadPrefab1"))
        {
            int iHandle = ResourceMgr.Instance.InstantiatePrefabAsync("Test/GameObject.prefab", LoadPrefabCompleted);
        }

        if (GUI.Button(new Rect(200, 0, 200, 80), "LoadPrefab2"))
        {
            int iHandle = ResourceMgr.Instance.InstantiatePrefabAsync("Test/Cube.prefab", LoadPrefabCompleted1);
        }


        if (GUI.Button(new Rect(0, 80, 200, 80), "LoadShader"))
        {
            int iHandle = ResourceMgr.Instance.LoadAssetAsync<Shader>("Test/Test2.shader", LoadShaderCompleted);
        }

        if (GUI.Button(new Rect(0, 160, 200, 80), "LoadSV"))
        {
            //ResourceMgr.Instance.LoadAssetAsync<ShaderVariantCollection>("Test/shadervariants.shadervariants", (ihandel, obj) => {
            //    ShaderVariantCollection shaderVariantCollection = obj as ShaderVariantCollection;
            //    if (shaderVariantCollection != null)
            //    {
            //        shaderVariantCollection.WarmUp();
            //        Log.Info("shaderVariantCollection.WarmUp");
            //    }
            //});
            int iHandle = ResourceMgr.Instance.LoadAssetAsync<ShaderVariantCollection>("Test/shadervar.shadervariants", LoadShadervariantCompleted);
        }
    }


    void LoadPrefabCompleted(int iHandle, UnityEngine.Object obj)
    {
        var @object = obj as GameObject;
        if (@object != null)
        {
            @object.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
    }
    void LoadPrefabCompleted1(int iHandle, UnityEngine.Object obj)
    {
        m_object = obj as GameObject;
        if (m_object != null)
        {
            MeshFilter meshFilter = m_object.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                m_mesh = meshFilter.sharedMesh;
            }
            MeshRenderer meshRenderer = m_object.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                m_material = new Material(meshRenderer.sharedMaterial);
            }
            //m_object.SetActive(false);
            //@object.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
    }

    void LoadShaderCompleted(int iHandle, UnityEngine.Object obj)
    {
        Shader shader = obj as Shader;
        if (shader != null && m_object != null)
        {
            MeshRenderer meshRenderer = m_object.GetComponent<MeshRenderer>();
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
