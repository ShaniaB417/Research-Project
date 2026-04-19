using UnityEngine;
using System.IO;

public class ParticipantSession : MonoBehaviour
{
    public static ParticipantSession Instance;
    public string ParticipantID = "DEFAULT";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadParticipantID();
        }
        else Destroy(gameObject);
    }

    void LoadParticipantID()
    {
        string path = Path.Combine(Application.persistentDataPath, "participant.txt");
        if (File.Exists(path))
        {
            ParticipantID = File.ReadAllText(path).Trim();
            Debug.Log("Loaded Participant ID: " + ParticipantID);
        }
        else
        {
            Debug.LogWarning("No participant.txt found, using DEFAULT.");
        }
    }
}
