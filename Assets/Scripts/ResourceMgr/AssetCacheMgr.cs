using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetCacheMgr : Singleton<AssetCacheMgr> {

    public T GetAsset<T>(string sPath) where T : UnityEngine.Object
    {
        T t = default(T);
        return t;
    }
}
