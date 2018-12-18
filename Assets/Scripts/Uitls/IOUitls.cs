using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;
public class IOUitls
{
    static public void WriteFile(string sPath, string sContext)
    {
        StreamWriter sw = new StreamWriter(sPath);
        sw.Write(sContext);
        sw.Close();
        sw.Dispose();
    }

    static public string ReadFile(string sPath)
    {
        StreamReader sr = new StreamReader(sPath);
        string ret = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();
        return ret;
    }

    static public string ReadFileWWW(string sPath)
    {
        WWW www = new WWW(sPath);
        if (www.error != null)
        {
            return "";
        }

        while (!www.isDone)
        {
        }

        return www.text;
    }

    static public void WriteJson(string sPath,object obj)
    {
        string str = JsonMapper.ToJson(obj);
        WriteFile(sPath, str);
    }

    static public T ReadJson<T>(string sPath,bool bWWW = false)
    {
        string str = string.Empty;
        if (bWWW)
        {
            str = ReadFileWWW(CrossPlatform.GetWWWDir(sPath));
        }
        else
            str = ReadFile(sPath);
        return JsonMapper.ToObject<T>(str);
    }
}
