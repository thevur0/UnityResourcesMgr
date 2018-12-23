using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUtils
{
    static public string UnityProjectPath()
    {
        return Application.dataPath.Substring(0, Application.dataPath.IndexOf("/Assets"));
    }
}
