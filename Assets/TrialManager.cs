using UnityEngine;
using UnityEngine.SceneManagement;
public class TrialManager : MonoBehaviour
{
public TrialTimer timer;
public DataLogger dataLogger;
public string participantID = "";
public int totalObjects = 2;
private int placedCount = 0;
private string lastPlacedObject = "";
private float objectSpawnTime;
private string currentObjectName;
void Awake()
    {
if (timer == null)
timer = GetComponent<TrialTimer>();
if (dataLogger == null)
dataLogger = GetComponent<DataLogger>();
    }
void Start() {
if(ParticipantSession.Instance != null) {
participantID = ParticipantSession.Instance.ParticipantID;
Debug.Log("Participant ID: " + participantID);
        }
else {
Debug.LogWarning("ParticipantSession instance not found. Using default participant ID.");
        }
    }
public void ObjectSpawned(string objectName)
    {
lastPlacedObject = "";
objectSpawnTime = Time.time;
currentObjectName = objectName;
Debug.Log("Object spawned: " + objectName);
    }
public void ObjectPlaced(string objectName, string binName, bool correct)
    {
if (objectName == lastPlacedObject) return;
lastPlacedObject = objectName;
float objectTime = Time.time - objectSpawnTime;
float sessionTime = Time.time - timer.GetStartTime();
placedCount++;
Debug.Log("Placed count: " + placedCount + " / Total: " + totalObjects);
dataLogger.LogPlacement(participantID, objectName, binName, correct, objectTime, sessionTime, "Warmup");
if (placedCount >= totalObjects)
        {
timer.StopTimer();
dataLogger.SaveFile();
Debug.Log("All objects placed — trial complete");
SceneManager.LoadScene("Environment");
        }
    }
}

