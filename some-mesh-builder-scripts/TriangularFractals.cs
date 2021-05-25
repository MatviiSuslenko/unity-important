using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangularFractals : MonoBehaviour
{
    public Color color;

    public Shader shader;

    [Range(1, 128)]
    public float size = 10;
    [Range(0, 6)]
    public int iterations = 2;

    public List<Vector3> gizmos = new List<Vector3>();

    private int currentVertice;
    private int currentTriangle;

    Vector3[] vertices;
    int[] triangles;

    public void OnValidate() // когда значение меняется в инспекторе
    {
        gizmos.Clear();
        ConstructFractal();
        PaintDonut();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 1);
        foreach(Vector3 gizmo in gizmos)
        {
            Gizmos.DrawSphere(gizmo, 1f);
        }
    }

    private void ConstructFractal()
    {
        currentVertice = 0;
        currentTriangle = 0;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(shader);

        vertices = new Vector3[5 * (int)Mathf.Pow(5, iterations)];
        triangles = new int[18*(int)Mathf.Pow(5,iterations)];

        Pyramid(1,
            new Vector3(-0.5f * size, 0, 0.5f * size),
            new Vector3(0.5f * size, 0, 0.5f * size),
            new Vector3(0.5f * size, 0, -0.5f * size),
            new Vector3(-0.5f * size, 0, -0.5f * size),
            new Vector3(0, Mathf.Sqrt(2)/2 * size, 0));

        Vector3[] discreteVertices;
        int[] discreteTriangles;

        RemakeMeshToDiscrete(vertices, triangles, out discreteVertices, out discreteTriangles);

        mesh.Clear(); // очищаем данные меша
        mesh.vertices = discreteVertices;
        mesh.triangles = discreteTriangles;
        mesh.RecalculateNormals();
    }

    void RemakeMeshToDiscrete(Vector3[] vert, int[] trig, out Vector3[] outVertices, out int[] outTriangles)
    {
        Vector3[] vertDiscrete = new Vector3[trig.Length];
        int[] trigDiscrete = new int[trig.Length];
        for (int i = 0; i < trig.Length; i++)
        {
            vertDiscrete[i] = vert[trig[i]];
            trigDiscrete[i] = i;
        }
        outVertices = vertDiscrete;
        outTriangles = trigDiscrete;
    }

    

    void Pyramid(int cur, Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e)
    {
        if (cur < iterations)
        {
            Pyramid(cur + 1, a, (a + b) / 2, (a + c + b + d) / 4, (a + d) / 2, (a + e) / 2);
            Pyramid(cur + 1, (a + b) / 2, b, (b + c) / 2, (a + c + b + d) / 4, (b + e) / 2);
            Pyramid(cur + 1, (a + c + b + d) / 4, (b + c) / 2, c, (c + d) / 2, (c + e) / 2);
            Pyramid(cur + 1, (a + d) / 2, (a + c + b + d) / 4, (c + d) / 2, d, (d + e) / 2);
            Pyramid(cur + 1, (a + e) / 2, (b + e) / 2, (c + e) / 2, (d + e) / 2, e);
        }
        else
        {
            vertices[currentVertice] = a;
            vertices[currentVertice + 1] = b;
            vertices[currentVertice + 2] = c;
            vertices[currentVertice + 3] = d;
            vertices[currentVertice + 4] = e;

            triangles[currentTriangle] = currentVertice;
            triangles[currentTriangle + 1] = currentVertice + 2;
            triangles[currentTriangle + 2] = currentVertice + 1;

            triangles[currentTriangle + 3] = currentVertice + 3;
            triangles[currentTriangle + 4] = currentVertice + 2;
            triangles[currentTriangle + 5] = currentVertice;

            triangles[currentTriangle + 6] = currentVertice;
            triangles[currentTriangle + 7] = currentVertice + 1;
            triangles[currentTriangle + 8] = currentVertice + 4;

            triangles[currentTriangle + 9] = currentVertice + 1;
            triangles[currentTriangle + 10] = currentVertice + 2;
            triangles[currentTriangle + 11] = currentVertice + 4;

            triangles[currentTriangle + 12] = currentVertice + 2;
            triangles[currentTriangle + 13] = currentVertice + 3;
            triangles[currentTriangle + 14] = currentVertice + 4;

            triangles[currentTriangle + 15] = currentVertice + 3;
            triangles[currentTriangle + 16] = currentVertice;
            triangles[currentTriangle + 17] = currentVertice + 4;

            currentVertice += 5;
            currentTriangle += 18;

            //gizmos.Add(a);
            //gizmos.Add(b);
            //gizmos.Add(c);
            //gizmos.Add(d);
            //gizmos.Add(e);
        }
    }

    void PaintDonut()
    {
        GetComponent<MeshRenderer>().sharedMaterial.SetColor("_MainColor", color);
    }
}
