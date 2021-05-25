using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class BezierPath : MonoBehaviour
{
    private class Segment
    {
        private Vector3 sp0;
        private Vector3 sp1;
        private Vector3 sp2;
        private Vector3 sp3;

        private Vector3[] points;
        public Vector3[] Points { get { return points; } }

        private float distance;
        public float Distance { get { return distance; } }

        public Segment(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float step)
        {
            sp0 = p0;
            sp1 = p1;
            sp2 = p2;
            sp3 = p3;

            int tempSegments = 1000;
            Vector3 tempPrevPos = p0;

            for (int i = 1; i <= tempSegments; i++)
            {
                float localT = 1f * i / (float)tempSegments;

                Vector3 curPos = Bezier.GetPointOnBezier(sp0, sp1, sp2, sp3, localT);

                distance += (tempPrevPos - curPos).magnitude;

                tempPrevPos = curPos;
            }

            int _segments = (int)Mathf.Ceil(distance/step);
            distance = 0;

            points = new Vector3[_segments + 1];
            points[0] = p0;

            Vector3 prevPos = p0;

            for (int i = 1; i <= _segments; i++)
            {
                float localT = 1f * i / (float)_segments;

                Vector3 curPos = Bezier.GetPointOnBezier(sp0, sp1, sp2, sp3, localT);
                points[i] = curPos;

                distance += (prevPos - curPos).magnitude;

                prevPos = curPos;
            }
        }

        public void GetPathPose(float distanceOnSegment, out Vector3 position, out Quaternion rotation)
        {
            float localT = distanceOnSegment / distance;
            position = Bezier.GetPointOnBezier(sp0, sp1, sp2, sp3, localT);
            rotation = Bezier.GetHorizontalRotation(sp0, sp1, sp2, sp3, localT);
        }

    }

    public List<BezierPoint> points;
    private Segment[] segments = new Segment[0];

    private float pathDistance;
    public float PathDistance { get { return pathDistance; } }

    [Range(0.2f, 100)]
    public float step;

    private void Update()
    {
        CreatePath();
    }

    public void GetPosesByStep(float tempStep, out List<Vector3> positions, out List<Quaternion> rotations)
    {
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();

        if (segments.Length > 0)
        {
            Vector3 tempPosition;
            Quaternion tempRotation;

            for(float a=0; a<pathDistance; a += tempStep)
            {
                GetPathPoseByDistance(a, out tempPosition, out tempRotation);
                positions.Add(tempPosition);
                rotations.Add(tempRotation);
            }
        }
    }

    public void GetPathPoseByPercentage(float persentageOnPath, out Vector3 position, out Quaternion rotation)
    {
        GetPathPoseByDistance(persentageOnPath * pathDistance, out position, out rotation);
    }

    public void GetPathPoseByDistance(float distanceOnPath, out Vector3 position, out Quaternion rotation)
    {
        if (distanceOnPath < 0)
        {
            distanceOnPath = 0;
        }

        if (distanceOnPath > pathDistance)
        {
            distanceOnPath = pathDistance;
        }

        if (segments == null)
        {
            position = transform.position;
            rotation = transform.rotation;
            return;
        }

        if (segments.Length > 0)
        {
            if(segments.Length > 1)
            {
                float tempDistance = distanceOnPath;

                for (int i = 0; i<segments.Length; i++)
                {
                    if (tempDistance - segments[i].Distance < Constants.compareConst)
                    {
                        segments[i].GetPathPose(tempDistance, out position, out rotation);
                        return;
                    }

                    tempDistance -= segments[i].Distance;
                }

                segments[segments.Length].GetPathPose(segments[segments.Length].Distance, out position, out rotation);
            }
            else
            {
                segments[0].GetPathPose(distanceOnPath, out position, out rotation);
            }            
        }
        else
        {
            position = transform.position;
            rotation = transform.rotation;
        }
    }

    public void GetStartPose(out Vector3 position, out Quaternion rotation)
    {
        GetPathPoseByPercentage(0, out position, out rotation);
    }

    public void GetEndPose(out Vector3 position, out Quaternion rotation)
    {
        GetPathPoseByPercentage(1, out position, out rotation);
    }

    void CreatePath()
    {
        if (points.Count > 1)
        {
            float tempDistance = 0;
            Segment[] tempSegments = new Segment[points.Count - 1];

            for (int i = 0; i < points.Count - 1; i++)
            {
                tempSegments[i] = new Segment(points[i].current.position,
                                                points[i].right.position,
                                                points[i + 1].left.position,
                                                points[i + 1].current.position,
                                                step);

                tempDistance += tempSegments[i].Distance;
            }

            segments = tempSegments;
            pathDistance = tempDistance;
        }
        else
        {
            segments = null;
            pathDistance = 0;
        }
    }

    void OnDrawGizmos()
    {
        if (segments != null)
        {
            if (segments.Length > 0)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.black;

                for (int i = 0; i < segments.Length; i++)
                {
                    Gizmos.color = (i%2==0)?Color.white : Color.yellow;

                    if (segments[i].Points != null)
                    {
                        for (int j = 0; j < segments[i].Points.Length - 1; j++)
                        {
                            Gizmos.DrawLine(segments[i].Points[j], segments[i].Points[j + 1]);
                        }
                    }                  

                    Vector3 curLabelPos = Bezier.GetPointOnBezier(points[i].current.position,
                                                            points[i].right.position,
                                                            points[i + 1].left.position,
                                                            points[i + 1].current.position,
                                                            0.5f);

                    Handles.Label(curLabelPos, $"№{i + 1} ({segments[i].Distance:N3})", style);

                }
                Gizmos.color = Color.white;

                style.normal.textColor = Color.blue;
                Handles.Label(transform.position, $"Distance: {pathDistance:N3}", style);
            }
        }
    }
}
