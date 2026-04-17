using UnityEngine;
using UnityEngine.XR.Management;
using System.Collections;

public class DeferredXRInit : MonoBehaviour
{
    [SerializeField] private string vrSceneName = "Start";

    public void StartXRAndLoadNextScene()
    {
        StartCoroutine(StartXR());
    }

    private IEnumerator StartXR()
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
        UnityEngine.SceneManagement.SceneManager.LoadScene(vrSceneName);
    }
}