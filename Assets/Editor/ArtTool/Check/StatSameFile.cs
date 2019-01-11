using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class StatSameFile : ScriptableWizard
{
    public string m_StatPath = string.Empty;
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
    void OnWizardCreate()// : 点击确定按钮调用此事件
    {

        CheckSameFile();

    }
    public void CheckSameFile()
    {
        string[] files = Directory.GetFiles(m_sAsset, "*.*", SearchOption.AllDirectories);
        Dictionary<string, List<string>> m_Dictionary = new Dictionary<string, List<string>>();
        int iIndex = 0;
        foreach (var item in files)
        {
            EditorUtility.DisplayProgressBar("Read File", item, (float)iIndex / files.Length);
            string sMD5 = MD5Util.GetMD5FromFile(item);
            List<string> fileList;
            if (!m_Dictionary.TryGetValue(sMD5,out fileList))
            {
                fileList = new List<string>();
                m_Dictionary.Add(sMD5, fileList);
            }
            fileList.Add(item);
            iIndex++;
        }
        string sSavePath = Path.Combine(Application.dataPath, "Output.txt");
        StreamWriter sw = new StreamWriter(sSavePath, false);
        var iter = m_Dictionary.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value.Count >= 2)
            {
                sw.WriteLine("MD5:{0}", iter.Current.Key);
                iter.Current.Value.ForEach((item)=>
                {
                    sw.WriteLine("\tFile:{0}",item);
                }
                );
            }
        }
        sw.Close();
        sw.Dispose();
        System.Diagnostics.Process.Start("notepad.exe", sSavePath);
    }

    [MenuItem("Tools/Check/StatSameFile", false)]
    static public void Open()
    {
        ScriptableWizard.DisplayWizard<StatSameFile>("StatSameFile", "Stat");

    }
}
