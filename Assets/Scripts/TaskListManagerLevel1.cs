using UnityEngine;
using System.Collections.Generic;

public class TaskListManagerLevel1 : MonoBehaviour, ITaskListManager
{
    [Header("References")]
    public GameObject taskItemPrefab;
    public Transform taskContainer;
    
    private List<TaskItem> taskItems = new List<TaskItem>();
    
    private bool beansPoured = false;
    private bool filterGround = false;
    private bool filterInMachine = false;
    private bool mugInMachine = false;
    private bool mugOnTray = false;
    
    private void Start()
    {
        Debug.Log("=== TaskListManagerLevel1 START ===");
        CreateTaskList();
    }
    
    private void CreateTaskList()
    {
        if (taskItemPrefab == null)
        {
            Debug.LogError("taskItemPrefab is NULL! Assign it in the Inspector!");
            return;
        }
        
        if (taskContainer == null)
        {
            Debug.LogError("taskContainer is NULL! Assign it in the Inspector!");
            return;
        }
        
        foreach (Transform child in taskContainer)
            Destroy(child.gameObject);
        taskItems.Clear();
        
        string[] taskDescriptions = new string[]
        {
            "1. Hover coffee bag over grinder",
            "2. Snap filter into grinder",
            "3. Move filter to espresso machine",
            "4. Snap mug into espresso machine",
            "5. Place mug on serving tray"
        };
        
        Debug.Log($"Creating {taskDescriptions.Length} tasks...");
        
        foreach (string description in taskDescriptions)
        {
            GameObject taskObj = Instantiate(taskItemPrefab, taskContainer);
            
            TaskItem taskItem = taskObj.GetComponent<TaskItem>();
            if (taskItem == null)
                taskItem = taskObj.GetComponentInChildren<TaskItem>();
            
            if (taskItem != null)
            {
                taskItem.Initialize(description);
                taskItems.Add(taskItem);
                Debug.Log($"✅ Task created: {description}");
            }
            else
            {
                Debug.LogError($"❌ TaskItem component not found on prefab! Prefab name: {taskItemPrefab.name}");
                Debug.LogError($"   Make sure the TaskItem script is attached to the root of the prefab!");
            }
        }
        
        Debug.Log($"Total tasks created: {taskItems.Count}");
    }
    
    public void OnBeansPoured()
    {
        Debug.Log($"OnBeansPoured called! Current state: beansPoured={beansPoured}");
        if (!beansPoured)
        {
            beansPoured = true;
            CompleteTask(0);
        }
    }
    
    public void OnFilterGround()
    {
        Debug.Log($"OnFilterGround called! beansPoured={beansPoured}, filterGround={filterGround}");
        if (!filterGround && beansPoured)
        {
            filterGround = true;
            CompleteTask(1);
        }
    }
    
    public void OnFilterInMachine()
    {
        Debug.Log($"OnFilterInMachine called! filterGround={filterGround}, filterInMachine={filterInMachine}");
        if (!filterInMachine && filterGround)
        {
            filterInMachine = true;
            CompleteTask(2);
        }
    }
    
    public void OnMugInMachine()
    {
        Debug.Log($"OnMugInMachine called! filterInMachine={filterInMachine}, mugInMachine={mugInMachine}");
        if (!mugInMachine && filterInMachine)
        {
            mugInMachine = true;
            CompleteTask(3);
        }
    }
    
    public void OnMugOnTray()
    {
        Debug.Log($"OnMugOnTray called! mugInMachine={mugInMachine}, mugOnTray={mugOnTray}");
        if (!mugOnTray && mugInMachine)
        {
            mugOnTray = true;
            CompleteTask(4);
            OnAllTasksComplete();
        }
    }
    
    public void OnMilkSteamed()
    {
        Debug.Log("Milk steaming not available in Level 1");
    }
    
    public void OnMilkPoured()
    {
        Debug.Log("Milk pouring not available in Level 1");
    }
    
    private void CompleteTask(int index)
    {
        Debug.Log($"CompleteTask called for index {index}. taskItems.Count={taskItems.Count}");
        
        if (index < taskItems.Count)
        {
            Debug.Log($"Completing task {index + 1}: {taskItems[index].GetType()}");
            taskItems[index].Complete();
            Debug.Log($"✅ Task {index + 1} completed!");
        }
        else
        {
            Debug.LogError($"❌ Task index {index} out of range! taskItems.Count={taskItems.Count}");
        }
    }
    
    private void OnAllTasksComplete()
    {
        Debug.Log("🎉 LEVEL 1 COMPLETE! 🎉");
    }
}