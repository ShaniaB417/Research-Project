using UnityEngine;
using UnityEngine.InputSystem;

public class BinDetector : MonoBehaviour
{
    [Header("Bin Settings")]
    public string binName;
    public TrialManager trialManager;
    public ObjectSpawner spawner;
    private string lastTriggeredObject = "";

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            trialManager.ObjectPlaced("TestObject", binName, true);
            Debug.Log("Manual test trigger fired on " + binName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("SortableObject")) return;
        SortableObject sortable = other.GetComponent<SortableObject>();
        if (sortable == null) return;
        if (sortable.objectName == lastTriggeredObject) return;
        lastTriggeredObject = sortable.objectName;

        bool correct = (sortable.correctBinName == binName);
        Debug.Log("BIN TRIGGERED: " + sortable.objectName + " entered " + binName);
        trialManager.ObjectPlaced(sortable.objectName, binName, correct);
        Destroy(other.gameObject);
        if (spawner != null)
            StartCoroutine(SpawnAfterDelay());
    }

    private System.Collections.IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        lastTriggeredObject = "";
        spawner.SpawnNext();
    }
}