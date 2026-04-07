using UnityEngine;
using System.IO;
using System.Text;

public class DataLogger : MonoBehaviour
{
    private string filePath;
    private StringBuilder csvData;

    void Awake()
    {
        // Creates file in a place accessible on both PC and Quest
        filePath = Path.Combine(Application.persistentDataPath, "trial_data.csv");
        csvData = new StringBuilder();

        // Write the header row
        csvData.AppendLine("ParticipantID,ObjectName,BinPlaced,Correct,ObjectTime,SessionTime,Realism");

        Debug.Log("DataLogger initialized. Saving to: " + filePath);
    }

    public void LogPlacement(string participantID, string objectName, string binPlaced, bool correct, float objectTime, float sessionTime, string realism)
    {
        string row = $"{participantID},{objectName},{binPlaced},{correct},{objectTime:F2},{sessionTime:F2},{realism}";
        csvData.AppendLine(row);
        Debug.Log("[CSV ROW] " + row);
    }

    // Call this when the full trial ends
    public void SaveFile()
    {
        File.WriteAllText(filePath, csvData.ToString());
        Debug.Log("Data saved to: " + filePath);
    }

    void OnApplicationQuit()
    {
        SaveFile();
    }
}