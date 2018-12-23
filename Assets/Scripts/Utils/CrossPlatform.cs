using UnityEngine;

public class CrossPlatform
{
    const string sWindowsDir = "Windows";
    const string sAndroidDir = "Android";
    const string sIOSDir = "IOS";

#if UNITY_EDITOR
    static public string GetABOutputDir(UnityEditor.BuildTarget buildTarget)
    {
        string sOutputDir = string.Empty;
        switch (buildTarget)
        {
            case UnityEditor.BuildTarget.StandaloneWindows:
                sOutputDir = sWindowsDir;
                break;
            case UnityEditor.BuildTarget.Android:
                sOutputDir = sAndroidDir;
                break;
            case UnityEditor.BuildTarget.iOS:
                sOutputDir = sIOSDir;
                break;
        }
        return sOutputDir;
    }

    static public UnityEditor.BuildTarget GetCurPlatform()
    {
        UnityEditor.BuildTarget buildTarget = UnityEditor.BuildTarget.StandaloneWindows;
#if UNITY_ANDROID
        buildTarget = UnityEditor.BuildTarget.Android;
#elif UNTIY_IOS
        buildTarget = UnityEditor.BuildTarget.iOS;
#endif
        return buildTarget;
    }
#endif


    static public string GetABDir()
    {
        string sDir = sWindowsDir;
#if UNITY_ANDROID
        sDir = sAndroidDir;
#elif UNTIY_IOS
        sDir = sIOSDir；
#endif
        return sDir;
    }

    static public string GetWWWDir(string sPath)
    {
        if (RuntimePlatform.Android != Application.platform)
        {
            sPath = StringUtils.PathCombine("file://", sPath);
        }
        return sPath;
    }
}
