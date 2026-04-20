using UnityEngine;
using TMPro;
 
public class TaskItem : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    public GameObject checkmarkIcon;  // Optional: Häkchen Icon
    private string taskDescription;
    private bool isCompleted = false;
    public void Initialize(string description)
    {
        taskDescription = description;
        if (taskText != null)
            taskText.text = description;
        if (checkmarkIcon != null)
            checkmarkIcon.SetActive(false);
    }
    public void Complete()
    {
        if (isCompleted) return;
        isCompleted = true;
        // Text durchstreichen
        if (taskText != null)
        {
            taskText.text = $"<s>{taskDescription}</s>";
            taskText.color = Color.gray;
        }
        // Häkchen zeigen
        if (checkmarkIcon != null)
            checkmarkIcon.SetActive(true);
        Debug.Log($"✅ Task completed: {taskDescription}");
    }
    public bool IsCompleted()
    {
        return isCompleted;
    }
}