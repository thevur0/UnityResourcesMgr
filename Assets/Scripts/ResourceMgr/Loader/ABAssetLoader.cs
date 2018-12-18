using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABAssetLoader : IAssetLoader {

    AssetBundleManifest m_Manifest = null;
    string m_sFileList = string.Empty;
    Dictionary<string, string> m_FileList = null;
    static ABAssetLoader m_ABLoader = null;
    static public ABAssetLoader Loader { get { return m_ABLoader; } }
    string m_AssetBundlePath = string.Empty;
    public ABAssetLoader(string sFileList)
    {
        m_sFileList = sFileList;
        m_ABLoader = this;
        m_AssetBundlePath = StringUitls.PathCombine(Application.streamingAssetsPath, CrossPlatform.GetABDir());
        LoadFileList();
        LoadAssetBundleManifest();
    }

    private void LoadAssetBundleManifest()
    {
        m_Manifest = LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }
    public string GetAssetBundleFromAsset(string sAsset)
    {
        string sAssetBundle = string.Empty;
        if (m_FileList != null)
        {
            if (m_FileList.TryGetValue(sAsset, out sAssetBundle))
            {
            }
        }
        return sAssetBundle;
    }
    public string[] GetAllDependencies(string sAssetBundleName)
    {
        if (m_Manifest == null)
            return new string[] { };
        else
            return m_Manifest.GetAllDependencies(sAssetBundleName);
    }
    private void LoadFileList()
    {
        m_FileList = IOUitls.ReadJson<Dictionary<string, string>>(StringUitls.PathCombine(m_AssetBundlePath, m_sFileList));
    }

    public T LoadAsset<T>(string sPath) where T : UnityEngine.Object
    {
        ABAssetInfo<T> abAssetInfo = AssetCacheMgr.Instance.GetAssetCache<T>(sPath,this) as ABAssetInfo<T>;
        return abAssetInfo.Load(this);
    }
    public int LoadAssetAsync<T>(string sPath, ResourceMgr.AssetLoadCompleted OnAssetLoadCompleted) where T : UnityEngine.Object
    {
        ABAssetInfo<T> abAssetInfo = AssetCacheMgr.Instance.GetAssetCache<T>(sPath, this) as ABAssetInfo<T>;
        return abAssetInfo.LoadAsync(this, OnAssetLoadCompleted);
    }


}
