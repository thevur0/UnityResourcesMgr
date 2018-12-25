using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
[InitializeOnLoad]
public static class Startup
{
    // Start is called before the first frame update
    static int ms_MaxDrawCall = 0;
    static Startup()
    {
        ClearLog();
        ms_MaxDrawCall = 0;
        EditorApplication.update += Update;
    }

    static void Update()
    {
        int iDrawCall = Mathf.Max(UnityStats.batches, ms_MaxDrawCall);
        if(iDrawCall> ms_MaxDrawCall)
        {
            ms_MaxDrawCall = iDrawCall;
            Debug.LogFormat("Max Batches:{0}",ms_MaxDrawCall);
        }  
    }

    static void ClearLog()
    {
        Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
        Type logEntriesType = unityEditorAssembly.GetType("UnityEditor.LogEntries");
        var LogEntriesGetEntry = logEntriesType.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        LogEntriesGetEntry.Invoke(null, null);
    }
}
