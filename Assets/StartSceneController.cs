using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
using System.Collections;

public class StartSceneController : MonoBehaviour
{
    public TMP_InputField participantIDField;

    public void BeginExperiment()
    {
        string id = participantIDField.text.Trim();
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("Participant ID cannot be empty.");
            return;
        }

        ParticipantSession.Instance.ParticipantID = id;
        StartCoroutine(InitXRAndLoad());
    }

    private IEnumerator InitXRAndLoad()
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        XRGeneralSettings.Instance.Manager.StartSubsystems();
        SceneManager.LoadScene("Warm UP");
    }
}