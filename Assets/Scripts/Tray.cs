using UnityEngine;
using System.Collections;

public class Tray : MonoBehaviour
{
    [Header("References")]
    public GameCompletionManager gameManager;
    public TaskListManager taskManager;
    
    [Header("Mug Spawning")]
    public GameObject mugPrefab;           // Das Mug Prefab
    public Transform mugSpawnPoint;        // Startposition
    public EspressoMachineController espressoMachine;
    
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
                
                if (taskManager != null)
                    taskManager.OnMugOnTray();
                
                if (gameManager != null)
                    gameManager.CompleteLevel(currentMug);
                
                // Alten Mug zerstören
                Destroy(currentMug);
                currentMug = null;
                
                // Neuen Mug spawnen
                SpawnNewMug();
                
                if (espressoMachine != null)
                    espressoMachine.ResetMachine();
                
                isProcessing = false;
                stayTimer = 0f;
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
    
    private void SpawnNewMug()
    {
        if (mugPrefab != null && mugSpawnPoint != null)
        {
            // Lösung 1: Spawne mit der exakten Position des SpawnPoints
            GameObject newMug = Instantiate(mugPrefab, mugSpawnPoint.position, mugSpawnPoint.rotation);
            newMug.tag = "Mug";
        
            // WICHTIG: Setze die Position des Parents auf die SpawnPoint Position
            newMug.transform.position = mugSpawnPoint.position;
            newMug.transform.rotation = mugSpawnPoint.rotation;
        
            // Stelle sicher dass der Mug nicht durchfällt
            Rigidbody rb = newMug.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        
            // Collider prüfen (aber NUR auf dem Child)
            Collider col = newMug.GetComponentInChildren<Collider>();
            if (col == null)
            {
                Debug.LogWarning("Mug has no collider on any child!");
            }
        
            // MugXR zurücksetzen
            MugXR mugXR = newMug.GetComponent<MugXR>();
            if (mugXR != null)
            {
                mugXR.RemoveCoffee();
            }
        
            Debug.Log($"🔄 New mug spawned at position: {mugSpawnPoint.position}");
        }
        else
        {
            Debug.LogError("mugPrefab or mugSpawnPoint not set in Tray!");
        }
    }
}