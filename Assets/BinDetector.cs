using UnityEngine;
using UnityEngine.InputSystem;

public class BinDetector : MonoBehaviour
{
    [Header("Bin Settings")]
    public string binName;
    public TrialManager trialManager;

void Update()
    {
        // TEMPORARY TEST - remove later
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

        bool correct = (sortable.correctBinName == binName);

        trialManager.ObjectPlaced(sortable.objectName, binName, correct);
        Destroy(other.gameObject);
    }
}

