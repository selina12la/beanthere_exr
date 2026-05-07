using UnityEngine;

public class MilkSteamer : MonoBehaviour
{
    private ITaskListManager taskManager;
    public MilkFrotherSnapZone snapZone;

    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        if (snapZone == null)
        {
            snapZone = GetComponent<MilkFrotherSnapZone>();
        }
    }

    public void OnMilkSteamed()
    {
        if (taskManager != null)
        {
            taskManager.OnMilkSteamed();
        }
    }
}