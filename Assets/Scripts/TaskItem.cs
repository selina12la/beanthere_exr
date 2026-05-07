using UnityEngine;
using TMPro;

public class TaskItem : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    private string taskDescription;
    private bool isCompleted = false;
    
    public void Initialize(string description)
    {
        taskDescription = description;
        
        if (taskText == null)
        {
            taskText = GetComponent<TextMeshProUGUI>();
            if (taskText == null)
            {
                taskText = GetComponentInChildren<TextMeshProUGUI>();
            }
        }
        
        if (taskText != null)
        {
            taskText.text = description;
            Debug.Log($"TaskItem initialized: '{description}'");
        }
        else
        {
            Debug.LogError($"❌ TaskItem: No TextMeshProUGUI found on {gameObject.name}!");
        }
    }
    
    public void Complete()
    {
        if (isCompleted) return;
        isCompleted = true;
        
        if (taskText != null)
        {
            taskText.text = $"<s>{taskDescription}</s>";
            taskText.color = Color.gray;
            Debug.Log($"✅ Task completed (UI updated): {taskDescription}");
        }
        else
        {
            Debug.LogError($"❌ Cannot complete task - taskText is NULL! Task: {taskDescription}");
        }
    }
}