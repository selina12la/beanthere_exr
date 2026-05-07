using UnityEngine;

public class EspressoFilterDetector : MonoBehaviour
{
    public EspressoMachineController machine;
    private ITaskListManager taskManager;

    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
    }

    public void OnFilterInserted()
    {
        if (machine != null)
        {
            machine.SetFilter(true);
        }

        if (taskManager != null)
        {
            taskManager.OnFilterInMachine();
        }
    }

    public void OnFilterRemoved()
    {
        if (machine != null)
        {
            machine.SetFilter(false);
        }
    }
}