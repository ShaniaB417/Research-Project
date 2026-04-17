using UnityEngine;

public class ParticipantSession : MonoBehaviour
{
    public static ParticipantSession Instance;
    public string ParticipantID;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this from your Confirm button via the InputField
    public void SetParticipantID(string id)
    {
        ParticipantID = id;
        Debug.Log("Participant ID set: " + ParticipantID);
    }
}
