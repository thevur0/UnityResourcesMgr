using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureChannelSwap : ScriptableWizard
{
    public Texture2D[] m_Textures = new Texture2D[1];
    public ChannelType m_ChannelA = ChannelType.R;
    public ChannelType m_ChannelB = ChannelType.R;

    public enum ChannelType
    {
        R,
        G,
        B,
        A
    }

    [MenuItem("Tools/Art/Texture/通道交换")]
    static void Open()
    {
        ScriptableWizard.DisplayWizard<TextureChannelSwap>("通道交换","Replace");
    }


    

    void OnWizardCreate()// : 点击确定按钮调用此事件
    {
        foreach (var texture in m_Textures)
        {
            Swap(texture,m_ChannelA,m_ChannelB);
        }
        AssetDatabase.Refresh();
    }
    void Swap(Texture2D texture, ChannelType a , ChannelType b)
    {
        if (a == b)
            return;


        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                Color color = texture.GetPixel(i, j);

                float value1 = GetChannel(color, m_ChannelA);
                float value2 = GetChannel(color, m_ChannelB);
                SetChannel(ref color, m_ChannelA, value2);
                SetChannel(ref color, m_ChannelB, value1);
                texture.SetPixel(i,j,color);

            }
        }
        texture.Apply();
        string sAssetPath = AssetDatabase.GetAssetPath(texture.GetInstanceID());
        sAssetPath = Application.dataPath.Substring(0, Application.dataPath.IndexOf("/Assets") + 1) + sAssetPath;
        byte[] bytes = texture.EncodeToPNG();
        string sFileName = sAssetPath;
        File.WriteAllBytes(sFileName, bytes);
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
    List<Texture2D> TextureList = new List<Texture2D>();
    protected override bool DrawWizardGUI()
    {
        base.DrawWizardGUI();
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Selecet Selection"))
        {
            TextureList.Clear();
            AddSelectTexture();
        }
        if (GUILayout.Button("Add Selection"))
        {
            TextureList.Clear();
            foreach (var tex in m_Textures)
            {
                if (tex != null)
                {
                    TextureList.Add(tex);
                }
            }
            AddSelectTexture();
        }
        EditorGUILayout.EndHorizontal();
        return true;
    }
    void AddSelectTexture()
    {
        var guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            string sPath = AssetDatabase.GUIDToAssetPath(guid);
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(sPath);
            if (tex != null)
            {
                if(!TextureList.Contains(tex))
                    TextureList.Add(tex);
            }
        }
        m_Textures = TextureList.ToArray();
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
        if (m_Textures == null)
        {
            isValid = false;
            errorString = "请放入图片";
            return;
        }
        foreach (var texture in m_Textures)
        {
            if (texture == null)
            {
                isValid = false;
                errorString = "请放入图片";
                return;
            }
            string sAssetPath = AssetDatabase.GetAssetPath(texture.GetInstanceID());
            TextureImporter textureImporter = AssetImporter.GetAtPath(sAssetPath) as TextureImporter;
            if (textureImporter.isReadable == false)
            {
                isValid = false;
                errorString = "请设置图片Read/Write Enable.";
                return;
            }
            else if (!texture.format.Equals(TextureFormat.RGBA32))
            {
                isValid = false;
                errorString = "请设置图片RGBA32格式";
                return;
            }
        }
        isValid = true;
        errorString = string.Empty;
    }
}
