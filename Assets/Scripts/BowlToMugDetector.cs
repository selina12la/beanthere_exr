using UnityEngine;

public class BowlToMugDetector : MonoBehaviour
{
    public MatchaBowlController bowl;
    private ITaskListManager taskManager;
    private bool hasPoured = false;
    
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        bool isBowl = other.CompareTag("Bowl") || other.GetComponent<MatchaBowlController>() != null;
        
        if (isBowl && bowl != null && bowl.HasMatchaReady() && !hasPoured)
        {
            hasPoured = true;
            
            MugXR mug = GetComponentInParent<MugXR>();
            if (mug != null)
            {
                mug.AddMatcha();
                bowl.ResetBowl();
                taskManager?.OnFilterInMachine(); // Task 3
            }
        }
    }
}