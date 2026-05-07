using UnityEngine;
using System.Collections.Generic;

public class TaskListManagerLevel2 : MonoBehaviour, ITaskListManager
{
    [Header("References")]
    public GameObject taskItemPrefab;
    public Transform taskContainer;
    
    private List<TaskItem> taskItems = new List<TaskItem>();
    
    private bool beansPoured = false;
    private bool filterGround = false;
    private bool filterInMachine = false;
    private bool mugInMachine = false;
    private bool milkSteamed = false;
    private bool milkPoured = false;
    private bool mugOnTray = false;
    
    private void Start()
    {
        CreateTaskList();
    }
    
    private void CreateTaskList()
    {
        foreach (Transform child in taskContainer)
            Destroy(child.gameObject);
        taskItems.Clear();
        
        string[] taskDescriptions = new string[]
        {
            "1. Hover coffee bag over grinder",
            "2. Snap filter into grinder",
            "3. Move filter to espresso machine",
            "4. Snap mug into espresso machine",
            "5. Steam milk",
            "6. Pour milk into coffee cup",
            "7. Place mug on serving tray"
        };
        
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
                Debug.Log($"Task created: {description}");
            }
            else
            {
                Debug.LogError($"TaskItem component not found on prefab! Prefab: {taskItemPrefab.name}");
            }
        }
        
        Debug.Log($"Total tasks created: {taskItems.Count}");
    }
    
    public void OnBeansPoured()
    {
        if (!beansPoured)
        {
            beansPoured = true;
            CompleteTask(0);
        }
    }
    
    public void OnFilterGround()
    {
        if (!filterGround && beansPoured)
        {
            filterGround = true;
            CompleteTask(1);
        }
    }
    
    public void OnFilterInMachine()
    {
        if (!filterInMachine && filterGround)
        {
            filterInMachine = true;
            CompleteTask(2);
        }
    }
    
    public void OnMugInMachine()
    {
        if (!mugInMachine && filterInMachine)
        {
            mugInMachine = true;
            CompleteTask(3);
        }
    }
    
    public void OnMilkSteamed()
    {
        if (!milkSteamed && mugInMachine)
        {
            milkSteamed = true;
            CompleteTask(4);
            Debug.Log("🥛 Milk steamed!");
        }
    }
    
    public void OnMilkPoured()
    {
        Debug.Log($"🔥 OnMilkPoured called! milkSteamed={milkSteamed}, milkPoured={milkPoured}");
    
        if (!milkPoured && milkSteamed)
        {
            milkPoured = true;
            CompleteTask(5);
            Debug.Log("🥛 Milk poured! Task completed!");
        }
        else
        {
            Debug.Log($"Cannot pour milk: milkSteamed={milkSteamed}, milkPoured={milkPoured}");
        }
    }
    
    public void OnMugOnTray()
    {
        if (!mugOnTray && milkPoured)
        {
            mugOnTray = true;
            CompleteTask(6);
            OnAllTasksComplete();
        }
    }
    
    private void CompleteTask(int index)
    {
        if (index < taskItems.Count)
        {
            taskItems[index].Complete();
            Debug.Log($"Task {index + 1} completed!");
        }
        else
        {
            Debug.LogError($"Task index {index} out of range! taskItems.Count={taskItems.Count}");
        }
    }
    
    private void OnAllTasksComplete()
    {
        Debug.Log("🎉 LEVEL 2 COMPLETE! 🎉");
    }
}