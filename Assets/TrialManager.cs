using UnityEngine;

public class TrialManager : MonoBehaviour
{
    public TrialTimer timer;
    public int totalObjects = 2;
    
    private int placedCount = 0;
    private float objectSpawnTime;      // when current object appeared
    private string currentObjectName;   // for logging

    void Awake()
    {
        if (timer == null)
            timer = GetComponent<TrialTimer>();
    }

    // Call this when a new object spawns
    public void ObjectSpawned(string objectName)
    {
        objectSpawnTime = Time.time;
        currentObjectName = objectName;
        Debug.Log("Object spawned: " + objectName);
    }

    // Call this when object is placed in a bin
    public void ObjectPlaced(string objectName, string binName, bool correct)
    {
        float objectTime = Time.time - objectSpawnTime;
        placedCount++;

        Debug.Log($"[TRIAL DATA] Object: {objectName} | Bin: {binName} | Correct: {correct} | Time: {objectTime:F2}s");

        if (placedCount >= totalObjects)
        {
            timer.StopTimer();
            Debug.Log("All objects placed — trial complete");
        }
    }
}

