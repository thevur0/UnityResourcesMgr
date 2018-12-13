using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResAssetLoader : IAssetLoader
{
    public T LoadAsset<T>(string sPath) where T : UnityEngine.Object
    {
        return Resources.Load<T>(StringUitls.WithoutExtension(sPath));
    }
    public int LoadAssetAsync<T>(string sPath, ResourceMgr.AssetLoadCompleted OnAssetLoadCompleted) where T : UnityEngine.Object
    {
        ResourceRequest req = Resources.LoadAsync<T>(StringUitls.WithoutExtension(sPath));
        AsyncOperateMgr.Instance.AddAssetLoad(req, OnAssetLoadCompleted);
        return req.GetHashCode();
    }
}
