using UnityEngine;
using System.Collections.Generic;

public class PlaneGenerator : MonoBehaviour
{
    public float width = 1;
    public float height = 1;
    public Material material;

    public void Start()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        List<Vector3> vertexList = new List<Vector3>();
        List<Vector3> normalList = new List<Vector3>();
        List<Vector2> textureList = new List<Vector2>();
        List<int> indices = new List<int>();
        int index = 0;

        for (int i = 0; i < width / 4; i++)
        {
            vertexList.Add(new Vector3(0, 0, 0));
            vertexList.Add(new Vector3(width / 4, 0, 0));
            vertexList.Add(new Vector3(0, 0, height / 4));
            vertexList.Add(new Vector3(width / 4, 0, height / 4));

            indices.Add(index);
            indices.Add(index + 2);
            indices.Add(index + 1);
            indices.Add(index + 2);
            indices.Add(index + 3);
            indices.Add(index + 1);
            index += 3;

            normalList.Add(-Vector3.forward);
            normalList.Add(-Vector3.forward);
            normalList.Add(-Vector3.forward);
            normalList.Add(-Vector3.forward);

            textureList.Add(new Vector2(0, 0));
            textureList.Add(new Vector2(width / 1, 0));
            textureList.Add(new Vector2(0, height / 1));
            textureList.Add(new Vector2(width / 1, height / 1));
        }

        mesh.vertices = vertexList.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.normals = normalList.ToArray();
        mesh.uv = textureList.ToArray();

        meshFilter.mesh = mesh;
    }
}
