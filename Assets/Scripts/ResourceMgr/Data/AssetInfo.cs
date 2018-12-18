using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssetInfo<T> : Cache, ILoad<T> where T : UnityEngine.Object
{
    private event ResourceMgr.AssetLoadCompleted AssetLoadCompletedEvent;

    public AssetInfo(string sPath, CacheManage mgr) :base(mgr)
    {
        Path = sPath.ToLower();
    }
    public string Path { get; protected set; }
    private T m_Asset = null;
    public T Asset { get { return m_Asset; }
        protected set
        {
            m_Asset = value;
            if(m_Asset!=null)
                AssetCacheMgr.Instance.AddAsset(m_Asset.GetInstanceID(), this);
        }
    }

    public virtual T Load(IAssetLoader loader)
    {
        if (Asset == null)
        {
            Asset = loader.LoadAsset<T>(Path);
        }
        return Asset;
    }
    public virtual int LoadAsync(IAssetLoader loader, ResourceMgr.AssetLoadCompleted OnAssetLoadCompleted)
    {
        if (Asset == null)
        {
            AssetLoadCompletedEvent += OnAssetLoadCompleted;
            AssetLoadCompletedEvent += (iHandle, @object) => { Asset = @object as T; };
            return loader.LoadAssetAsync<T>(Path, AssetLoadCompletedEvent);
        }
        else
        {
            OnAssetLoadCompleted(0, Asset);
            return 0;
        }
    }

    public override void UnLoad()
    {
        Object.DestroyImmediate(Asset);
        Asset = null;
        AssetLoadCompletedEvent = null;
    }
}