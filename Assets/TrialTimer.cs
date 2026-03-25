using UnityEngine;
using TMPro;

public class TrialTimer : MonoBehaviour
{
    public TextMeshPro timerText;
   
    private float startTime;
    
    private bool timerRunning = false;
    
    private bool trialStarted = false;

    void Update()
    {
        // Every frame, if timer is running update the display
        if (timerRunning)
        {
            float elapsed = Time.time - startTime;
            
            
            if (timerText != null)
                timerText.text = "Time: " + elapsed.ToString("F2") + "s";
        }
    }

 
    // Starts the timer on the participant's first grab only
    public void StartTimer()
    {
        if (!trialStarted)
        {
            startTime = Time.time;
            timerRunning = true;
            trialStarted = true;
            Debug.Log("Trial started"); 
        }
    }


    // Stops the timer and logs the final time
    public void StopTimer()
    {
        if (timerRunning)
        {
            timerRunning = false;
            float totalTime = Time.time - startTime;
            
            // Logs time to Console (save this to CSV later)
            Debug.Log("Trial complete. Time: " + totalTime.ToString("F2") + "s");
        }
    }
    
    // reset between conditions
    public void ResetTimer()
    {
        timerRunning = false;
        trialStarted = false;
        startTime = 0f;
        
        if (timerText != null)
            timerText.text = "Time: 0.00s";
            
        Debug.Log("Timer reset");
    }
}