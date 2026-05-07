using UnityEngine;
using System.Collections;

public class EspressoMachineController : MonoBehaviour
{
    public ParticleSystem coffeeParticles;
    public AudioSource coffeeSound;
    public float brewingDuration = 15f;
    public bool loopSound = true;
    
    private bool mugInserted = false;
    private bool filterInserted = false;
    private MugXR currentMug;
    private bool isBrewing = false;
    private Coroutine brewingCoroutine;

    public void SetFilter(bool state)
    {
        filterInserted = state;
        CheckStartCondition();
    }

    public void SetMug(bool state, MugXR mug = null)
    {
        mugInserted = state;
        if (mug != null)
        {
            currentMug = mug;
        }

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
            if (loopSound)
            {
                coffeeSound.loop = true;
            }

            coffeeSound.Play();
        }

        if (brewingCoroutine != null)
        {
            StopCoroutine(brewingCoroutine);
        }

        brewingCoroutine = StartCoroutine(StopCoffeeAfterDelay(brewingDuration));
    }

    private IEnumerator StopCoffeeAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (coffeeParticles != null)
        {
            coffeeParticles.Stop();
        }

        if (coffeeSound != null)
        {
            coffeeSound.loop = false;
            coffeeSound.Stop();
        }

        isBrewing = false;

        if (currentMug != null)
        {
            currentMug.AddCoffee();
        }

        mugInserted = false;
        filterInserted = false;
    }

    public void ResetMachine()
    {
        if (brewingCoroutine != null)
        {
            StopCoroutine(brewingCoroutine);
        }

        isBrewing = false;

        if (coffeeParticles != null)
        {
            coffeeParticles.Stop();
        }

        if (coffeeSound != null)
        {
            coffeeSound.loop = false;
            coffeeSound.Stop();
        }

        mugInserted = false;
        filterInserted = false;
    }
}