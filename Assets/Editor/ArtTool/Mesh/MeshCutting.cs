using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MeshCutting : ScriptableWizard
{
    public Mesh[] m_Meshs = new Mesh[1];
    public ChannelType m_ChannelA = ChannelType.R;
    public ChannelType m_ChannelB = ChannelType.R;

    public enum ChannelType
    {
        R,
        G,
        B,
        A
    }

    [MenuItem("Tools/Art/Mesh/Mesh切割")]
    static void Open()
    {
        ScriptableWizard.DisplayWizard<MeshCutting>("Mesh切割", "Cut");
    }


    

    void OnWizardCreate()// : 点击确定按钮调用此事件
    {
        foreach (var mesh in m_Meshs)
        {
            Cut(mesh);
        }
        AssetDatabase.Refresh();
    }
    void Cut(Mesh mesh)
    {
    }

    float GetChannel(Color color,ChannelType channel)
    {
        switch (channel)
        {
            case ChannelType.R:
                return color.r;
            case ChannelType.G:
                return color.g;
            case ChannelType.B:
                return color.b;
            case ChannelType.A:
                return color.a;
        }
        return 0;
    }

    void SetChannel(ref Color color, ChannelType channel,float value)
    {
        switch (channel)
        {
            case ChannelType.R:
                color.r = value;
                break;
            case ChannelType.G:
                color.g = value;
                break;
            case ChannelType.B:
                color.b = value;
                break;
            case ChannelType.A:
                color.a = value;
                break;
        }
    }
    List<Mesh> MeshList = new List<Mesh>();
    protected override bool DrawWizardGUI()
    {
        base.DrawWizardGUI();
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Selecet Selection"))
        {
            MeshList.Clear();
            AddSelectAsset();
        }
        if (GUILayout.Button("Add Selection"))
        {
            MeshList.Clear();
            foreach (var mesh in MeshList)
            {
                if (mesh != null)
                {
                    MeshList.Add(mesh);
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
            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(sPath);
            if (mesh != null)
            {
                if(!MeshList.Contains(mesh))
                    MeshList.Add(mesh);
            }
        }

        m_Meshs = MeshList.ToArray();
    }

    private void OnWizardUpdate()// : 当编辑器向导更新时调用
    {
        CheckValid();
    }

    private void OnInspectorUpdate()// : 当编辑器向导更新时调用
    {
        CheckValid();
    }

    void CheckValid()
    {
        if (m_Meshs == null)
        {
            isValid = false;
            errorString = "请放入Mesh";
            return;
        }
        foreach (var mesh in m_Meshs)
        {
            if (mesh == null)
            {
                isValid = false;
                errorString = "请放入Mesh";
                return;
            }
            string sAssetPath = AssetDatabase.GetAssetPath(mesh.GetInstanceID());
        }
        isValid = true;
        errorString = string.Empty;
    }
}
