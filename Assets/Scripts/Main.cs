using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    static Main main = null;
    public enum ResLoadMode
    {
        Resource,
        AssetDatabase,
        AssetBundle,
    }
    [SerializeField]
    private ResLoadMode ResMode = ResLoadMode.Resource;


    static public ResLoadMode ResourceMode
    {
        get
        {
            return main.ResMode;
        }
    }

    static string m_ResourcePath = @"Assets/ABResources";
    static public string ResourcePath { get { return m_ResourcePath; } }
    static string m_FileList = @"filelist.json";
    static public string FileList { get { return m_FileList; } }


    private void Awake()
    {
        main = this;
        ResourceMgr.Instance.Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        int iHandle = ResourceMgr.Instance.LoadAssetAsync<GameObject>("Cube.prefab", AssetLoadCompleted);
    }

    void AssetLoadCompleted(int iHandle, UnityEngine.Object obj)
    {
        GameObject go = obj as GameObject;
        if (go != null)
        {
            GameObject.Instantiate(go);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
