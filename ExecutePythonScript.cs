using System.Collections;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class ExecutePythonScript : MonoBehaviour
{
    public ComputationUI computationScreenUI;
    public Manager manager;

    float waitTimeForRendering = 6.8f;
    string docPath = Directory.GetCurrentDirectory();

    private IEnumerator WaitAndExecute(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        computationScreenUI.TurnOnSceneRendered();
        var p2 = new Process
        {
            StartInfo =
                 {
                     FileName = "Render.png",
                     WorkingDirectory = @docPath + "\\Assets\\Scene_Render",
                 }
        }.Start();
        manager.RenderCompleted();
    }

    public void RenderAndOpenImage()
    {
        var p1 = new Process
        {
            StartInfo =
                 {
                     FileName = "RenderScene.exe",
                     WorkingDirectory = @docPath + "\\Assets\\Scripts",
                     CreateNoWindow = true,
                     WindowStyle = ProcessWindowStyle.Hidden
                 }
        }.Start();

        StartCoroutine(WaitAndExecute(waitTimeForRendering));
    }
}
