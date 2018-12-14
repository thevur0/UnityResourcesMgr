using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABInfo
{
    static ABAssetLoader m_ABLoader = null;
    public string Path { get; protected set; }
    public string ABName { get; protected set; }
    private AssetBundle Bundle { get; set; }
    string m_AssetBundlePath = string.Empty;
    public ABInfo(string sABName)
    {
        m_ABLoader = ABAssetLoader.Loader;
        m_AssetBundlePath = StringUitls.PathCombine(Application.streamingAssetsPath, CrossPlatform.GetABDir());
        ABName = sABName;
        Path = StringUitls.PathCombine(m_AssetBundlePath, sABName);
        
    }
    public T LoadAsset<T>(string sAsset) where T: UnityEngine.Object
    {
        T t = default(T);
        if (Bundle != null)
        {
            t = Bundle.LoadAsset<T>(sAsset);
        }
        return t;
    }
    public void LoadAB(bool bDepend = true)
    {
        if (Bundle == null)
        {
            Bundle = AssetBundle.LoadFromFile(Path);
            if(bDepend)
            {
                string[] assetbundles = m_ABLoader.GetAllDependencies(ABName);
                for (int i = 0; i < assetbundles.Length; i++)
                {
                    ABInfo abInfo = new ABInfo(assetbundles[i]);
                    abInfo.LoadAB(false);
                }
            }
        }
    }
    public void LoadABAsync()
    {
    }
    public void UnLoadAB()
    {
        if (Bundle != null)
            Bundle.Unload(true);
    }
    
}

public class ABAssetInfo<T> where T: UnityEngine.Object
{
    public ABAssetInfo(string sPath)
    {
        m_ABLoader = ABAssetLoader.Loader;
        Path = sPath.ToLower();
    }
    public string Path { get; protected set; }
    public T Asset { get; protected set; }
    static ABAssetLoader m_ABLoader = null;
    public void Load()
    {
        ABInfo abInfo = new ABInfo(m_ABLoader.GetAssetBundleFromAsset(Path));
        abInfo.LoadAB();
        Asset = abInfo.LoadAsset<T>(Path);
    }
    public void LoadAsync()
    {
    }
}

public class ABAssetLoader : IAssetLoader {

    AssetBundleManifest m_Manifest = null;
    Dictionary<string, string> m_FileList = null;
    static ABAssetLoader m_ABLoader = null;
    static public ABAssetLoader Loader { get { return m_ABLoader; } }
    string m_AssetBundlePath = string.Empty;
    public ABAssetLoader()
    {
        m_ABLoader = this;
        m_AssetBundlePath = StringUitls.PathCombine(Application.streamingAssetsPath, CrossPlatform.GetABDir());
        LoadFileList();
        LoadAssetBundleManifest();
    }


    private void LoadAssetBundleManifest()
    {
        ABAssetInfo<AssetBundleManifest> abAssetInfo = new ABAssetInfo<AssetBundleManifest>("AssetBundleManifest");
        abAssetInfo.Load();
        m_Manifest = abAssetInfo.Asset;
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
        m_FileList = IOUitls.ReadJson<Dictionary<string, string>>(StringUitls.PathCombine(m_AssetBundlePath, Main.FileList));
    }

    public T LoadAsset<T>(string sPath) where T : UnityEngine.Object
    {
        ABAssetInfo<T> abAssetInfo = new ABAssetInfo<T>(sPath);
        abAssetInfo.Load();
        return abAssetInfo.Asset;
    }
    public int LoadAssetAsync<T>(string sPath, ResourceMgr.AssetLoadCompleted OnAssetLoadCompleted) where T : UnityEngine.Object
    {
        ABAssetInfo<T> abAssetInfo = new ABAssetInfo<T>(sPath);
        abAssetInfo.Load();
        OnAssetLoadCompleted(0,abAssetInfo.Asset);
        return 0;
    }


}
