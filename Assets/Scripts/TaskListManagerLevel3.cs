using UnityEngine;
using System.Collections.Generic;

public class TaskListManagerLevel3 : MonoBehaviour, ITaskListManager
{
    [Header("References")]
    public GameObject taskItemPrefab;
    public Transform taskContainer;
    
    private List<TaskItem> taskItems = new List<TaskItem>();
    
    private bool matchaPowderAdded = false;
    private bool waterAdded = false;
    private bool matchaInMug = false;
    private bool milkSteamed = false;
    private bool milkPoured = false;
    private bool mugOnTray = false;
    
    private void Start()
    {
        CreateTaskList();
    }
    
    private void CreateTaskList()
    {
        if (taskItemPrefab == null || taskContainer == null) return;
        
        foreach (Transform child in taskContainer)
            Destroy(child.gameObject);
        taskItems.Clear();
        
        string[] taskDescriptions = new string[]
        {
            "1. Hover matcha powder over bowl",
            "2. Hover water pot over bowl",
            "3. Hover matcha bowl over mug",
            "4. Steam milk in espresso machine",
            "5. Pour milk into matcha cup",
            "6. Place mug on serving tray"
        };
        
        foreach (string description in taskDescriptions)
        {
            GameObject taskObj = Instantiate(taskItemPrefab, taskContainer);
            TaskItem taskItem = taskObj.GetComponent<TaskItem>();
            if (taskItem == null)
            {
                taskItem = taskObj.GetComponentInChildren<TaskItem>();
            }
            
            if (taskItem != null)
            {
                taskItem.Initialize(description);
                taskItems.Add(taskItem);
            }
        }
    }
    
    public void OnBeansPoured()
    {
        if (!matchaPowderAdded)
        {
            matchaPowderAdded = true;
            CompleteTask(0);
        }
    }
    
    public void OnFilterGround()
    {
        if (!waterAdded && matchaPowderAdded)
        {
            waterAdded = true;
            CompleteTask(1);
        }
    }
    
    public void OnFilterInMachine()
    {
        if (!matchaInMug && waterAdded)
        {
            matchaInMug = true;
            CompleteTask(2);
        }
    }
    
    public void OnMugInMachine()
    {
        // Not used in Level 3
    }
    
    public void OnMilkSteamed()
    {
        if (!milkSteamed && matchaInMug)
        {
            milkSteamed = true;
            CompleteTask(3);
        }
    }
    
    public void OnMilkPoured()
    {
        if (!milkPoured && milkSteamed)
        {
            milkPoured = true;
            CompleteTask(4);
        }
    }
    
    public void OnMugOnTray()
    {
        if (!mugOnTray && milkPoured)
        {
            mugOnTray = true;
            CompleteTask(5);
            OnAllTasksComplete();
        }
    }
    
    private void CompleteTask(int index)
    {
        if (index < taskItems.Count && taskItems[index] != null)
        {
            taskItems[index].Complete();
        }
    }
    
    private void OnAllTasksComplete()
    {
        Debug.Log("LEVEL 3 COMPLETE!");
        LevelCompletionManager completion = FindFirstObjectByType<LevelCompletionManager>();
        if (completion != null)
        {
            completion.CompleteLevel("Level 3");
        }
    }
}