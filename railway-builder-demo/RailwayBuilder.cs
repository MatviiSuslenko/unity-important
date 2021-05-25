using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwayBuilder : MonoBehaviour
{
    public float sleeperStep = 2;
    public float railStep = 1;

    public BezierPath path;
    public Material material;
    public Mesh sleeperMesh;
    public RailProfile railProfile;

    public float railVerticalPosition = 0.1f;
    public float distanceBetweenRails = 0.4f;

    public void GenerateMesh()
    {
        List<Vector3> positions;
        List<Quaternion> rotations;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        path.GetPosesByStep(sleeperStep, out positions, out rotations);

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = material;

        // - - - Шпалы - - - //

        for (int i=0; i<positions.Count && i<rotations.Count; i++)
        {
            for (int j=0; j<sleeperMesh.vertices.Length; j++)
            {
                Quaternion q = Quaternion.AngleAxis(90, Vector3.up);
                vertices.Add(rotations[i] * q * sleeperMesh.vertices[j]  + positions[i]);
            }

            for (int j = 0; j < sleeperMesh.triangles.Length; j++)
            {
                triangles.Add(sleeperMesh.triangles[j] + sleeperMesh.vertices.Length*(i));
            }
        }


        // - - - Рельсы - - - //

        path.GetPosesByStep(railStep, out positions, out rotations);

        Vector3 endPosition;
        Quaternion endRotation;
        path.GetEndPose(out endPosition, out endRotation);
        positions.Add(endPosition);
        rotations.Add(endRotation);

        for(int m=-1; m<=1; m += 2)
        {
            for (int i = 0; i < positions.Count && i < rotations.Count; i++)
            {
                for (int j = 0; j < railProfile.points.Count; j++)
                {
                    vertices.Add(rotations[i] *
                                new Vector3(railProfile.points[j].x + m*distanceBetweenRails,
                                            railProfile.points[j].y + railVerticalPosition,
                                            0) +
                                positions[i]);

                    if (i > 0)
                    {
                        triangles.Add(vertices.Count - 1);
                        triangles.Add(vertices.Count - railProfile.points.Count - 1);
                        triangles.Add(vertices.Count - railProfile.points.Count);


                        triangles.Add(vertices.Count - 1);
                        triangles.Add(vertices.Count - 2);
                        triangles.Add(vertices.Count - railProfile.points.Count - 1);
                    }
                }
            }
        }      

        //mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    private void Update()
    {
        GenerateMesh();
    }
}
