using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUtils
{
    static public string UnityProjectPath()
    {
        return Application.dataPath.Substring(0, Application.dataPath.IndexOf("/Assets"));
    }

    public static string ProjectName
    {
        get
        {
            //在这里分析shell传入的参数， 还记得上面我们说的哪个 project-$1 这个参数吗？
            //这里遍历所有参数，找到 project开头的参数， 然后把-符号 后面的字符串返回，
            //这个字符串就是 91 了。。
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("project"))
                {
                    return arg.Split("-"[0])[1];
                }
            }
            return "test";
        }
    }
}
