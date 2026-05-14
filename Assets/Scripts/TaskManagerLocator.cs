using UnityEngine;

public static class TaskManagerLocator
{
    private static ITaskListManager cachedTaskManager;
    private static bool hasSearched = false;
    
    public static ITaskListManager Current
    {
        get
        {
            if (!hasSearched || cachedTaskManager == null)
            {
                FindTaskManager();
            }
            return cachedTaskManager;
        }
    }
    
    private static void FindTaskManager()
    {
        hasSearched = true;
        
        var allManagers = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var manager in allManagers)
        {
            if (manager is ITaskListManager taskManager)
            {
                cachedTaskManager = taskManager;
                return;
            }
        }
    }
    
    public static void Reset()
    {
        cachedTaskManager = null;
        hasSearched = false;
    }
}