using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshTool))]
public class MeshToolInspector : Editor
{
    Mesh m_Mesh = null;
    SerializedProperty m_MeshProperty;
    Vector3[] m_Vectors;
    int[] m_Indices;
    int[] m_Triangles;
    protected void OnEnable()
    {
        MeshTool meshTool = target as MeshTool;
        m_Mesh = meshTool.GetComponent<MeshFilter>().sharedMesh;
        m_Vectors = m_Mesh.vertices;
        m_Triangles = m_Mesh.triangles;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("拆分"))
        {
            MeshTool meshTool = target as MeshTool;
            m_Mesh = meshTool.GetComponent<MeshFilter>().sharedMesh;

            List<Vector3> vector3s = new List<Vector3>();
            List<Color> colors = new List<Color>();
            m_Mesh.GetVertices(vector3s);
            m_Mesh.GetColors(colors);


            Dictionary<int, int> keyValues = new Dictionary<int, int>();
            List<Vector3> newVector = new List<Vector3>();
            int iIndex = 0;
            for (int i = 0;i< colors.Count;i++)
            {
                var color = colors[i];
                if (color.r > color.b)
                {
                    newVector.Add(vector3s[i]);
                    keyValues.Add(i, iIndex);
                    iIndex++;
                }
            }
            List<int> newTriangles = new List<int>();
            for (int i = 0;i< m_Triangles.Length/3;i++)
            {
                int index1 = 0;
                int index2 = 0;
                int index3 = 0;
                if (keyValues.TryGetValue(m_Triangles[i],out index1) && 
                    keyValues.TryGetValue(m_Triangles[i + 1], out index2) && 
                    keyValues.TryGetValue(m_Triangles[i + 2], out index3))
                {
                    newTriangles.Add(index1);
                    newTriangles.Add(index2);
                    newTriangles.Add(index3);
                }
            }
            Mesh newMesh = new Mesh();
            newMesh.SetVertices(newVector);
            newMesh.SetTriangles(newTriangles,0);
            AssetDatabase.CreateAsset(newMesh, "Assets/newmesh1.mesh");
        }

        if (GUILayout.Button("设置顶点色"))
        {
            List<Vector3> vector3s = new List<Vector3>();
            Mesh newMesh = Object.Instantiate(m_Mesh);
            newMesh.GetVertices(vector3s);
            List<Color> colors = new List<Color>();
            vector3s.ForEach((item) => {
                Color color = Color.white;
                if (item.y > 0)
                {
                    color = Color.red;
                }
                else
                {
                    color = Color.blue;
                }
                colors.Add(color);
            }
            );
            newMesh.SetColors(colors);
            AssetDatabase.CreateAsset(newMesh, "Assets/newmesh.mesh");
        }
    }
}
