using UnityEngine;

public class shuye : MonoBehaviour
{
    [Range(3, 64)] public int segments = 12; 
    public float height = 2f;                
    public float radius = 1f;                

    void Start()
    {
        GenerateSolidCone();
    }

    void GenerateSolidCone()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        int vertexCount = segments + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        for (int i = 0; i < segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments;
            vertices[i] = new Vector3(
                radius * Mathf.Cos(angle),
                0,
                radius * Mathf.Sin(angle)
            );
        }
        vertices[segments] = new Vector3(0, 0, 0);
        vertices[segments + 1] = new Vector3(0, height, 0);
        int[] triangles = new int[segments * 3 * 2]; 
        for (int i = 0; i < segments; i++)
        {
            int current = i;
            int next = (i + 1) % segments;
            int tip = segments + 1;

            triangles[i * 3] = current;
            triangles[i * 3 + 1] = tip;
            triangles[i * 3 + 2] = next;
        }
        int baseIndex = segments * 3;
        for (int i = 0; i < segments; i++)
        {
            int current = i;
            int next = (i + 1) % segments;
            int center = segments;

            triangles[baseIndex + i * 3] = current;
            triangles[baseIndex + i * 3 + 1] = next;
            triangles[baseIndex + i * 3 + 2] = center;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

        Material material = new Material(Shader.Find("Standard"));
        material.SetFloat("_Mode", 0); 
        material.renderQueue = 2000;   
        meshRenderer.material = material;
    }
}