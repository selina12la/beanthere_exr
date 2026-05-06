using DefaultNamespace;
using UnityEngine;
public class EspressoFilterDetector : MonoBehaviour
{
    public EspressoMachineController machine;
    private ITaskListManager taskManager;  // ← Interface
 
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
    }
    public void OnFilterInserted()
    {
        if (machine != null)
        {
            machine.SetFilter(true);
            Debug.Log("Filter inserted into Espresso Machine!");
        }
        if (taskManager != null)
        {
            taskManager.OnFilterInMachine();
            Debug.Log("Task 3: Filter in Espresso Machine - COMPLETED!");
        }
    }
    public void OnFilterRemoved()
    {
        if (machine != null)
        {
            machine.SetFilter(false);
            Debug.Log("Filter removed from Espresso Machine");
        }
    }
}