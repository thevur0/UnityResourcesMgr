using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABInfo
{
    static ABAssetLoader m_ABLoader = null;
    public string Path { get; protected set; }
    private AssetBundle Bundle { get; set; }
    string m_AssetBundlePath = string.Empty;
    public ABInfo(string sPath)
    {
        m_ABLoader = ABAssetLoader.Loader;
        m_AssetBundlePath = StringUitls.PathCombine(Application.streamingAssetsPath, CrossPlatform.GetABDir());
        Path = StringUitls.PathCombine(m_AssetBundlePath,sPath);
        
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
    public void LoadAB()
    {
        if (Bundle == null)
        {
            Bundle = AssetBundle.LoadFromFile(Path);

            string[] assetbundles = m_ABLoader.GetAllDependencies(Path);
            for (int i = 0; i < assetbundles.Length; i++)
            {
                ABInfo abInfo = new ABInfo(assetbundles[i]);
                abInfo.LoadAB();
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

public class ABAssetInfo<T>
{
    public ABAssetInfo(string sPath)
    {
        m_ABLoader = ABAssetLoader.Loader;
        Path = sPath;
    }
    public string Path { get; protected set; }
    public T Asset { get; protected set; }
    static ABAssetLoader m_ABLoader = null;
    public T Load()
    {
        T t = default(T);
        ABInfo abInfo = new ABInfo(m_ABLoader.GetAssetBundleFromAsset(Path));
        abInfo.LoadAB();
        return t;
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

        ABAssetInfo<AssetBundleManifest> abAssetInfo = new ABAssetInfo<AssetBundleManifest>(CrossPlatform.GetABDir());
        abAssetInfo.Load();
        m_Manifest = abAssetInfo.Asset;
        //ABInfo abInfo = new ABInfo();
        //var bundle = AssetBundle.LoadFromFile();
        //m_Manifest = bundle.LoadAllAssets<AssetBundleManifest>();


        //bundle.Unload(false);
        //bundle = null;
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
        return m_Manifest.GetAllDependencies(sAssetBundleName);
    }
    private void LoadFileList()
    {
        m_FileList = IOUitls.ReadJson<Dictionary<string, string>>(StringUitls.PathCombine(m_AssetBundlePath, Main.FileList));
    }

    public T LoadAsset<T>(string sPath) where T : UnityEngine.Object
    {
        return Resources.Load<T>(sPath);
    }
    public int LoadAssetAsync<T>(string sPath, ResourceMgr.AssetLoadCompleted OnAssetLoadCompleted) where T : UnityEngine.Object
    {
        return 0;
    }


}
