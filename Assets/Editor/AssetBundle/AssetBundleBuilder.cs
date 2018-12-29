using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class AssetBundleBuilder
{
    static ResourceSetting ms_ResourceSetting = null;
    static private AssetBundleDirInfo m_ABDirInfo = null;
    static private void BuildAssetBundle(BuildTarget buildTarget)
    {
        var abDirInfo = AssetBundleDirInfo.LoadAssetBundleDirInfo();
        ms_ResourceSetting = Resources.Load<ResourceSetting>("ResSetting");
        string sOutputPath = GetOutputDir(buildTarget);
        if (!Directory.Exists(sOutputPath))
        {
            Directory.CreateDirectory(sOutputPath);
        }

        AssetBundleBuild[] assetBundleBuilds = abDirInfo.GetAssetBundleBuilds(ms_ResourceSetting.ResourcePath);
        AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(sOutputPath, assetBundleBuilds, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
        if (assetBundleManifest != null)
        {
            CreateFileList(buildTarget,assetBundleBuilds);
            AssetDatabase.Refresh();
            Debug.Log("Build assetbundle successed.");
        }
        else
        {
            Debug.LogError("Build assetbundle failed.");
        }
    }

    static void CreateFileList(BuildTarget buildTarget, AssetBundleBuild[] assetBundleBuilds)
    {
        string sOutputPath = StringUtils.PathCombine(GetOutputDir(buildTarget), ms_ResourceSetting.FileList);
        Dictionary<string, string> dicFileList = new Dictionary<string, string>();
        string sPlatformDir = CrossPlatform.GetABOutputDir(buildTarget);
        dicFileList.Add("AssetBundleManifest".ToLower(), sPlatformDir);
        foreach (var abBuild in assetBundleBuilds)
        {
            foreach (var asset in abBuild.addressableNames)
            {
                string sAssetName = asset;
                if (asset.IndexOf(ms_ResourceSetting.ResourcePath.ToLower()) == 0)
                {
                    sAssetName = asset.Substring(ms_ResourceSetting.ResourcePath.Length + 1);
                    sAssetName = asset;
                }
                
                if (!dicFileList.ContainsKey(sAssetName))
                {
                    dicFileList.Add(sAssetName, abBuild.assetBundleName);
                }
            }
        }
        IOUtils.WriteJson(sOutputPath, dicFileList);
    }

    //static private AssetBundleBuild[] GetAssetBundleBuilds(string sABResourcesPath)
    //{
    //    string[] sAllGUIDs = AssetDatabase.FindAssets("",new string[] { sABResourcesPath });
    //    List<AssetBundleBuild> m_listAssetBundleBuild = new List<AssetBundleBuild>();
    //    foreach (string sGUID in sAllGUIDs)
    //    {
            

    //        string sPath = AssetDatabase.GUIDToAssetPath(sGUID).ToLower();
    //        if (AssetDatabase.IsValidFolder(sPath))
    //        {
    //            continue;
    //        }
    //        string sABName = StringUtils.WithoutExtension(sPath);
    //        AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
    //        assetBundleBuild.assetBundleName = sABName;
    //        assetBundleBuild.assetNames = new string[] { sPath };
    //        m_listAssetBundleBuild.Add(assetBundleBuild);
    //    }
    //    return m_listAssetBundleBuild.ToArray();
    //}




    [MenuItem("Tools/AssetBundleBuild/BuildAssetBundleCurPlatform")]
    static public void BuildAssetBundleCurPlatform()
    {
        BuildAssetBundle(GetCurPlatform());
    }
    [MenuItem("Tools/AssetBundleBuild/BuildAssetBundle(Android)")]
    static public void BuildAssetBundleAndroid()
    {
        BuildAssetBundle(BuildTarget.Android);
    }
    [MenuItem("Tools/AssetBundleBuild/BuildAssetBundle(IOS)")]
    static public void BuildAssetBundleIOS()
    {
        BuildAssetBundle(BuildTarget.iOS);
    }
    [MenuItem("Tools/AssetBundleBuild/BuildAssetBundle(Win32)")]
    static public void BuildAssetBundleWin32()
    {
        BuildAssetBundle(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Tools/AssetBundleBuild/BuildAssetBundle(Win64)")]
    static public void BuildAssetBundleWin64()
    {
        BuildAssetBundle(BuildTarget.StandaloneWindows64);
    }
    static public string GetOutputDir(BuildTarget buildTarget)
    {
        string sPlatformDir = CrossPlatform.GetABOutputDir(buildTarget);
        if (string.IsNullOrEmpty(sPlatformDir))
        {
            sPlatformDir = buildTarget.ToString();
        }
        string sOutputDir = string.Format("{0}/{1}", Application.streamingAssetsPath, sPlatformDir);
        return sOutputDir;
    }
    static public BuildTarget GetCurPlatform()
    {
        return CrossPlatform.GetCurPlatform();
    }

}
