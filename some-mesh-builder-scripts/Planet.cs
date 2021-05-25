using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2,256)]
    public int resolution = 10;
    [Range(0, 1)]
    public float roundness = 1;
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    ShapeGenerator shapeGenerator;

    public Shader shader;

    public void OnValidate() // когда значение меняется в инспекторе
    {
        GeneratePlanet();
    }

    void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);

        if(meshFilters==null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i=0; i<6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject($"Mesh {i + 1}");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(shader);
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }    

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, roundness, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        Initialize();
        GenerateMesh();
    }

    public void OnColorSettingsUpdated()
    {
        Initialize();
        GenerateColors();
    }

    void GenerateMesh()
    {
        foreach(TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    void GenerateColors()
    {
        foreach(MeshFilter filter in meshFilters)
        {
            filter.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_MainColor", colorSettings.planetColor);
        }
    }
}
