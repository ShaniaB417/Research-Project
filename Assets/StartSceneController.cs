using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
 
public TMP_InputField participantIDField; // Reference to the input field for participant ID

public void BeginExperiment() {

    string id = participantIDField.text.Trim();

    if (string.IsNullOrEmpty(id)) {
        Debug.LogWarning("Participant ID cannot be empty."); 
        return; 
    }

    ParticipantSession.Instance.ParticipantID = id;

    SceneManager.LoadScene("Warm UP"); //Make sure this is the same as the name of the actual scene 
}
}
