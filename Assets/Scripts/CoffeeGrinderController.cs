using UnityEngine;

public class CoffeeGrinderController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource beansPourSound;
    public AudioSource filterSnapSound;
    public AudioSource grinderSound;
    
    [Header("Visual")]
    public ParticleSystem beansParticles;
    public ParticleSystem grindParticles;
    
    private bool hasBeans = false;
    private bool hasFilter = false;
    
    public void AddBeans()
    {
        if (!hasBeans)
        {
            hasBeans = true;
            beansPourSound.Play();
            beansParticles.Play();
            CheckGrinderReady();
        }
    }
    
    public void RemoveBeans()
    {
        hasBeans = false;
        beansParticles.Stop();
        StopGrinder();
    }
    
    public void AddFilter()
    {
        if (!hasFilter)
        {
            hasFilter = true;
            filterSnapSound.Play();
            CheckGrinderReady();
        }
    }
    
    public void RemoveFilter()
    {
        hasFilter = false;
        StopGrinder();
    }
    
    private void CheckGrinderReady()
    {
        if (hasBeans && hasFilter)
        {
                grinderSound.Play();
                grindParticles.Play();
        }
    }
    
    private void StopGrinder()
    {
        grinderSound.Stop();
        grindParticles.Stop();
    }
}