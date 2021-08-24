using UnityEngine;
using UnityEngine.UI;

public class AddSphere : MonoBehaviour
{
    public GameObject SphereDataPanel;
    public InputField RadiusInput;
    public InputField XPos;
    public InputField YPos;
    public InputField ZPos;
    public InputField Red;
    public InputField Green;
    public InputField Blue;
    public Toggle CheckeredPattern;
    public Toggle MirroredSurface;

    private float radiusValue;
    private Vector3 position;
    private Vector3 color;

    bool checkeredIsActive = false;
    bool mirroredIsActive = false;

    public void DisplaySphereDataPanel()
    {
        if(SphereDataPanel != null)
        {
            if(SphereDataPanel.activeInHierarchy)
            {
                SphereDataPanel.SetActive(false);
            }
            else
            {
                SphereDataPanel.SetActive(true);
            }
        }
    }

    public void UpdateRadius()
    {
        radiusValue = Mathf.Clamp((float)System.Math.Round(Mathf.Abs(float.Parse(RadiusInput.text)), 2), 0f, 1f);
    }

    float xPosition = 0f;
    float yPosition = 0f;
    float zPosition = 0f;

    public void UpdateXPosition()
    {
        xPosition = float.Parse(XPos.text);
        position.x = xPosition;
    }

    public void UpdateYPosition()
    {
        yPosition = float.Parse(YPos.text);
        position.y = yPosition;
    }

    public void UpdateZPosition()
    {
        zPosition = float.Parse(ZPos.text);
        position.z = zPosition;
    }

    private float redValue = 1f;
    private float blueValue = 1f;
    private float greenValue = 1f;

    public void UpdateRed()
    {
        redValue = Mathf.Clamp(Mathf.Abs(float.Parse(Red.text)), 0f, 1f);
        color.x = redValue;
    }
    public void UpdateGreen()
    {
        greenValue = Mathf.Clamp(Mathf.Abs(float.Parse(Green.text)), 0f, 1f);
        color.y = greenValue;
    }
    public void UpdateBlue()
    {
        blueValue = Mathf.Clamp(Mathf.Abs(float.Parse(Blue.text)), 0f, 1f);
        color.z = blueValue;
    }



    public void ToggleCheckeredPattern()
    {
        if (checkeredIsActive)
        {
            MirroredSurface.interactable = true;
            Red.interactable = true;
            Green.interactable = true;
            Blue.interactable = true;
            checkeredIsActive = false;
        }
        else
        {
            MirroredSurface.interactable = false;
            Red.interactable = false;
            Green.interactable = false;
            Blue.interactable = false;
            checkeredIsActive = true;
        }
    }

    public void ToggleMirroredSurface()
    {
        if(mirroredIsActive)
        {
            CheckeredPattern.interactable = true;
            Red.interactable = true;
            Green.interactable = true;
            Blue.interactable = true;
            mirroredIsActive = false;
        }
        else
        {
            CheckeredPattern.interactable = false;
            Red.interactable = false;
            Green.interactable = false;
            Blue.interactable = false;
            mirroredIsActive = true;
        }
    }

    public float GetSphereRadius()
    {
        return radiusValue;
    }

    public Vector3 GetSpherePosition()
    {
        return position;
    }

    public Vector3 GetSphereColor()
    {
        return color;
    }

    public bool IsCheckeredActive()
    {
        return checkeredIsActive;
    }

    public bool IsMirroredActive()
    {
        return mirroredIsActive;
    }
}
