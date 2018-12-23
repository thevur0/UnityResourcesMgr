using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class StringUtils
{
    static public string WithoutExtension(string sPath, bool bLower = false)
    {
        UnityEngine.Debug.Log(sPath);
        if(bLower)
            return sPath.Substring(0, sPath.LastIndexOf('.')).ToLower();
        else
            return sPath.Substring(0, sPath.LastIndexOf('.'));
    }

    static public string PathCombine(string a,string b)
    {
        return Format("{0}/{1}",a,b);
    }

    static public string Combine(string a, string b)
    {
        return Format("{0}{1}", a, b);
    }

    static public string FileName(string sPath,bool bExt = true)
    {
        if (bExt)
            return Path.GetFileName(sPath);
        else
            return Path.GetFileNameWithoutExtension(sPath);
    }

    static public string Relpace(string str,string a,string b)
    {
        return str.Replace(a,b);
    }

    static public string Format(string sFormat,params object[] args)
    {
        return string.Format(sFormat, args);
    }
}

 