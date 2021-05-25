using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager2 : MonoBehaviour
{
    public static WaveManager2 instance;

    public Material water;
    public float amplitude = 1f;
    public float length = 2f;
    public float speed = 1f;
    public float offset = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Wave manager 2 already exists, destroying it...");
            Destroy(this);
        }
    }

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }

    public float GetWaveHeight(float x)
    {
        Texture tex = water.GetTexture("");
        return amplitude * Mathf.Sin(x / length + offset);
    }
}
