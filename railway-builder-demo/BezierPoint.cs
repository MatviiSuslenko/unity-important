using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BezierPoint : MonoBehaviour
{
    public Transform current;
    public Transform left;
    public Transform right;

    private Vector3 leftOldPos;
    private Vector3 rightOldPos;

    private float compareConst = 0.0001f;

    private void Start()
    {
        Update();
        leftOldPos = left.position;
        rightOldPos = right.position;
    }

    void Update()
    {    
        if((left.position - leftOldPos).magnitude > compareConst)
        {
            leftOldPos = left.position;
            rightOldPos = transform.position - (leftOldPos - transform.position);
            right.position = rightOldPos;
        }
        else if ((right.position - rightOldPos).magnitude > compareConst)
        {
            rightOldPos = right.position;
            leftOldPos = transform.position - (rightOldPos - transform.position);
            left.position = leftOldPos;
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.black;
        //Gizmos.DrawLine(left.position, right.position);
        //Gizmos.color = Color.white;
    }
}
