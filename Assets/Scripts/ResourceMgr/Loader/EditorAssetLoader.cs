using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorAssetLoader : IAssetLoader
{
    string m_ResourcePath = string.Empty;
    public EditorAssetLoader(string resPath)
    {
        m_ResourcePath = resPath;
    }
    public T LoadAsset<T>(string sPath) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(StringUitls.PathCombine(m_ResourcePath ,sPath));
#else
        return null;
#endif

    }
    public int LoadAssetAsync<T>(string sPath, ResourceMgr.AssetLoadCompleted OnAssetLoadCompleted) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        T t = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(StringUitls.PathCombine(m_ResourcePath, sPath));
        int obj = 0;
        OnAssetLoadCompleted(obj.GetHashCode(), t);
        return obj.GetHashCode();
#else
        return 0;
#endif
    }
}
