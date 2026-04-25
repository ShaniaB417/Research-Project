using UnityEngine;
using UnityEngine.InputSystem;

public class BinDetector : MonoBehaviour
{
    [Header("Bin Settings")]
    public string binName;
    public TrialManager trialManager;
    public ObjectSpawner spawner;

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
        Debug.Log("Something entered " + binName + ": " + other.gameObject.name + " tag: " + other.tag);
        if (!other.CompareTag("SortableObject")) return;
        SortableObject sortable = other.GetComponent<SortableObject>();
        if (sortable == null) return;
        bool correct = (sortable.correctBinName == binName);
        Debug.Log("BIN TRIGGERED: " + sortable.objectName + " entered " + binName);
        trialManager.ObjectPlaced(sortable.objectName, binName, correct);
        Destroy(other.gameObject);
        if (spawner != null)
            spawner.SpawnNext();
    }
}
