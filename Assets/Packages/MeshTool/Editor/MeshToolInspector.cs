using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshTool))]
public class MeshToolInspector : Editor
{
    Mesh m_Mesh = null;
    SerializedProperty m_MeshProperty;

    protected void OnEnable()
    {
        
        
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("拆分"))
        {
            MeshTool meshTool = target as MeshTool;
            m_Mesh = meshTool.GetComponent<MeshFilter>().sharedMesh;
            Mesh rMesh = CutMesh1(m_Mesh,(color)=> {
                if (color.r >= color.b && color.r >= color.g && color.r >= color.a)
                    return true;
                else
                    return false;
            });

            Mesh gMesh = CutMesh1(m_Mesh, (color) => {
                if (color.g >= color.r && color.g >= color.b && color.g >= color.a)
                    return true;
                else
                    return false;
            });

            Mesh bMesh = CutMesh1(m_Mesh, (color) => {
                if (color.b >= color.r && color.b >= color.g && color.b >= color.a)
                    return true;
                else
                    return false;
            });

            Mesh aMesh = CutMesh1(m_Mesh, (color) => {
                if (color.a >= color.r && color.a >= color.g && color.a >= color.b)
                    return true;
                else
                    return false;
            });


            //AssetDatabase.CreateAsset(redMesh, "Assets/newmesh1.mesh");
        }

        if (GUILayout.Button("设置顶点色"))
        {
            MeshTool meshTool = target as MeshTool;
            m_Mesh = meshTool.GetComponent<MeshFilter>().sharedMesh;
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

    Mesh CutMesh(Mesh mesh)
    {
        List<Vector3> vector3s = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> triangles = new List<int>();

        mesh.GetVertices(vector3s);
        mesh.GetColors(colors);
        mesh.GetTriangles(triangles, 0);

        Dictionary<int, int> keyValues = new Dictionary<int, int>();
        List<Vector3> newVector = new List<Vector3>();
        int iIndex = 0;
        for (int i = 0; i < colors.Count; i++)
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
        for (int i = 0; i < triangles.Count ; i+=3)
        {
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;
            if (keyValues.TryGetValue(triangles[i], out index1) &&
                keyValues.TryGetValue(triangles[i + 1], out index2) &&
                keyValues.TryGetValue(triangles[i + 2], out index3))
            {
                newTriangles.Add(index1);
                newTriangles.Add(index2);
                newTriangles.Add(index3);
            }
        }
        Mesh newMesh = new Mesh();
        newMesh.SetVertices(newVector);
        newMesh.SetTriangles(newTriangles, 0);
        return newMesh;
    }

    delegate bool ComparaChannel(Color color);

    Mesh CutMesh1(Mesh mesh, ComparaChannel comparaChannel)
    {
        List<Vector3> vector3s = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> triangles = new List<int>();

        mesh.GetVertices(vector3s);
        mesh.GetColors(colors);
        mesh.GetTriangles(triangles, 0);

        Dictionary<int, int> keyValues = new Dictionary<int, int>();
        List<Vector3> newVector = new List<Vector3>();

        HashSet<int> hash1 = new HashSet<int>();
        HashSet<int> hash2 = new HashSet<int>();
        for (int i = 0; i < colors.Count; i++)
        {
            var color = colors[i];
            if (comparaChannel(color))
            {
                hash1.Add(i);
            }
        }
        if (hash1.Count < 3)
            return null;

        for (int i = 0; i < triangles.Count; i+=3)
        {

            if (hash1.Contains(triangles[i]) ||
                hash1.Contains(triangles[i+1]) ||
                hash1.Contains(triangles[i+2]))
            {
                hash2.Add(triangles[i]);
                hash2.Add(triangles[i+1]);
                hash2.Add(triangles[i+2]);
            }
        }

        int iIndex = 0;
        for (int i = 0; i < colors.Count; i++)
        {
            if (hash2.Contains(i))
            {
                newVector.Add(vector3s[i]);
                keyValues.Add(i, iIndex);
                iIndex++;
            }
        }
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < triangles.Count; i += 3)
        {
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;
            if (keyValues.TryGetValue(triangles[i], out index1) &&
                keyValues.TryGetValue(triangles[i + 1], out index2) &&
                keyValues.TryGetValue(triangles[i + 2], out index3))
            {
                newTriangles.Add(index1);
                newTriangles.Add(index2);
                newTriangles.Add(index3);
            }
        }
        Mesh newMesh = new Mesh();
        newMesh.SetVertices(newVector);
        newMesh.SetTriangles(newTriangles, 0);
        return newMesh;
    }
}
