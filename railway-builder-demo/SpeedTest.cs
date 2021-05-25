using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class SpeedTest : MonoBehaviour
{
    private Vector3 prevPos;
    public float curSpeed;

    void Update()
    {
        curSpeed = (prevPos - transform.position).magnitude / Time.deltaTime ;
        prevPos = transform.position;
    }

    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        Handles.Label(transform.position, $"Speed: {curSpeed:N3}", style);
    }
}
