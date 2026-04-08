using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public List<GameObject> objectPrefabs;
    public TrialManager trialManager;

    private int currentIndex = 0;
    private GameObject currentObject;

    void Start()
    {
        SpawnNext();
    }

    public void SpawnNext()
    {
        if (currentIndex >= objectPrefabs.Count)
        {
            Debug.Log("All objects spawned");
            return;
        }

        // Spawn the next object at the spawn point
        currentObject = Instantiate(
            objectPrefabs[currentIndex],
            spawnPoint.position,
            spawnPoint.rotation
        );

        // Tell TrialManager which object just appeared
        SortableObject sortable = currentObject.GetComponent<SortableObject>();
        if (sortable != null)
            trialManager.ObjectSpawned(sortable.objectName);

        currentIndex++;
        Debug.Log("Spawned object " + currentIndex + " of " + objectPrefabs.Count);
    }
}