using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GameObject blurPanel;
    public GameObject quitButton;
    public GameObject newSceneButton;

    public void RenderCompleted()
    {
        if(blurPanel != null)
        {
            blurPanel.SetActive(true);
        }
        if(quitButton != null)
        {
            quitButton.SetActive(true);
        }
        if (newSceneButton != null)
        {
            newSceneButton.SetActive(true);
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    
}
