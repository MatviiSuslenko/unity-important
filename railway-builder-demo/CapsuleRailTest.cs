using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CapsuleRailTest : MonoBehaviour
{
    public enum MovementMode
    {
        PingPong,
        Repeat
    }

    [Range(0,1)]
    public float percentage;
    public BezierPath rails;

    [Range(1, 100)]
    public float speed;
    private float distanceSum;
    public bool loopMove;

    public MovementMode movementMode;

    public 

    void Update()
    {
        Vector3 position;
        Quaternion rotation;

        if (loopMove)
        {
            distanceSum += Time.deltaTime * speed / 1000f;

            if(movementMode == MovementMode.PingPong)
            {
                percentage = Mathf.PingPong(distanceSum, 1);
            }
            else if (movementMode == MovementMode.Repeat)
            {
                percentage = Mathf.Repeat(distanceSum, 1);
            }
        }

        rails.GetPathPoseByPercentage(percentage, out position, out rotation);

        transform.position = position;
        transform.rotation = rotation;
    }
}
