using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
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

    static public void WriteJson(string sPath,object obj)
    {
        string str = JsonMapper.ToJson(obj);
        WriteFile(sPath, str);
    }

    static public T ReadJson<T>(string sPath)
    {
        string str = ReadFile(sPath);
        return JsonMapper.ToObject<T>(str);
    }
}
