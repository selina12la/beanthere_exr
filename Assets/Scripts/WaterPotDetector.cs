using UnityEngine;

public class WaterPotDetector : MonoBehaviour
{
    public MatchaBowlController bowl;
    private ITaskListManager taskManager;
    
    void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterPot"))
        {
            bowl.AddWater();
            taskManager?.OnFilterGround(); // Task 2
            Debug.Log("✅ Water added! Task 2 complete");
        }
    }
}