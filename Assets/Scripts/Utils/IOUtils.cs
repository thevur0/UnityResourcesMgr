using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;
public class IOUtils
{
    static public void WriteFile(string sPath, string sContext, bool bAppend = false)
    {
        StreamWriter sw = new StreamWriter(sPath, bAppend);
        sw.WriteLine(sContext);
        //sw.Close();
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

    static public byte[] ReadByteFileWWW(string sPath)
    {
        WWW www = new WWW(sPath);
        if (www.error != null)
        {
            return null;
        }

        while (!www.isDone)
        {
        }
        return www.bytes;
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

    static public void CreateDir(string targetPath)
    {
        if (targetPath.IndexOf('.') != -1)
        {
            targetPath = targetPath.Substring(0, targetPath.LastIndexOf('/'));
        }
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }
    }
}
