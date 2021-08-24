using UnityEngine;

public class CreateSphere : MonoBehaviour
{
    public GameObject SphereDataPanel;

    public void CreateNewSphere()
    {
        Sphere NewSphere = ScriptableObject.CreateInstance("Sphere") as Sphere;
        if(SphereDataPanel != null)
        {
            SphereDataPanel.SetActive(false);
        }
    }
}
