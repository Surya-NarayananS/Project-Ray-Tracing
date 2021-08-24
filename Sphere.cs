using UnityEngine;

[CreateAssetMenu(menuName ="Sphere")]
public class Sphere : ScriptableObject
{
    private AddSphere sphereData;
    public GameObject SpherePrimitive;
    public Vector3 position;
    public Vector3 sphereColor;
    public float radius;
    public bool IsCheckeredActive;
    public bool IsMirroredActive;

    private void OnEnable()
    {
        sphereData = FindObjectOfType<AddSphere>();
        GetSphereData();
        SpherePrimitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        SpherePrimitive.tag = "Sphere";
        SpherePrimitive.transform.position = position;
        SpherePrimitive.transform.localScale = new Vector3(radius, radius, radius);
        if(IsCheckeredActive)
        {
            Material checkeredMat = Resources.Load("Checkered", typeof(Material)) as Material;
            SpherePrimitive.GetComponent<Renderer>().material = checkeredMat;
        }
        else if(IsMirroredActive)
        {
            Material mirroredMat = Resources.Load("Mirrored", typeof(Material)) as Material;
            SpherePrimitive.GetComponent<Renderer>().material = mirroredMat;
        }
        else
        {
            SpherePrimitive.GetComponent<Renderer>().material.color = new Color(sphereColor.x, sphereColor.y, sphereColor.z);
        }
    }

    private void GetSphereData()
    {
        position = sphereData.GetSpherePosition();
        sphereColor = sphereData.GetSphereColor();
        radius = sphereData.GetSphereRadius();
        IsCheckeredActive = sphereData.IsCheckeredActive();
        IsMirroredActive = sphereData.IsMirroredActive();
    }

}
