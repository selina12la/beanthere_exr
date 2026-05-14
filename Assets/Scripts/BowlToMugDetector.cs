using UnityEngine;

public class BowlToMugDetector : MonoBehaviour
{
    public MatchaBowlController bowl;
    private ITaskListManager taskManager;
    private bool hasPoured = false;
    
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        
        // Sicherstellen dass Collider als Trigger ist
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
            Debug.Log("BowlToMugDetector: Collider set to trigger");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"BowlToMugDetector triggered by: {other.name} (Tag: '{other.tag}')");
        
        // Prüfe auf Bowl - entweder per Tag oder per Component
        bool isBowl = other.CompareTag("Bowl") || other.GetComponent<MatchaBowlController>() != null;
        
        if (isBowl && bowl != null && bowl.HasMatchaReady() && !hasPoured)
        {
            hasPoured = true;
            
            // Finde den Mug (Parent des Detectors)
            MugXR mug = GetComponentInParent<MugXR>();
            if (mug != null)
            {
                mug.AddMatcha(); // Matcha in Mug füllen
                bowl.ResetBowl();
                taskManager?.OnFilterInMachine(); // Task 3
                Debug.Log("✅ Matcha poured into mug! Task 3 complete");
            }
            else
            {
                Debug.LogError("BowlToMugDetector: No MugXR found in parent!");
            }
        }
    }
}