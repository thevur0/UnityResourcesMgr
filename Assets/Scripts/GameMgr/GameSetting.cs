using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class GameSetting :ScriptableObject
{
    public DirSetting DirSetting { get { return m_DirSetting; } }
    public LogSetting LogSetting { get { return m_LogSetting; } }

    [SerializeField]
    DirSetting m_DirSetting = new DirSetting();
    [SerializeField]
    LogSetting m_LogSetting = new LogSetting();
}
[Serializable]
public class DirSetting
{
    public string LuaDir { get { return m_LuaDir; } }

    [SerializeField]
    private string m_LuaDir = @"Lua";
}

[Serializable]
public class LogSetting
{
    public bool WriteFile { get { return m_WriteFile; } }
    public string LogDir { get { return m_LogDir; } }
    [SerializeField]
    private bool m_WriteFile = false;
    [SerializeField]
    private string m_LogDir = @"Logs";
}

