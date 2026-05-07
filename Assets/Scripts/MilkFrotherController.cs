using UnityEngine;

public class MilkFrotherController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource frotherSound;      // Sound beim Steamen
    
    [Header("Visual")]
    public ParticleSystem steamParticles; // Dampf-Particles beim Steamen
    
    [Header("MilkFrother Reference")]
    public MilkFrotherXR currentMilkFrother;
    
    private ITaskListManager taskManager;
    private bool hasFrother = false;
    
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        Debug.Log($"MilkFrotherController started");
    }
    
    public void SetMilkFrotherSnapped(bool state, MilkFrotherXR frother)
    {
        Debug.Log($"SetMilkFrotherSnapped: state={state}, frother={(frother != null ? "exists" : "null")}");
        
        if (!hasFrother && state)
        {
            hasFrother = true;
            currentMilkFrother = frother;
            
            // Sofort steamen (Milch ist schon von Anfang an da)
            StartFrothing();
        }
        else if (!state)
        {
            hasFrother = false;
            currentMilkFrother = null;
            StopFrother();
        }
    }
    
    private void StartFrothing()
    {
        Debug.Log("🔥 Frothing started - IMMEDIATELY STEAMED!");
        
        // Particles starten
        if (steamParticles != null)
        {
            steamParticles.Play();
            Debug.Log("💨 Steam particles started!");
        }
        
        // Sound starten
        if (frotherSound != null)
        {
            frotherSound.Play();
            Debug.Log("🔊 Frother sound started!");
        }
        
        // Sofort fertig
        FinishFrothing();
    }
    
    private void FinishFrothing()
    {
        Debug.Log($"✅ FinishFrothing called! currentMilkFrother={(currentMilkFrother != null ? "exists" : "null")}");
        
        if (currentMilkFrother != null)
        {
            // Milch als gesteamt markieren
            currentMilkFrother.SetSteamed();
            Debug.Log("🥛 Milk steamed and ready to pour!");
            
            // Task 5 vervollständigen
            if (taskManager != null)
            {
                taskManager.OnMilkSteamed();
                Debug.Log("📋 Task 5 completed: Milk steamed!");
            }
            else
            {
                Debug.LogError("❌ taskManager is NULL!");
            }
        }
        
        // Sound und Particles nach kurzer Zeit stoppen (für besseres Feedback)
        Invoke(nameof(StopFrother), 1.5f);
    }
    
    private void StopFrother()
    {
        if (steamParticles != null)
        {
            steamParticles.Stop();
            Debug.Log("💨 Steam particles stopped!");
        }
        
        if (frotherSound != null)
        {
            frotherSound.Stop();
            Debug.Log("🔊 Frother sound stopped!");
        }
    }
}