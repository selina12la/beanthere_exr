using UnityEngine;
using System.Collections.Generic;
 
public class TaskListManager : MonoBehaviour
{
    [Header("References")]
    public GameObject taskItemPrefab;     // Prefab für einen Task
    public Transform taskContainer;        // Parent für die Task-Liste (z.B. ein Vertical Layout Group)
    [Header("Task Definitions")]
    public string[] taskDescriptions = new string[]
    {
        "1. Hover coffee bag over grinder",
        "2. Snap filter into grinder",
        "3. Move filter to espresso machine",
        "4. Snap mug into espresso machine",
        "5. Place mug on serving tray"
    };
    private List<TaskItem> taskItems = new List<TaskItem>();
    // Task States
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
        // Lösche vorhandene Tasks
        foreach (Transform child in taskContainer)
            Destroy(child.gameObject);
        taskItems.Clear();
        // Erstelle neue Tasks
        foreach (string description in taskDescriptions)
        {
            GameObject taskObj = Instantiate(taskItemPrefab, taskContainer);
            TaskItem taskItem = taskObj.GetComponent<TaskItem>();
            if (taskItem != null)
            {
                taskItem.Initialize(description);
                taskItems.Add(taskItem);
            }
        }
    }
    // Task 1
    public void OnBeansPoured()
    {
        if (!beansPoured)
        {
            beansPoured = true;
            CompleteTask(0); 
        }
    }
    // Task 2
    public void OnFilterGround()
    {
        if (!filterGround && beansPoured) 
        {
            filterGround = true;
            CompleteTask(1); 
        }
    }
    // Task 3
    public void OnFilterInMachine()
    {
        if (!filterInMachine && filterGround) 
        {
            filterInMachine = true;
            CompleteTask(2); 
        }
    }
   //Task 4
    public void OnMugInMachine()
    {
        if (!mugInMachine && filterInMachine) 
        {
            mugInMachine = true;
            CompleteTask(3); 
        }
    }
  // Task 5
    public void OnMugOnTray()
    {
        if (!mugOnTray && mugInMachine) 
        {
            mugOnTray = true;
            CompleteTask(4); 
            // Alle Tasks fertig!
            OnAllTasksComplete();
        }
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
        Debug.Log("🎉 ALL TASKS COMPLETE! 🎉");
        Debug.Log("Great job! You made a perfect espresso!");
    }
    public void ResetTasks()
    {
        beansPoured = false;
        filterGround = false;
        filterInMachine = false;
        mugInMachine = false;
        mugOnTray = false;
        CreateTaskList();
    }
}