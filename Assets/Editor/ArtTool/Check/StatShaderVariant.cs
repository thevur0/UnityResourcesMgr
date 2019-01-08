using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
public class StatShaderVariant : ScriptableWizard {


    public string m_StatPath = string.Empty;
    void OnWizardCreate()// : 点击确定按钮调用此事件
    {
        StatShader();
    }
    void FixMaterial()
    {
    }
    void StatShader()
    {
        var method = typeof(ShaderUtil).GetMethod("GetVariantCount", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        
        string[] files = Directory.GetFiles(m_sAsset, "*.shader", SearchOption.AllDirectories);
        List<KeyValuePair<string, ulong>> m_OutputList = new List<KeyValuePair<string, ulong>>();
        int iIndex = 0;
        foreach (var item in files)
        {
            EditorUtility.DisplayProgressBar("Shader", item, (float)iIndex / files.Length);
            string sAssetPath = item.Substring(item.IndexOf("Assets\\"));
            Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(sAssetPath);
            if (shader != null)
            {
                List<string> warninglist = new List<string>();
                object[] param = new object[] {shader,false };
                ulong ulCount = (ulong)method.Invoke(null, param);
                m_OutputList.Add(new KeyValuePair<string, ulong>(sAssetPath, ulCount));
            }
            iIndex++;
        }

        m_OutputList.Sort((item1, item2) =>
        {
            ulong iMax1 = item1.Value;
            ulong iMax2 = item2.Value;
            return (int)(iMax2 - iMax1);
        });
        string sSavePath = Path.Combine(Application.dataPath, "Output.txt");
        StreamWriter sw = new StreamWriter(sSavePath, false);
        m_OutputList.ForEach((item) =>
        {
            sw.WriteLine("File:{0}", item.Key);
            sw.WriteLine("\tVariant:{0}", item.Value.ToString());
            sw.WriteLine("------------------------------------------------------------------------------");
        });
        sw.Close();
        sw.Dispose();
        System.Diagnostics.Process.Start("notepad.exe", sSavePath);
    }
    string m_sAsset = string.Empty;
    void OnWizardUpdate()// : 当编辑器向导更新时调用
    {
        m_sAsset = Path.Combine(Application.dataPath, m_StatPath);
        helpString = m_sAsset;
        if (Directory.Exists(m_sAsset))
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }
    }

    [MenuItem("Tools/StatShaderVariant", false)]
    static public void Open()
    {
        ScriptableWizard.DisplayWizard<StatShaderVariant>("StatShaderVariant", "Stat");
        
    }
}
