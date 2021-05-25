using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SoloBezierTest : MonoBehaviour
{
    public Transform p0;
    public Transform p1;
    public Transform p2;
    public Transform p3;


    [Range(0, 1)]
    public float t;

    void Update()
    { }


    private void OnDrawGizmos()
    {
        int segments = 20;
        Vector3 prevPos = p0.position;

        for (int i = 1; i <= segments; i++)
        {
            float localT = 1f * i / segments;

            Vector3 curPos = Bezier.GetPointOnBezier(p0.position,
                                                    p1.position,
                                                    p2.position,
                                                    p3.position,
                                                    localT);

            Gizmos.DrawLine(prevPos, curPos);

            prevPos = curPos;
        }
    }
}
