using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donut : MonoBehaviour
{
    [Range(4, 128)]
    public int rotationSegments = 40;
    [Range(4, 128)]
    public int innerRotationSegments = 16;

    [Range(2, 256)]
    public float radius = 4f;
    [Range(1, 128)]
    public float innerRadius = 1f;

    public Color color;
    
    public Shader shader;

    public void OnValidate() // когда значение меняется в инспекторе
    {
        ConstructDonut();
        PaintDonut();
    }

    private void ConstructDonut()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(shader);

        Vector3[] vertices = new Vector3[rotationSegments * innerRotationSegments];
        int[] triangles = new int[rotationSegments * innerRotationSegments * 6];

        int i = 0;
        int triangleIndex = 0;

        for (int a = 0; a < rotationSegments; a++)
        {
            double angleA = a / (double)rotationSegments * 2 * Mathf.PI;
            //print(angleA);

            for (int b = 0; b < innerRotationSegments; b++)
            {
                double angleB = b / (double)innerRotationSegments * 2 * Mathf.PI;
                //print(angleB);

                vertices[i] = new Vector3(
                    radius * Mathf.Cos((float)angleA),
                    0,
                    radius * Mathf.Sin((float)angleA)
                    )

                    +

                    new Vector3(
                    innerRadius * Mathf.Cos((float)angleA) * Mathf.Cos((float)angleB),
                    innerRadius * Mathf.Sin((float)angleB),
                    innerRadius * Mathf.Sin((float)angleA) * Mathf.Cos((float)angleB)
                    );

                if (a < rotationSegments - 1)
                {
                    if (b < innerRotationSegments - 1)
                    {
                        triangles[triangleIndex] = i;
                        triangles[triangleIndex + 1] = i + innerRotationSegments + 1;
                        triangles[triangleIndex + 2] = i + innerRotationSegments;

                        triangles[triangleIndex + 3] = i;
                        triangles[triangleIndex + 4] = i + 1;
                        triangles[triangleIndex + 5] = i + innerRotationSegments + 1;
                    }
                    else
                    {
                        triangles[triangleIndex] = i;
                        triangles[triangleIndex + 1] = i + 1;
                        triangles[triangleIndex + 2] = i + innerRotationSegments;

                        triangles[triangleIndex + 3] = i - innerRotationSegments + 1;
                        triangles[triangleIndex + 4] = i + 1;
                        triangles[triangleIndex + 5] = i;
                    }                 
                }
                else
                {
                    if (b < innerRotationSegments - 1)
                    {
                        triangles[triangleIndex] = i;
                        triangles[triangleIndex + 1] = i - (innerRotationSegments) * (rotationSegments-1) + 1;
                        triangles[triangleIndex + 2] = i - (innerRotationSegments) * (rotationSegments-1);

                        triangles[triangleIndex + 3] = i;
                        triangles[triangleIndex + 4] = i + 1;
                        triangles[triangleIndex + 5] = i - (innerRotationSegments) * (rotationSegments-1) + 1;
                    }
                    else
                    {
                        triangles[triangleIndex] = i;
                        triangles[triangleIndex + 1] = 0;
                        triangles[triangleIndex + 2] = innerRotationSegments - 1;

                        triangles[triangleIndex + 3] = i;
                        triangles[triangleIndex + 4] = i - innerRotationSegments + 1;
                        triangles[triangleIndex + 5] = 0;
                    }
                }
                triangleIndex += 6;

                i++;
            }
        }

        mesh.Clear(); // очищаем данные меша
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void PaintDonut()
    {
        GetComponent<MeshRenderer>().sharedMaterial.SetColor("_MainColor", color);
    }
}
