using UnityEngine;
using System.Collections;

public class EspressoMachineController : MonoBehaviour
{
    private bool mugInserted = false;
    private bool filterInserted = false;

    public ParticleSystem coffeeParticles;
    public AudioSource coffeeSound;
    
    public float brewingDuration = 15f;
    
    private bool isBrewing = false;

    public void SetMug(bool state)
    {
        mugInserted = state;
        CheckStartCondition();
    }

    public void SetFilter(bool state)
    {
        filterInserted = state;
        CheckStartCondition();
    }
    
    private void CheckStartCondition()
    {
        if (mugInserted && filterInserted && !isBrewing)
        {
            StartCoffee();
        }
    }
    
    private void StartCoffee()
    {
        isBrewing = true;

        if (coffeeParticles != null)
        {
            coffeeParticles.Play();
        }

        if (coffeeSound != null)
        {
            coffeeSound.Play();
        }

        StartCoroutine(StopCoffeeAfterDelay());
    }
    
    private IEnumerator StopCoffeeAfterDelay()
    {
        yield return new WaitForSeconds(brewingDuration);

        if (coffeeParticles != null)
        {
            coffeeParticles.Stop();
        }

        if (coffeeSound != null)
        {
            coffeeSound.Stop();
        }

        isBrewing = false;
        
        mugInserted = false;
        filterInserted = false;
    }
}