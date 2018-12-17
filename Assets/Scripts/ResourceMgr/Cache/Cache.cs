using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ILoad<T> where T: UnityEngine.Object
{
    T Load(IAssetLoader loader);
}
public abstract class Cache
{
    public Cache(CacheManage mgr)
    {
        m_Mgr = mgr;
    }

    public void AddRef()
    {
        if(GetRefCount() == 0)
        {
            m_Mgr.UpdateUseState(this, true);
        }
        m_iRefCount++;
    }
    public void DefRef()
    {
        m_iRefCount--;
        if (m_iRefCount < 0)
        {
            Debug.LogError("m_iRefCount < 0");
            m_iRefCount = 0;
        }
        if (GetRefCount() == 0)
        {
            m_Mgr.UpdateUseState(this, false);
        }
    }
    public int GetRefCount()
    {
        return m_iRefCount;
    }
    public bool IsAllowFree()
    {
        if (GetRefCount() == 0)
            return true;
        return false;
    }
    public abstract void UnLoad();
    int m_iRefCount = 0;
    public float LastTime { get; set; }
    CacheManage m_Mgr = null;
}