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
        CreateTaskList();
    }
    
    private void CreateTaskList()
    {
        if (taskItemPrefab == null)
        {
            return;
        }
        
        if (taskContainer == null)
        {
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
            }
        }
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
    
    public void OnMugOnTray()
    {
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
        if (index < taskItems.Count)
        {
            taskItems[index].Complete();
        }
    }
    
    private void OnAllTasksComplete()
    {
        Debug.Log("LEVEL 1 COMPLETE!");
    }
}