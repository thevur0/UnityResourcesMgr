using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceMgr : Singleton<ResourceMgr> {

    ResourceSetting ResourceSetting { get; set; }
    IAssetLoader m_Loader = null;
    public delegate void AssetLoadCompleted(int iHandle, UnityEngine.Object obj);
    public void Init(string sResSetting)
    {
        LoadResSetting(sResSetting);
        CreateLoader();
    }
    void LoadResSetting(string sResSetting)
    {
        ResourceSetting = Resources.Load<ResourceSetting>(sResSetting);
    }
    void CreateLoader()
    {
        switch (ResourceSetting.ResMode)
        {
            case ResourceSetting.ResLoadMode.Resource:
                m_Loader = new ResAssetLoader();
                break;
            case ResourceSetting.ResLoadMode.AssetDatabase:
                m_Loader = new EditorAssetLoader(ResourceSetting.ResourcePath);
                break;
            case ResourceSetting.ResLoadMode.AssetBundle:
                m_Loader = new ABAssetLoader(ResourceSetting.FileList);
                break;
        }
    }
    public T LoadAsset<T>(string sPath) where T : UnityEngine.Object
    {
        AssetInfo<T> ai = AssetCacheMgr.Instance.GetAssetCache<T>(sPath, m_Loader);
        return ai.Load(m_Loader);
    }
    public int LoadAssetAsync<T>(string sPath, AssetLoadCompleted OnAssetLoadCompleted) where T : UnityEngine.Object
    {
        AssetInfo<T> ai = AssetCacheMgr.Instance.GetAssetCache<T>(sPath, m_Loader);
        return ai.LoadAsync(m_Loader, OnAssetLoadCompleted);
    }

    public GameObject InstantiatePrefab(string sPath)
    {
        GameObject gameobject = LoadAsset<GameObject>(sPath);
        GameObject go = GameObject.Instantiate(gameobject) as GameObject;
        m_PrefabMap.Add(go.GetInstanceID(), gameobject.GetInstanceID());
        return go;
    }

    public int InstantiatePrefabAsync(string sPath, AssetLoadCompleted OnAssetLoadCompleted)
    {
        int iHandle = LoadAssetAsync<GameObject>(sPath, (ihandle,gameobject)=> {
            GameObject go = GameObject.Instantiate(gameobject) as GameObject;
            m_PrefabMap.Add(go.GetInstanceID(), gameobject.GetInstanceID());
            OnAssetLoadCompleted.Invoke(ihandle, go);
            });
        return iHandle;
    }

    public void DestroyObject(UnityEngine.Object @object)
    {
        int iAssetID = @object.GetInstanceID();
        if (m_PrefabMap.TryGetValue(@object.GetInstanceID(), out iAssetID))
        {
            m_PrefabMap.Remove(@object.GetInstanceID());
            Object.DestroyImmediate(@object);
        }
        AssetCacheMgr.Instance.ReleaseAsset(iAssetID);
    }
    Dictionary<int, int> m_PrefabMap = new Dictionary<int, int>();

    public void Update()
    {
        AssetCacheMgr.Instance.Update();
    }

    public void Destroy()
    {
        AssetCacheMgr.Instance.Destroy();
    }
}
