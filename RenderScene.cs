using System.Collections.Generic;
using UnityEngine;

public class RenderScene : MonoBehaviour
{
    public GameObject RayTracingEngine;
    private GameObject[] spheresInScene;
    private Vector3[] sphereCenters;
    private float[] sphereRadius;
    private Vector3[] sphereColors;
    private int sphereCount;
    private Material sphereMaterial;
    private bool isCheckered = false;
    private List<int> checkeredSphereIndex = new List<int>();

    private void GetSpheresInScene()
    {
        spheresInScene = GameObject.FindGameObjectsWithTag("Sphere");
        sphereCount = spheresInScene.Length;
    }

    private void GetSphereCenters()
    {
        sphereCenters = new Vector3[sphereCount];
        for(int i=0; i<sphereCount; i++)
        {
            sphereCenters[i] = spheresInScene[i].transform.position;
        }
    }

    private void GetSphereRadius()
    {
        sphereRadius = new float[sphereCount];
        for (int i = 0; i < sphereCount; i++)
        {
            sphereRadius[i] = (spheresInScene[i].transform.localScale.x)/2;
        }
    }
     
    private void GetSphereColors()
    {
        sphereColors = new Vector3[sphereCount];
        for (int i = 0; i < sphereCount; i++)
        {
            sphereMaterial = spheresInScene[i].GetComponent<Renderer>().material;
            if(sphereMaterial.name == "Checkered (Instance)")
            {
                sphereColors[i] = new Vector3(0f, 0f, 0f);
                isCheckered = true;
                checkeredSphereIndex.Add(i);
            }
            else if(sphereMaterial.name == "Mirrored (Instance)")
            {
                sphereColors[i] = new Vector3(0f, 0f, 0f);
            }
            else
            {
                sphereColors[i] = new Vector3(sphereMaterial.color.r, sphereMaterial.color.g, sphereMaterial.color.b);
            }
        }
    }

    public bool GetSphereData(List<Vector3> sphereCenterList, List<float> sphereRadiusList, List<Vector3> sphereColourList, List<int> checkeredSphereIndexes)
    {
        for(int i=0; i<sphereCount; i++)
        {
            sphereCenterList.Add(sphereCenters[i]);
            sphereRadiusList.Add(sphereRadius[i]);
            sphereColourList.Add(sphereColors[i]);
        }

        if(isCheckered)
        {
            for (int i = 0; i < checkeredSphereIndex.Count; i++)
            {
                checkeredSphereIndexes.Add(checkeredSphereIndex[i]);
            }
        }

        return isCheckered;
    }

    public void RenderSceneByRayTracing()
    {
        GetSpheresInScene();
        GetSphereCenters();
        GetSphereRadius();
        GetSphereColors();

        if(RayTracingEngine != null)
        {
            RayTracingEngine.SetActive(true);
        }
    }
}

