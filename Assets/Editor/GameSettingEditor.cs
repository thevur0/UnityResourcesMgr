using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class GameSettingEditor
{
    [MenuItem("Tools/GameSetting/CreateResourceSetting")]
    static void CreateResourceSetting()
    {
        ResourceSetting gameSetting = ScriptableObject.CreateInstance<ResourceSetting>();
        AssetDatabase.CreateAsset(gameSetting,"Assets/Resources/ResSetting.asset");
    }

    [MenuItem("Tools/GameSetting/CreateGameSetting")]
    static void CreateGameSetting()
    {
        ResourceSetting resourceSetting = Resources.Load<ResourceSetting>("ResSetting");
        if (resourceSetting == null)
            return;
        GameSetting gameSetting = ScriptableObject.CreateInstance<GameSetting>();
        string sPath = StringUtils.PathCombine(resourceSetting.ResourcePath, "GameSetting.asset");
        AssetDatabase.CreateAsset(gameSetting, sPath);
        resourceSetting = null;
    }
}
