using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class GameSetting :ScriptableObject
{
    public DirSetting DirSetting { get { return m_DirSetting; } }
    [SerializeField]
    DirSetting m_DirSetting = new DirSetting();
}
[Serializable]
public class DirSetting
{
    public string LuaDir { get { return m_LuaDir; } }

    [SerializeField]
    private string m_LuaDir = @"Lua";
}

