using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheManage
{
    float m_fCacheTime = 30.0f;
    public Cache GetCache(string sPath)
    {
        Cache cache;
        if (m_DicCache.TryGetValue(sPath, out cache))
        {
            return cache;
        }
        return null;
    }

    public void AddCacheToDic(string sKey,Cache cache)
    {
        if (!m_DicCache.ContainsKey(sKey))
        {
            m_DicCache.Add(sKey, cache);
        }
    }

    public void Update()
    {
        var node = m_UnUsedCache.First;
        float fTime = UnityEngine.Time.fixedUnscaledTime;
        while (node != null)
        {
            var next = node.Next;
            if (node.Value.IsAllowFree() && fTime - node.Value.LastTime > m_fCacheTime)
            {
                node.Value.UnLoad();
                m_UnUsedCache.Remove(node.Value);
            }
            node = next;
        }
    }

    public void UpdateUseState(Cache cache, bool bUsed)
    {
        cache.LastTime = UnityEngine.Time.fixedUnscaledTime;
        if (bUsed)
        {
            if (cache.GetRefCount() == 0)
            {
                m_UnUsedCache.Remove(cache);
                m_UsedCache.AddLast(cache);
            }
        }
        else
        {
            if (cache.GetRefCount() == 0)
            {
                m_UsedCache.Remove(cache);
                m_UnUsedCache.AddLast(cache);
            }
        }
    }

    public void Destroy()
    {
        m_UsedCache.Clear();
        m_UnUsedCache.Clear();

        var iter = m_DicCache.GetEnumerator();
        while (iter.MoveNext())
        {
            iter.Current.Value.UnLoad();
        }
        m_DicCache.Clear();
    }

    Dictionary<string, Cache> m_DicCache = new Dictionary<string, Cache>();
    LinkedList<Cache> m_UsedCache = new LinkedList<Cache>();
    LinkedList<Cache> m_UnUsedCache = new LinkedList<Cache>();


}
