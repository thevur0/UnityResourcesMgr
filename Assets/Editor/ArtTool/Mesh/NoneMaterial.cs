using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NoneMaterial
{
    [MenuItem("Assets/替换成None材质")]
    static void ReplaceNoneMaterial()
    {
        Material material = AssetDatabase.LoadAssetAtPath<Material>("Assets/ABResources/Materials/None.mat");
        if (material == null)
            return;
        var guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            string sPath = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter modelImporter = ModelImporter.GetAtPath(sPath) as ModelImporter;
            if (modelImporter == null)
                continue;
            SerializedObject serializedObject = new SerializedObject(modelImporter);

            SerializedProperty spMaterials = serializedObject.FindProperty("m_Materials");
            SerializedProperty spExternalObjects = serializedObject.FindProperty("m_ExternalObjects");

            for (int materialIdx = 0; materialIdx < spMaterials.arraySize; ++materialIdx)
            {
                var id = spMaterials.GetArrayElementAtIndex(materialIdx);
                var name = id.FindPropertyRelative("name").stringValue;
                var type = id.FindPropertyRelative("type").stringValue;
                var assembly = id.FindPropertyRelative("assembly").stringValue;

                SerializedProperty materialProp = null;
                var propertyIdx = 0;

                for (int externalObjectIdx = 0, count = spExternalObjects.arraySize; externalObjectIdx < count; ++externalObjectIdx)
                {
                    var pair = spExternalObjects.GetArrayElementAtIndex(externalObjectIdx);
                    var externalName = pair.FindPropertyRelative("first.name").stringValue;
                    var externalType = pair.FindPropertyRelative("first.type").stringValue;

                    if (externalName == name && externalType == type)
                    {
                        materialProp = pair.FindPropertyRelative("second");
                        materialProp.objectReferenceValue = material;
                        propertyIdx = externalObjectIdx;
                        break;
                    }
                }

                if (materialProp != null)
                {
                    if (materialProp.objectReferenceValue == null)
                    {
                        spExternalObjects.DeleteArrayElementAtIndex(propertyIdx);
                    }
                }
                else
                {
                    var newIndex = spExternalObjects.arraySize++;
                    var pair = spExternalObjects.GetArrayElementAtIndex(newIndex);
                    pair.FindPropertyRelative("first.name").stringValue = name;
                    pair.FindPropertyRelative("first.type").stringValue = type;
                    pair.FindPropertyRelative("first.assembly").stringValue = assembly;
                    pair.FindPropertyRelative("second").objectReferenceValue = material;
                }
            }
            serializedObject.ApplyModifiedProperties();
            modelImporter = serializedObject.targetObject as ModelImporter;
            AssetDatabase.ImportAsset(sPath);
        }
        AssetDatabase.SaveAssets();
    }
}
