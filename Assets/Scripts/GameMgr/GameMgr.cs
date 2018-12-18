using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : Singleton<GameMgr>
{
    public GameSetting GameSetting { get; set; } 
    public void Init()
    {
        ResourceMgr.Instance.Init("ResSetting");
        GameSetting = ResourceMgr.Instance.LoadAsset<GameSetting>("GameSetting.asset");
    }
    public void Update()
    {
        ResourceMgr.Instance.Update();
    }
    public void Destroy()
    {
        ResourceMgr.Instance.Destroy();
    }
}
