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
        {
            col.isTrigger = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        MilkFrotherXR frother = other.GetComponent<MilkFrotherXR>();
        if (frother == null)
        {
            frother = other.GetComponentInParent<MilkFrotherXR>();
        }
        
        if (frother != null && frother.HasMilk() && frother.IsSteamed())
        {
            if (mugXR != null && mugXR.HasCoffee())
            {
                milkPourSound?.Play();
                mugXR.AddMilk();
                frother.RemoveMilk();
                taskManager?.OnMilkPoured();
            }
            return;
        }
        
        MatchaBowlController bowl = other.GetComponent<MatchaBowlController>();
        if (bowl == null)
        {
            bowl = other.GetComponentInParent<MatchaBowlController>();
        }
        
        if (bowl != null && bowl.HasMatchaReady())
        {
            if (mugXR != null)
            {
                mugXR.AddMatcha();
                bowl.ResetBowl();
                taskManager?.OnFilterInMachine(); // Task 3
            }
            return;
        }
    }
}