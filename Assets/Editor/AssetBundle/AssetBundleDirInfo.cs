using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
public class AssetBundleDirInfo
{
    public List<string> ABCombineDir { get; set; }
    public List<string> ABCombineSubDir { get; set; }
    public List<string> ABSeparateDir { get; set; }
    public List<string> ABIgnoreDir { get; set; }

    private int GetABCombineSubDirIndex(string sPath, out string sABName)
    {
        int iCombineSubDirMax = 0;
        sABName = string.Empty;
        for (int i = 0; i < ABCombineSubDir.Count; i++)
        {
            string str = ABCombineSubDir[i];
            if (IsContainPath(sPath, str))
            {
                int iPos = sPath.IndexOf('/', str.Length + 1);
                if (iPos >= 0)
                {
                    iCombineSubDirMax = iPos;
                    sABName = sPath.Substring(0, iPos);
                }
            }
        }
        return iCombineSubDirMax;
    }
    private int GetABCombineDirIndex(string sPath, out string sABName)
    {
        int iCombineDirMax = 0;
        sABName = string.Empty;
        for (int i = 0; i < ABCombineDir.Count; i++)
        {
            string str = ABCombineDir[i];
            if (IsContainPath(sPath, str))
            {
                iCombineDirMax = str.Length;
                sABName = str;
            }
        }
        return iCombineDirMax;
    }

    private int GetABSeparateDirIndex(string sPath)
    {
        int iSeparateDirMax = 0;
        for (int i = 0; i < ABSeparateDir.Count; i++)
        {
            string str = ABSeparateDir[i];
            if (IsContainPath(sPath, str))
            {
                iSeparateDirMax = str.Length;
            }
        }
        return iSeparateDirMax;
    }

    private bool IsABIgnoreDir(string sPath)
    {
        for (int i = 0; i < ABIgnoreDir.Count; i++)
        {
            if (IsContainPath(sPath, ABIgnoreDir[i]))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsContainPath(string sPathA, string sPathB)
    {
        if (sPathA.IndexOf(sPathB.ToLower()) == 0)
        {
            if (sPathB.Length < sPathA.Length)
            {
                if (sPathA[sPathB.Length].Equals('/'))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }
    public string GetAssetBundleName(string sPath)
    {
        string sABName = string.Empty;
        if (IsABIgnoreDir(sPath))
            return string.Empty;
        string sCDir = string.Empty;
        string sCSDir = string.Empty;
        int iCombineDirMax = GetABCombineDirIndex(sPath, out sCDir);
        int iCombineSubDirMax = GetABCombineSubDirIndex(sPath, out sCSDir);
        int iCurDirMax = iCombineDirMax > iCombineSubDirMax ? iCombineDirMax : iCombineSubDirMax;
        sABName = iCombineDirMax > iCombineSubDirMax ? sCDir : sCSDir;
        int iSeparateDirMax = GetABSeparateDirIndex(sPath);
        if (iSeparateDirMax >= iCurDirMax)
        {
            sABName = sPath;
        }
        return sABName;
    }



    public AssetBundleBuild[] GetAssetBundleBuilds(string sABResourcesPath)
    {
        Dictionary<string, AssetBundleBuild> mapABBuild = new Dictionary<string, AssetBundleBuild>();
        string[] sAllGUIDs = AssetDatabase.FindAssets("", new string[] { sABResourcesPath });
        foreach (string sGUID in sAllGUIDs)
        {
            string sPath = AssetDatabase.GUIDToAssetPath(sGUID).ToLower();
            if (AssetDatabase.IsValidFolder(sPath))
            {
                continue;
            }

            string sABName = GetAssetBundleName(sPath);
            AssetBundleBuild abBuild = default(AssetBundleBuild);
            ABNameTrans(ref sABName,sABResourcesPath);
            if (mapABBuild.TryGetValue(sABName, out abBuild))
            {
                List<string> tempStrList = new List<string>(abBuild.assetNames);
                tempStrList.Add(sPath);
                abBuild.assetNames = tempStrList.ToArray();
                mapABBuild[sABName] = abBuild;
            }
            else
            {
                abBuild.assetBundleName = sABName;
                abBuild.assetNames = new string[] { sPath };
                mapABBuild.Add(sABName, abBuild);
            }
        }
        AssetBundleBuild[] abBuilds = new AssetBundleBuild[mapABBuild.Values.Count];
        mapABBuild.Values.CopyTo(abBuilds, 0);
        return abBuilds;
    }

    void ABNameTrans(ref string sABName,string sResourcesPath,bool bDirType = false)
    {
        if (!bDirType)
        {
            sABName = sABName.Substring(sResourcesPath.Length + 1);
            sABName = sABName.Replace('/', '$');
        }
        sABName = StringUtils.Format("{0}.ab", sABName.ToLower());
    }

    static public AssetBundleDirInfo LoadAssetBundleDirInfo()
    {
        string sPath = Application.dataPath + "/ABDirInfo.json";
        return IOUtils.ReadJson<AssetBundleDirInfo>(sPath);
    }
}