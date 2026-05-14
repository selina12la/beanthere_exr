using UnityEngine;

public class MatchaMilkDetector : MonoBehaviour
{
    private ITaskListManager taskManager;
    private MugXR mugXR;
    
    [Header("Audio")]
    public AudioSource milkPourSound;
    public MatchaBowlController matchaBowl;
    
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        mugXR = GetComponentInParent<MugXR>();
        
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
            col.isTrigger = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"MatchaMilkDetector: {other.name} (Tag: {other.tag})");
        
        // 1. Prüfe auf MilkFrother (Milch einfüllen)
        MilkFrotherXR frother = other.GetComponent<MilkFrotherXR>();
        if (frother == null)
            frother = other.GetComponentInParent<MilkFrotherXR>();
        
        if (frother != null && frother.HasMilk() && frother.IsSteamed())
        {
            if (mugXR != null && mugXR.HasCoffee())
            {
                milkPourSound?.Play();
                mugXR.AddMilk();
                frother.RemoveMilk();
                taskManager?.OnMilkPoured();
                Debug.Log("✅ Milk poured into mug! Task 5 complete");
            }
            return;
        }
        
        // 2. Prüfe auf Bowl (Matcha einfüllen)
        MatchaBowlController bowl = other.GetComponent<MatchaBowlController>();
        if (bowl == null)
            bowl = other.GetComponentInParent<MatchaBowlController>();
        
        if (bowl != null && bowl.HasMatchaReady())
        {
            if (mugXR != null)
            {
                mugXR.AddMatcha();
                bowl.ResetBowl();
                taskManager?.OnFilterInMachine(); // Task 3
                Debug.Log("✅ Matcha poured into mug! Task 3 complete");
            }
            return;
        }
    }
}