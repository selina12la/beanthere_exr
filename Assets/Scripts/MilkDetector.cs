using DefaultNamespace;
using UnityEngine;
public class MilkDetector : MonoBehaviour
{
    private ITaskListManager taskManager;  // ← Interface
    private MugXR mugXR;
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        mugXR = GetComponentInParent<MugXR>();
        if (mugXR == null)
        {
            Debug.LogError("MilkDetector: No MugXR found on parent!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MilkFrother"))
        {
            MilkFrotherXR frother = other.GetComponent<MilkFrotherXR>();
            if (frother != null && frother.HasMilk() && frother.IsSteamed())
            {
                Debug.Log("🥛 Milk frother detected over mug! Pouring milk...");
                if (mugXR != null)
                {
                    mugXR.AddMilk();
                    Debug.Log("✅ Milk added to mug!");
                }
                frother.RemoveMilk();
                Debug.Log("✅ Milk removed from frother!");
                if (taskManager != null)
                {
                    taskManager.OnMilkPoured();
                    Debug.Log("📋 Task 6 completed: Milk poured into coffee!");
                }
            }
            else if (frother != null && !frother.HasMilk())
            {
                Debug.Log("No milk in frother!");
            }
            else if (frother != null && !frother.IsSteamed())
            {
                Debug.Log("Milk not steamed yet!");
            }
        }
    }
}