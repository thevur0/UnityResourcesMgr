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
        req.completed += (ao) => {
            ResourceRequest resourceRequest = ao as ResourceRequest;
            UnityEngine.Object @object = null;
            if (resourceRequest != null)
                @object = resourceRequest.asset;
            OnAssetLoadCompleted.Invoke(req.GetHashCode(), @object);
        };
        return req.GetHashCode();
    }
}
