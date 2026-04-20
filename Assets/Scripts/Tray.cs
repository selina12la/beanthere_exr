using UnityEngine;
using System.Collections;

public class Tray: MonoBehaviour
{
    [Header("References")]
    public GameCompletionManager gameManager;
    
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
            Debug.Log($"Mug placed on tablet! Waiting {requiredStayTime} seconds...");
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
                Debug.Log("Coffee delivered! Level complete!");
                
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
            Debug.Log("Mug removed from tablet - timer reset");
        }
    }
}