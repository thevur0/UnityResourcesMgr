using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class AssetBundleBuilder
{
    static string m_ResourcePath = @"Assets/ABResources";
    static string m_FileList = @"filelist.json";
    static public string ResourcePath { get { return m_ResourcePath; } }
    static private void BuildAssetBundle(BuildTarget buildTarget)
    {
        string sOutputPath = GetOutputDir(buildTarget);
        if (!Directory.Exists(sOutputPath))
        {
            Directory.CreateDirectory(sOutputPath);
        }

        AssetBundleBuild[] assetBundleBuilds = GetAssetBundleBuilds(m_ResourcePath);
        AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(sOutputPath, assetBundleBuilds, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
        if (assetBundleManifest != null)
        {
            CreateFileList(buildTarget,assetBundleBuilds);
            Debug.Log("Build assetbundle successed.");
        }
        else
        {
            Debug.LogError("Build assetbundle failed.");
        }
    }

    static void CreateFileList(BuildTarget buildTarget, AssetBundleBuild[] assetBundleBuilds)
    {
        string sOutputPath = StringUitls.PathCombine(GetOutputDir(buildTarget), m_FileList);
        Dictionary<string, string> dicFileList = new Dictionary<string, string>();
        string sPlatformDir = CrossPlatform.GetABOutputDir(buildTarget);
        dicFileList.Add(sPlatformDir,sPlatformDir);
        foreach (var abBuild in assetBundleBuilds)
        {
            foreach (var asset in abBuild.assetNames)
            {
                if (!dicFileList.ContainsKey(asset))
                {
                    dicFileList.Add(asset, abBuild.assetBundleName);
                }
            }
        }
        IOUitls.WriteJson(sOutputPath, dicFileList);
    }

    static private AssetBundleBuild[] GetAssetBundleBuilds(string sABResourcesPath)
    {
        string[] sAllGUIDs = AssetDatabase.FindAssets("",new string[] { sABResourcesPath });
        List<AssetBundleBuild> m_listAssetBundleBuild = new List<AssetBundleBuild>();
        foreach (string sGUID in sAllGUIDs)
        {
            string sPath = AssetDatabase.GUIDToAssetPath(sGUID).ToLower();
            string sABName = StringUitls.WithoutExtension(sPath);
            AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
            assetBundleBuild.assetBundleName = sABName;
            assetBundleBuild.assetNames = new string[] { sPath };
            m_listAssetBundleBuild.Add(assetBundleBuild);
        }
        return m_listAssetBundleBuild.ToArray();
    }

    [MenuItem("Tools/AssetBundleBuild/BuildAssetBundleCurPlatform")]
    static public void BuildAssetBundleCurPlatform()
    {
        BuildAssetBundle(GetCurPlatform());
    }
    static public void BuildAssetBundleAndroid()
    {
        BuildAssetBundle(BuildTarget.Android);
    }

    static public void BuildAssetBundleIOS()
    {
        BuildAssetBundle(BuildTarget.iOS);
    }

    static public void BuildAssetBundleWin32()
    {
        BuildAssetBundle(BuildTarget.StandaloneWindows);
    }

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
