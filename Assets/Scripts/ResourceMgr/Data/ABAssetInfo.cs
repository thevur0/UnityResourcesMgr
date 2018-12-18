using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ABAssetInfo<T> : AssetInfo<T> where T :UnityEngine.Object
{
    private event ResourceMgr.AssetLoadCompleted AssetLoadCompletedEvent;
    public ABAssetInfo(string sPath, CacheManage mgr) :base(sPath,mgr)
    {
        Path = sPath;
    }
    ABInfo m_ABInfo = null;
    public override T Load(IAssetLoader loader)
    {
        if (Asset == null)
        {
            ABAssetLoader abLoader = loader as ABAssetLoader;
            if (abLoader == null)
                return null;
            string sABPath = abLoader.GetAssetBundleFromAsset(Path);
            if (string.IsNullOrEmpty(sABPath))
                return null;
            m_ABInfo = AssetCacheMgr.Instance.GetABCache(sABPath);
            m_ABInfo.LoadAB();
            Asset = m_ABInfo.LoadAsset<T>(Path);
        }
        return Asset;
    }
    public override int LoadAsync(IAssetLoader loader, ResourceMgr.AssetLoadCompleted OnAssetLoadCompleted)
    {
        if (Asset == null)
        {
            ABAssetLoader abLoader = loader as ABAssetLoader;
            if (abLoader == null)
            {
                OnAssetLoadCompleted(0, null);
                return 0;
            }
            string sABPath = abLoader.GetAssetBundleFromAsset(Path);
            if (string.IsNullOrEmpty(sABPath))
                return 0;

            m_ABInfo = AssetCacheMgr.Instance.GetABCache(sABPath);
            AssetLoadCompletedEvent += OnAssetLoadCompleted;
            AssetLoadCompletedEvent += OnLoadAssetCompeleted;
            m_ABInfo.LoadABAsync(true, null, OnLoadABCompeleted);
            return 0;
        }
        else
        {
            OnAssetLoadCompleted.Invoke(0, Asset);
            return 0;
        }
    }

    void OnLoadABCompeleted()
    {
        m_ABInfo.LoadAssetAsync<T>(Path, AssetLoadCompletedEvent);
    }
    void OnLoadAssetCompeleted(int iHandle,Object obj)
    {
        Asset = obj as T;
    }
    public override void UnLoad()
    {
        base.UnLoad();
        if(m_ABInfo!=null)
            m_ABInfo.DefRef();
    }
}