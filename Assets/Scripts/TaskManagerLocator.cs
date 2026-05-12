using UnityEngine;

public static class TaskManagerLocator
{
    private static ITaskListManager cachedTaskManager;
    private static bool hasSearched = false;
    
    // Gets the current task manager
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
        
        // Find all MonoBehaviours that implement ITaskListManager
        var allManagers = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var manager in allManagers)
        {
            if (manager is ITaskListManager taskManager)
            {
                cachedTaskManager = taskManager;
                Debug.Log($"✅ TaskManager found: {manager.GetType().Name}");
                return;
            }
        }
        
        Debug.LogError("❌ No ITaskListManager found in scene!");
    }
    
    // Call this when loading a new scene to reset the cache
    public static void Reset()
    {
        cachedTaskManager = null;
        hasSearched = false;
    }
}