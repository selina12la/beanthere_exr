using UnityEngine;
using System.Collections;

public class Tray : MonoBehaviour
{
    [Header("References")]
    public GameCompletionManager gameManager;
    public TaskListManager taskManager;  // NEU: TaskManager Referenz
    
    [Header("Settings")]
    public float requiredStayTime = 2f;  
    public string requiredTag = "Mug";   
    
    private GameObject currentMug = null;
    private float stayTimer = 0f;
    private bool isProcessing = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (isProcessing) return;
        
        if (other.CompareTag(requiredTag))
        {
            currentMug = other.gameObject;
            stayTimer = 0f;
            Debug.Log($"Mug placed on tray! Waiting {requiredStayTime} seconds...");
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (isProcessing) return;
        
        if (currentMug != null && other.gameObject == currentMug)
        {
            stayTimer += Time.deltaTime;
            
            if (stayTimer >= requiredStayTime)
            {
                isProcessing = true;
                Debug.Log("✅ Coffee delivered! Task 5 complete!");
                
                // Task 5: Mug on Tray
                if (taskManager != null)
                {
                    taskManager.OnMugOnTray();
                    Debug.Log("📋 Task 5 completed: Mug on serving tray");
                }
                
                // Optional: Level Completion
                if (gameManager != null)
                    gameManager.CompleteLevel(currentMug);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (isProcessing) return;
        
        if (currentMug != null && other.gameObject == currentMug)
        {
            currentMug = null;
            stayTimer = 0f;
            Debug.Log("Mug removed from tray - timer reset");
        }
    }
}