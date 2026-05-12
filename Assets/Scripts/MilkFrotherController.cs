using UnityEngine;

public class MilkFrotherController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource frotherSound;
    
    [Header("Visual")]
    public ParticleSystem steamParticles;
    
    [Header("MilkFrother Reference")]
    public MilkFrotherXR currentMilkFrother;
    
    private bool hasFrother = false;
    
    public void SetMilkFrotherSnapped(bool state, MilkFrotherXR frother)
    {
        if (!hasFrother && state)
        {
            hasFrother = true;
            currentMilkFrother = frother;
            
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
        if (steamParticles != null)
        {
            steamParticles.Play();
        }
        
        if (frotherSound != null)
        {
            frotherSound.Play();
        }
        
        FinishFrothing();
    }
    
    private void FinishFrothing()
    {
        if (currentMilkFrother != null)
        {
            currentMilkFrother.SetSteamed();
            
            // Task 5
            TaskManagerLocator.Current?.OnMilkSteamed();
        }
        Invoke(nameof(StopFrother), 5f);
    }
    
    private void StopFrother()
    {
        if (steamParticles != null)
        {
            steamParticles.Stop();
        }
        
        if (frotherSound != null)
        {
            frotherSound.Stop();
        }
    }
}