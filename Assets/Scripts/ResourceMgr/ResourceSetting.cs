using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResourceSetting : ScriptableObject
{
    public enum ResLoadMode
    {
        Resource,
        AssetDatabase,
        AssetBundle,
    }

    public ResourceSetting()
    {

    }
    public string ResourcePath { get { return m_ResourcePath; } }
    public string FileList { get { return m_FileList; } }
    public ResLoadMode ResMode = ResLoadMode.Resource;
    [SerializeField]
    private string m_ResourcePath = @"Assets/ABResources";
    [SerializeField]
    private string m_FileList = @"filelist.json";
}