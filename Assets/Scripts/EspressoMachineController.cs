using UnityEngine;
using System.Collections;
public class EspressoMachineController : MonoBehaviour
{
    private bool mugInserted = false;
    private bool filterInserted = false;
 
    public ParticleSystem coffeeParticles;
    public AudioSource coffeeSound;
    public float brewingDuration = 15f;
    public bool loopSound = true;
    private bool isBrewing = false;
    private Coroutine brewingCoroutine;
    
    public void SetFilter(bool state)
    {
        filterInserted = state;
        Debug.Log($"🔵 ESPRESSO SetFilter called: {state} (mug={mugInserted}, filter={filterInserted})");
        CheckStartCondition();
    }

    public void SetMug(bool state)
    {
        mugInserted = state;
        Debug.Log($"🔵 ESPRESSO SetMug called: {state} (mug={mugInserted}, filter={filterInserted})");
        CheckStartCondition();
    }

    private void CheckStartCondition()
    {
        Debug.Log($"🔍 CheckStartCondition: mug={mugInserted}, filter={filterInserted}, isBrewing={isBrewing}");
    
        // WICHTIG: Prüfe ob BEIDE true sind
        if (mugInserted && filterInserted && !isBrewing)
        {
            Debug.Log("✅✅✅ STARTING ESPRESSO MACHINE! ✅✅✅");
            StartCoffee();
        }
    }
    private void StartCoffee()
    {
        isBrewing = true;
        Debug.Log($"☕ Coffee started for {brewingDuration} seconds!");
        if (coffeeParticles != null)
            coffeeParticles.Play();
        if (coffeeSound != null)
        {
            if (loopSound)
                coffeeSound.loop = true;
            coffeeSound.Play();
        }
        if (brewingCoroutine != null)
            StopCoroutine(brewingCoroutine);
        brewingCoroutine = StartCoroutine(StopCoffeeAfterDelay(brewingDuration));
    }
    private IEnumerator StopCoffeeAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (coffeeParticles != null)
            coffeeParticles.Stop();
        if (coffeeSound != null)
        {
            coffeeSound.loop = false;
            coffeeSound.Stop();
        }
        isBrewing = false;
        Debug.Log("☕ Coffee finished!");
    }
    
    public void ResetMachine()
    {
        if (brewingCoroutine != null)
            StopCoroutine(brewingCoroutine);
        isBrewing = false;
    
        if (coffeeParticles != null)
            coffeeParticles.Stop();
        if (coffeeSound != null)
        {
            coffeeSound.loop = false;
            coffeeSound.Stop();
        }
        mugInserted = false;
        filterInserted = false;
        Debug.Log("Espresso machine reset!");
    }
}