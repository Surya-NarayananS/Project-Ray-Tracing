using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RayTracing : MonoBehaviour
{
    public UVMapper uvMapper;
    public ExecutePythonScript executePyScript;
    public RenderScene renderScene;
    public ComputationUI computationScreenUI;

    private int width = 1920;
    private int height = 1080;
    private Vector3 camPosition;
    private float screenRatio;
    private float[] screen;
    private float[,,] image;
    private Vector3 pixel;
    private Vector3 illumination;

    string docPath = Directory.GetCurrentDirectory();

    Vector3 origin;
    Vector3 camToPixelUnitVector;
    Vector3 intersection;
    Vector3 intersectionToLight;
    Vector3 normalToSurface;
    Vector3 shiftedPosition;
    Vector3 IntersectionToCamera;
    Vector3 temporaryVector;
    Vector3 lightIllumination;
    Vector3 color;
    float reflection;
    int? nearestObjectIndex;
    float minDistance;
    int? nearestLightObstructingObject;
    float intersectionToLightDistance;
    bool is_Shadowed;
    bool is_Checkered = false;

    List<Vector3> sphereCenters = new List<Vector3>();
    List<float> SphereRadius = new List<float>();
    List<Vector3> SphereAmbient = new List<Vector3>();
    List<Vector3> SphereDiffuse = new List<Vector3>();
    List<Vector3> SphereSpecular = new List<Vector3>();
    List<int> SphereShininess = new List<int>();
    List<float> SphereReflection = new List<float>();

    List<int> CheckeredSphereIndexes = new List<int>();

    private void Start()
    {
        computationScreenUI.TurnOnComputingIllumination();
        Canvas.ForceUpdateCanvases();
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Resolution.txt")))
        {
            outputFile.Write(width);
            outputFile.Write(",");
            outputFile.Write(height);
        }
        StartCoroutine(WaitAndExecute(0.000001f));
    }

    private IEnumerator WaitAndExecute(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        RayTrace();
    }

    private void RayTrace()
    {
        

        screenRatio = width /(float) height;
        camPosition = new Vector3(0, 0, -1);
        screen = new float[] {-1f, 1f/screenRatio, 1f, -1f/screenRatio};
        image = new float[height, width, 3];

        is_Checkered = renderScene.GetSphereData(sphereCenters, SphereRadius, SphereAmbient, CheckeredSphereIndexes);
        sphereDiffuseData(SphereDiffuse);
        sphereSpecularShininess(SphereSpecular, SphereShininess);
        sphereReflectionData(SphereReflection);
        CreatePlane();

        List<Vector3> lightPosition = new List<Vector3>();
        lightPosition.Add(new Vector3(5, 5, -5));
        lightPosition.Add(new Vector3(-5, 5, -5));

        List<Vector3> lightAmbient = new List<Vector3>();
        lightAmbient.Add(new Vector3(0.5f, 0.5f, 0.5f));
        lightAmbient.Add(new Vector3(0.5f, 0.5f, 0.5f));

        List<Vector3> lightDiffuse = new List<Vector3>();
        lightDiffuse.Add(new Vector3(0.5f, 0.5f, 0.5f));
        lightDiffuse.Add(new Vector3(0.5f, 0.5f, 0.5f));

        List<Vector3> lightSpecular = new List<Vector3>();
        lightSpecular.Add(new Vector3(0.8f, 0.8f, 0.8f));
        lightSpecular.Add(new Vector3(0.8f, 0.8f, 0.8f));

        for (int i=0; i<height; i++)
        {
            for(int j=0; j<width; j++)
            {
                for(int k=0; k<3; k++)
                {
                    image[i, j, k] = 0f;
                }
            }
        }

        float[] horizontalPixels = new float[width];
        horizontalPixels = linspace(screen[0], screen[2], width);

        float[] verticalPixels = new float[height];
        verticalPixels = linspace(screen[1], screen[3], height);

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Render.txt")))
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pixel = new Vector3(horizontalPixels[j], verticalPixels[i], 0f);
                    camToPixelUnitVector = (pixel - camPosition).normalized;
                    origin = camPosition;
                    reflection = 1f;
                    color = new Vector3(0f, 0f, 0f);
                    is_Shadowed = false;

                    for (int a=0; a<3; a++)
                    {
                        (nearestObjectIndex, minDistance) = NearestIntersectedObjectIndex(camToPixelUnitVector, origin, SphereRadius, sphereCenters);

                        if (nearestObjectIndex == null)
                        {
                            break;
                        }

                        intersection = origin + (minDistance * camToPixelUnitVector);

                        normalToSurface = (intersection - sphereCenters[(int)nearestObjectIndex]).normalized;
                        shiftedPosition = intersection + 0.00001f * normalToSurface;

                        lightIllumination = new Vector3(0f, 0f, 0f); //Additive Identity (0)

                        for (int lightIndex = 0; lightIndex < lightPosition.Count; lightIndex++)
                        {
                            is_Shadowed = false;
                            intersectionToLight = (lightPosition[lightIndex] - shiftedPosition).normalized;
                            intersectionToLightDistance = (lightPosition[lightIndex] - intersection).magnitude;

                            (nearestLightObstructingObject, minDistance) = NearestIntersectedObjectIndex(intersectionToLight, shiftedPosition, SphereRadius, sphereCenters);
                            if (minDistance < intersectionToLightDistance)
                            {
                                is_Shadowed = true;
                            }

                            if (is_Shadowed)
                            {
                                continue;
                            }

                            illumination = new Vector3(0, 0, 0);
                            if(is_Checkered)
                            {
                                int checkeredCount = CheckeredSphereIndexes.Count;
                                for(int alpha=0; alpha<checkeredCount; alpha++)
                                {
                                    int currentIndex = CheckeredSphereIndexes[alpha];
                                    SphereAmbient[currentIndex] = uvMapper.UVMap((sphereCenters[currentIndex] - intersection).normalized);
                                }
                            }
                            
                            illumination += Vector3.Scale(SphereAmbient[(int)nearestObjectIndex], lightAmbient[lightIndex]);
                            illumination += Vector3.Scale(SphereDiffuse[(int)nearestObjectIndex], lightDiffuse[lightIndex]) * Vector3.Dot(intersectionToLight, normalToSurface);
                            IntersectionToCamera = (camPosition - intersection).normalized;
                            temporaryVector = (intersectionToLight + IntersectionToCamera).normalized;
                            illumination += Vector3.Scale(SphereSpecular[(int)nearestObjectIndex], lightSpecular[lightIndex]) * Mathf.Pow(Vector3.Dot(normalToSurface, temporaryVector), SphereShininess[(int)nearestObjectIndex] / 4);
                            lightIllumination += illumination;
                        }
                        color += (lightIllumination * reflection);
                        reflection *= SphereReflection[(int)nearestObjectIndex];

                        origin = shiftedPosition;
                        camToPixelUnitVector = ReflectedVector(camToPixelUnitVector, normalToSurface);
                    }

                    for (int k=0; k<3; k++)
                    {
                        image[i, j, k] = Mathf.Clamp(color[k], 0, 1);
                        outputFile.Write(image[i, j, k]);
                        outputFile.Write(",");
                    }
                }
            }
        }

        computationScreenUI.TurnOnComputationCompleted();
        computationScreenUI.TurnOnInitiatingSceneRender();
        executePyScript.RenderAndOpenImage();
    }

    public float[] linspace(double start, double end, int num)
    {
        double[] sampleArray = new double[num];
        float[] linspaceArray = new float[num];
        double difference = end - start;
        double step = difference / (num - 1);

        for(int i=0; i<num; i++)
        {
            sampleArray[i] = start;
            start += step;
        }

        for(int i=0; i<num; i++)
        {
            linspaceArray[i] = (float)sampleArray[i];
        }

        return linspaceArray;
    }

    public float? SphereIntersection(float sphereRadius, Vector3 sphereCenter, Vector3 rayDirection, Vector3 rayOrigin)
    {
        float b = 2 * (Vector3.Dot(rayDirection, (rayOrigin - sphereCenter)));
        float c = Mathf.Pow(Vector3.Magnitude((rayOrigin - sphereCenter)), 2) - Mathf.Pow(sphereRadius, 2);
        float discriminant = Mathf.Pow(b, 2) - (4*c);
        if(discriminant > 0)
        {
            float t1 = (-b + Mathf.Sqrt(discriminant)) / 2;
            float t2 = (-b - Mathf.Sqrt(discriminant)) / 2;
            
            if(t1>0 && t2>0)
            {
                return Mathf.Min(t1, t2);
            }
        }
        return null;
    }

    public (int?, float) NearestIntersectedObjectIndex(Vector3 rayDirection, Vector3 rayOrigin, List<float> sphereRadius, List<Vector3> sphereCenter)
    {
        List<float?> distances = new List<float?>();
        for(int i=0; i<sphereCenter.Count; i++)
        {
            distances.Add(SphereIntersection(sphereRadius[i], sphereCenter[i], rayDirection, rayOrigin));
        }

        int? nearestSphereIndex = null;
        float minDistance = Mathf.Infinity;

        for(int i=0; i<distances.Count; i++)
        {
            if(distances[i] != null && distances[i] < minDistance)
            {
                minDistance = (float) distances[i];
                nearestSphereIndex = i;
            }
        }
        return (nearestSphereIndex, minDistance);
    }

    public Vector3 ReflectedVector(Vector3 incidentVector, Vector3 normalVector)
    {
        return incidentVector - 2 * Vector3.Dot(incidentVector, normalVector) * normalVector;
    }

    private void sphereDiffuseData(List<Vector3> sphereDiffuseList)
    {
        for(int i=0; i<sphereCenters.Count; i++)
        {
            Vector3 temp = SphereAmbient[i];
            if(temp == new Vector3(0f, 0f, 0f))
            {
                sphereDiffuseList.Add(new Vector3(0f, 0f, 0f));
            }
            else if((temp.x != 0f) && (temp.y != 0f) && (temp.z != 0f))
            {
                sphereDiffuseList.Add(new Vector3(0.3f, 0.3f, 0.3f));
            }
            else if((temp.x != 0f) && (temp.y == 0f) && (temp.z == 0f))
            {
                sphereDiffuseList.Add(new Vector3(0.3f, 0f, 0f));
            }
            else if ((temp.x == 0f) && (temp.y != 0f) && (temp.z == 0f))
            {
                sphereDiffuseList.Add(new Vector3(0f, 0.3f, 0f));
            }
            else if ((temp.x == 0f) && (temp.y == 0f) && (temp.z != 0f))
            {
                sphereDiffuseList.Add(new Vector3(0f, 0f, 0.3f));
            }
            else if ((temp.x != 0f) && (temp.y != 0f) && (temp.z == 0f))
            {
                sphereDiffuseList.Add(new Vector3(0.3f, 0.3f, 0f));
            }
            else if ((temp.x == 0f) && (temp.y != 0f) && (temp.z != 0f))
            {
                sphereDiffuseList.Add(new Vector3(0f, 0.3f, 0.3f));
            }
            else if ((temp.x != 0f) && (temp.y == 0f) && (temp.z != 0f))
            {
                sphereDiffuseList.Add(new Vector3(0.3f, 0f, 0.3f));
            }
            else
            {
                Debug.Log("Error: Didn't add diffuse data for object index - " + i.ToString());
            }
        }
    }

    private void sphereSpecularShininess(List<Vector3> sphereSpecularList, List<int> sphereShininessList)
    {
        for (int i = 0; i < sphereCenters.Count; i++)
        {
            sphereSpecularList.Add(new Vector3(1f, 1f, 1f));
            sphereShininessList.Add(100);
        }
    }

    private void sphereReflectionData(List<float> sphereReflectionList)
    {
        for (int i = 0; i < sphereCenters.Count; i++)
        {
            if(SphereAmbient[i] == new Vector3(0f, 0f, 0f))
            {
                sphereReflectionList.Add(1f);
            }
            else
            {
                sphereReflectionList.Add(0.5f);
            }
        }
    }

    private void CreatePlane()
    {
        sphereCenters.Add(new Vector3(0f, -20000f, 0f));
        SphereRadius.Add(20000f - 0.2f);
        SphereAmbient.Add(new Vector3(0.05f, 0.05f, 0.05f));
        SphereDiffuse.Add(new Vector3(0.05f, 0.05f, 0.05f));
        SphereSpecular.Add(new Vector3(0.2f, 0.2f, 0.2f));
        SphereShininess.Add(10);
        SphereReflection.Add(0.1f);
    }

}