using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

public class LuaMgr : Singleton<LuaMgr>
{
    private LuaEnv LuaEnv { get { return m_luaEnv; } }
    LuaEnv m_luaEnv = null;
    Dictionary<string, LuaFunction> m_LuaFunction = new Dictionary<string, LuaFunction>();
    static string LuaDir { get; set; }


    public void Init(string sLuaDir)
    {
        LuaDir = sLuaDir;
        m_luaEnv = new LuaEnv();
        m_luaEnv.AddLoader(CustomLoader);
    }

    public void Start()
    {
        DoFile("Main");
        CallGlobalFun("GameStart");
    }

    public void Destroy()
    {
        CallGlobalFun("GameEnd");
        if (m_luaEnv != null)
        {
            m_luaEnv.Dispose();
            m_luaEnv = null;
        }

    }

    public void DoFile(string sFilePath)
    {
        byte[] luaScript = CustomLoader(ref sFilePath);
        if (luaScript != null)
        {
            DoString(luaScript);
        }
    }
    public void DoString(string sContent)
    {
        if (m_luaEnv != null)
        {
            try
            {
                m_luaEnv.DoString(sContent);
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg);
            }
        }
    }
    void DoString(byte[] sContent)
    {
        if (m_luaEnv != null)
        {
            try
            {
                m_luaEnv.DoString(sContent);
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg);
            }
        }
    }

    private object[] CallGlobalFun(string sFunName,params object[] args)
    {
        LuaFunction luaFunction;
        if (!m_LuaFunction.TryGetValue(sFunName, out luaFunction))
        {
            luaFunction = LuaEnv.Global.Get<LuaFunction>(sFunName);
            m_LuaFunction.Add(sFunName, luaFunction);
        }
        if (luaFunction != null)
        {
            return luaFunction.Call(args);
        }
        return null;
    }
    public static byte[] CustomLoader(ref string sFilePath)
    {
        sFilePath = StringUtils.Combine(StringUtils.Relpace(sFilePath, ".", "/"), ".bytes");
        return ResourceMgr.Instance.LoadLua(StringUtils.PathCombine(LuaDir, sFilePath));
    }
}
