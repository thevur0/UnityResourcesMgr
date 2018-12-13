using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAssetLoader
{
    T LoadAsset<T>(string sPath) where T : UnityEngine.Object;
    int LoadAssetAsync<T>(string sPath, ResourceMgr.AssetLoadCompleted OnAssetLoadCompleted) where T : UnityEngine.Object;
}


