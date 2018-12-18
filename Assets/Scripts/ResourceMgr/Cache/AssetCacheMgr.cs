using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetCacheMgr : Singleton<AssetCacheMgr> {

    CacheManage m_AssetCache = new CacheManage();
    CacheManage m_ABCache = new CacheManage();

    public AssetInfo<T> GetAssetCache<T>(string sPath,IAssetLoader loader) where T : UnityEngine.Object
    {
        sPath = sPath.ToLower();
        Cache cache = m_AssetCache.GetCache(sPath);
        if (cache == null)
        {
            if (loader is ABAssetLoader)
                cache = new ABAssetInfo<T>(sPath, m_AssetCache);
            else
                cache = new AssetInfo<T>(sPath, m_AssetCache);
        }
        m_AssetCache.AddCacheToDic(sPath, cache);
        
        AssetInfo<T> assetInfo = cache as AssetInfo<T>;
        if (assetInfo != null)
        {
            assetInfo.AddRef();
            return assetInfo;
        }
        else
        {
            return null;
        }
    }

    public ABInfo GetABCache(string sPath)
    {
        Cache cache = m_ABCache.GetCache(sPath);
        if (cache == null)
        {
            cache = new ABInfo(sPath, m_ABCache);
        }
        m_ABCache.AddCacheToDic(sPath, cache);
        ABInfo abInfo = cache as ABInfo;
        if (abInfo != null)
        {
            abInfo.AddRef();
            return abInfo;
        }
        else
        {
            return null;
        }
    }

    public void Update()
    {
        m_AssetCache.Update();
        m_ABCache.Update();
    }
    Dictionary<int, Cache> m_DicAssetInfo = new Dictionary<int, Cache>();

    public void AddAsset(int iAssetID, Cache cache)
    {
        m_DicAssetInfo.Add(iAssetID, cache);
    }
    public void ReleaseAsset(int iAssetID)
    {
        Cache cache;
        if (m_DicAssetInfo.TryGetValue(iAssetID, out cache))
        {
            cache.DefRef();
        }
    }

    public void Destroy()
    {
        m_DicAssetInfo.Clear();
        m_ABCache.Destroy();
        m_AssetCache.Destroy();
    }
}
