using UnityEngine;

public class MatchaPowderDetector : MonoBehaviour
{
    public MatchaBowlController bowl;
    private ITaskListManager taskManager;
    private bool matchaWasDetected = false;

    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!matchaWasDetected && other.transform.root.CompareTag("CoffeeBeans"))
        {
            matchaWasDetected = true;
            bowl.AddMatchaPowder();
            taskManager?.OnBeansPoured(); // Task 1
        }
    }
}