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
        Log.StartUp(GameSetting.LogSetting);
        LuaMgr.Instance.Init(GameSetting.DirSetting.LuaDir);

    }
    public void Start()
    {
        LuaMgr.Instance.Start();
    }
    public void Update()
    {
        ResourceMgr.Instance.Update();
    }
    public void FixedUpdate()
    {

    }
    public void LateUpdate()
    {

    }
    public void Destroy()
    {
        ResourceMgr.Instance.DestroyObject(GameSetting);
        ResourceMgr.Instance.Destroy();
        LuaMgr.Instance.Destroy();
        Log.ShutDown();
    }
}
