using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class MeshTool : MonoBehaviour
{
    Mesh mesh = null;
    private void Start()
    {
        
    }
    private void OnDrawGizmos()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.sharedMesh;
        var vertices = mesh.vertices;
        for (int i = 0;i<mesh.vertexCount;i++)
        {
            var vertex = mesh.vertices[i];
            Color color = Color.white;
            if(i< mesh.colors.Length)
                color = mesh.colors[i];

            Gizmos.color = color;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawSphere(vertex, 0.01f);
        }
    }
}
