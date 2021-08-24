using UnityEngine;
using UnityEngine.UI;

public class ComputationUI : MonoBehaviour
{
    public GameObject ComputingIllumination;
    public GameObject ComputationCompleted;
    public GameObject InitiatingSceneRender;
    public GameObject SceneRendered;

    public void TurnOnComputingIllumination()
    {
        if(ComputingIllumination != null)
        {
            ComputingIllumination.SetActive(true);
        }
    }

    public void TurnOnComputationCompleted()
    {
        if (ComputationCompleted != null)
        {
            ComputationCompleted.SetActive(true);
        }
    }

    public void TurnOnInitiatingSceneRender()
    {
        if (InitiatingSceneRender != null)
        {
            InitiatingSceneRender.SetActive(true);
        }
    }

    public void TurnOnSceneRendered()
    {
        if (SceneRendered != null)
        {
            SceneRendered.SetActive(true);
        }
    }

}
