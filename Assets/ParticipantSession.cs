using UnityEngine;

public class ParticipantSession : MonoBehaviour
{
    public static ParticipantSession Instance; //participant ID is needed across scenes 

    public string ParticipantID; 

    void Awake() {
        if (Instance == null) {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
