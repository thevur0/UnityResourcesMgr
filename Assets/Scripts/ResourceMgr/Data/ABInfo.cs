using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ABInfo : Cache
{
    static ABAssetLoader m_ABLoader = null;
    public string Path { get; protected set; }
    public string ABName { get; protected set; }
    private AssetBundle Bundle { get; set; }
    string m_AssetBundlePath = string.Empty;
    private List<ABInfo> m_ListDepend = new List<ABInfo>();
    public delegate void AssetBundleLoadCompleted(int iHandle, AssetBundle assetBundle);

    public ABInfo(string sABName, CacheManage mgr):base(mgr)
    {
        m_ABLoader = ABAssetLoader.Loader;
        m_AssetBundlePath = StringUtils.PathCombine(Application.streamingAssetsPath, CrossPlatform.GetABDir());
        ABName = sABName;
        Path = StringUtils.PathCombine(m_AssetBundlePath, sABName);
    }
    static string AssetName(string sPath)
    {
        //用索引名从AB里读文件
        return sPath;
        //return StringUtils.FileName(sPath, false);
    }
    public T LoadAsset<T>(string sAsset) where T : UnityEngine.Object
    {
        T t = default(T);
        if (Bundle != null)
        {
            t = Bundle.LoadAsset<T>(AssetName(sAsset));
        }
        return t;
    }

    public int LoadAssetAsync<T>(string sAsset, ResourceMgr.AssetLoadCompleted OnAssetLoadCompleted) where T : UnityEngine.Object
    {
        if (Bundle != null)
        {
            AssetBundleRequest abReq = Bundle.LoadAssetAsync<T>(AssetName(sAsset));
            abReq.completed += (ao) => {
                AssetBundleRequest assetBundleRequest = ao as AssetBundleRequest;
                if(assetBundleRequest!=null)
                {
                    OnAssetLoadCompleted(assetBundleRequest.GetHashCode(), assetBundleRequest.asset);
                }
            };

            return abReq.GetHashCode();
        }
        return 0;
    }

    public void LoadAB(bool bDepend = true)
    {
        if (Bundle == null)
        {
            Bundle = AssetBundle.LoadFromFile(Path);
            if (bDepend)
            {
                bool bAddDepend = m_ListDepend.Count == 0;
                string[] assetbundles = m_ABLoader.GetAllDependencies(ABName);
                for (int i = 0; i < assetbundles.Length; i++)
                {
                    ABInfo abInfo = AssetCacheMgr.Instance.GetABCache(assetbundles[i]);
                    abInfo.LoadAB(false);
                    if (bAddDepend)
                        m_ListDepend.Add(abInfo);
                }
            }
        }
    }
    Action m_ActAllLoaded = null;
    public void LoadABAsync(bool bDepend = true, AssetBundleLoadCompleted assetBundleLoadCompleted = null,Action actAllLoaded = null)
    {
        m_ActAllLoaded = actAllLoaded;
        if (Bundle == null)
        {
            AssetBundleCreateRequest abReq = AssetBundle.LoadFromFileAsync(Path);
            abReq.completed += (ao) =>
            {
                AssetBundleCreateRequest assetBundleCreateRequest = ao as AssetBundleCreateRequest;
                if (assetBundleCreateRequest != null)
                {
                    Bundle = assetBundleCreateRequest.assetBundle;
                }
                OnAssetBundleLoadCompleted(ao.GetHashCode(), Bundle);
                if (assetBundleLoadCompleted != null)
                {
                    assetBundleLoadCompleted.Invoke(ao.GetHashCode(), Bundle);
                }
            }; 
            if (bDepend)
            {
                bool bAddDepend = m_ListDepend.Count == 0;

                string[] assetbundles = m_ABLoader.GetAllDependencies(ABName);
                for (int i = 0; i < assetbundles.Length; i++)
                {
                    ABInfo abInfo = AssetCacheMgr.Instance.GetABCache(assetbundles[i]);
                    abInfo.LoadABAsync(false, OnAssetBundleLoadCompleted);
                    if (bAddDepend)
                        m_ListDepend.Add(abInfo);
                }
            }
        }
    }
    void OnAssetBundleLoadCompleted(int iHandle,AssetBundle assetBundle)
    {

        if (Bundle == null)
        {
            return;
        }
        for (int i = 0; i < m_ListDepend.Count; i++)
        {
            ABInfo abInfo = m_ListDepend[i];
            if (abInfo == null)
                return;
            if (abInfo.Bundle == null)
                return;
        }
        if (m_ActAllLoaded != null)
            m_ActAllLoaded.Invoke();
    }
    void UnLoadAB()
    {
        if (Bundle != null)
            Bundle.Unload(true);
        for (int i = 0; i < m_ListDepend.Count; i++)
        {
            ABInfo abInfo = m_ListDepend[i];
            abInfo.DefRef();
        }
    }

    public override void UnLoad()
    {
        UnLoadAB();

    }
}