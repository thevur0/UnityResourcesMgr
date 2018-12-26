using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class TextureTool : MonoBehaviour
{
    [MenuItem("Assets/Save PNG")]
    static void SavePNG()
    {
        var sels = Selection.objects;
        foreach (var obj in sels)
        {
            Texture2D tex = obj as Texture2D;
            if (tex != null)
            {
                var bytes = tex.EncodeToPNG();
                if(bytes!=null)
                {
                    string sAssetPath = AssetDatabase.GetAssetPath(tex);
                    string sFilePath = StringUtils.Combine(StringUtils.PathCombine(UnityUtils.UnityProjectPath(), sAssetPath), ".png");
                    File.WriteAllBytes(sFilePath, bytes);
                }
                
            }
        }
        AssetDatabase.Refresh();
    }
}
