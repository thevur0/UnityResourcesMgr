using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class StringUitls
{
    static public string WithoutExtension(string sPath, bool bLower = false)
    {
        if(bLower)
            return sPath.Substring(0, sPath.LastIndexOf('.')).ToLower();
        else
            return sPath.Substring(0, sPath.LastIndexOf('.'));
    }

    static public string PathCombine(string a,string b)
    {
        return string.Format("{0}/{1}",a,b);
    }
}

 