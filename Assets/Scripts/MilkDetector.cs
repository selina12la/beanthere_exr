// MilkDetector.cs - Mit Pouring Sound
using UnityEngine;
 
public class MilkDetector : MonoBehaviour
{
    private ITaskListManager taskManager;
    private MugXR mugXR;
    
    [Header("Audio")]
    public AudioSource milkPourSound;  // ← Neu: Sound beim Eingießen
    
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        mugXR = GetComponentInParent<MugXR>();
        
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
        }
        
        Debug.Log($"✅ MilkDetector initialized on {transform.parent?.name ?? gameObject.name}");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"🔍 MilkDetector triggered by: {other.gameObject.name} (Tag: '{other.tag}')");
        
        MilkFrotherXR frother = other.GetComponent<MilkFrotherXR>();
        if (frother == null)
            frother = other.GetComponentInParent<MilkFrotherXR>();
        
        if (frother != null)
        {
            Debug.Log($"✅ Found MilkFrotherXR! HasMilk={frother.HasMilk()}, IsSteamed={frother.IsSteamed()}");
            
            if (frother.HasMilk() && frother.IsSteamed())
            {
                Debug.Log("🥛 Conditions met! Pouring milk...");
                
                // ✨ Pouring Sound abspielen
                if (milkPourSound != null)
                {
                    milkPourSound.Play();
                    Debug.Log("🔊 Milk pouring sound played!");
                }
                
                if (mugXR != null && mugXR.HasCoffee())
                {
                    mugXR.AddMilk();
                    frother.RemoveMilk();
                    
                    if (taskManager != null)
                    {
                        taskManager.OnMilkPoured();
                        Debug.Log("📋 Task 6 completed: Milk poured!");
                    }
                }
                else if (mugXR == null)
                {
                    Debug.LogError("❌ mugXR is null!");
                }
                else if (!mugXR.HasCoffee())
                {
                    Debug.LogWarning("⚠️ No coffee in mug yet! Pour milk after coffee is made.");
                }
            }
            else
            {
                Debug.Log($"⏳ Cannot pour: HasMilk={frother.HasMilk()}, IsSteamed={frother.IsSteamed()} (Both need to be true)");
            }
        }
    }
}