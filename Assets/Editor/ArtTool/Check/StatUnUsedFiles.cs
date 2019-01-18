using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class StatUnUsedFiles : ScriptableWizard
{
    public string m_StatPath = string.Empty;
    string m_sAsset = string.Empty;
    public SceneAsset[] m_Scenes;
    List<SceneAsset> m_SceneList = new List<SceneAsset>();
    void OnWizardUpdate()// : 当编辑器向导更新时调用
    {
        m_sAsset = Path.Combine(Application.dataPath, m_StatPath);
        helpString = m_sAsset;
        if (Directory.Exists(m_sAsset) && m_Scenes!=null && m_Scenes.Length>0)
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

        CheckUnUsedFile();

    }
    public void CheckUnUsedFile()
    {
        List<string> listpath = new List<string>();
        foreach (var scene in m_Scenes)
        {
            string sPath = AssetDatabase.GetAssetPath(scene);
            listpath.Add(sPath);
        }
        EditorUtility.DisplayProgressBar("GetDependencies","",0);
        string[] sDependencies = AssetDatabase.GetDependencies(listpath.ToArray());
        HashSet<string> setHash = new HashSet<string>(sDependencies);

        List<string> errorlist = new List<string>();
        string[] sGUIDs = AssetDatabase.FindAssets("", new string[] {"Assets/" + m_StatPath });

        for (int i = 0;i< sGUIDs.Length;i++)
        {
            string guid = sGUIDs[i];
            string sAsset = AssetDatabase.GUIDToAssetPath(guid);
            if (AssetDatabase.IsValidFolder(sAsset))
            {
                continue;
            }
            EditorUtility.DisplayProgressBar("Scanning", sAsset, (float)i / sGUIDs.Length);
            if (!setHash.Contains(sAsset) && !errorlist.Contains(sAsset))
            {
                errorlist.Add(sAsset);
            }
        }
        EditorUtility.ClearProgressBar();
        string sSavePath = Path.Combine(Application.dataPath, "Output.txt");
        StreamWriter sw = new StreamWriter(sSavePath, false);
        errorlist.ForEach((item) => { sw.WriteLine("File:{0}", item); });
        sw.Close();
        sw.Dispose();
        System.Diagnostics.Process.Start("notepad.exe", sSavePath);
    }

    [MenuItem("Tools/Check/StatUnUsedFiles", false)]
    static public void Open()
    {
        ScriptableWizard.DisplayWizard<StatUnUsedFiles>("StatUnUsedFiles", "Stat");

    }



    protected override bool DrawWizardGUI()
    {
        base.DrawWizardGUI();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Selecet Selection"))
        {
            m_SceneList.Clear();
            AddSelectAsset();
        }
        if (GUILayout.Button("Add Selection"))
        {
            m_SceneList.Clear();
            foreach (var scene in m_SceneList)
            {
                if (scene != null)
                {
                    m_SceneList.Add(scene);
                }
            }
            AddSelectAsset();
        }
        EditorGUILayout.EndHorizontal();
        return true;
    }
    void AddSelectAsset()
    {
        var guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            string sPath = AssetDatabase.GUIDToAssetPath(guid);
            var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(sPath);
            if (scene != null)
            {
                if (!m_SceneList.Contains(scene))
                    m_SceneList.Add(scene);
            }
        }

        m_Scenes = m_SceneList.ToArray();
    }

}
