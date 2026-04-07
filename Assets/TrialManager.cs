using UnityEngine;

public class TrialManager : MonoBehaviour
{
    public TrialTimer timer;
    public DataLogger dataLogger;
    public string participantID = "P01";
    public int totalObjects = 2;

    private int placedCount = 0;
    private float objectSpawnTime;
    private string currentObjectName;

    void Awake()
    {
        if (timer == null)
            timer = GetComponent<TrialTimer>();
        if (dataLogger == null)
            dataLogger = GetComponent<DataLogger>();
    }

    public void ObjectSpawned(string objectName)
    {
        objectSpawnTime = Time.time;
        currentObjectName = objectName;
        Debug.Log("Object spawned: " + objectName);
    }

    public void ObjectPlaced(string objectName, string binName, bool correct)
    {
        float objectTime = Time.time - objectSpawnTime;
        float sessionTime = Time.time - timer.GetStartTime();
        placedCount++;

        dataLogger.LogPlacement(participantID, objectName, binName, correct, objectTime, sessionTime, "Warmup");

        if (placedCount >= totalObjects)
        {
            timer.StopTimer();
            dataLogger.SaveFile();
            Debug.Log("All objects placed — trial complete");
        }
    }
}

