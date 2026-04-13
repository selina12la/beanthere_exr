using UnityEngine;
 
public class EspressoMachineController : MonoBehaviour
{
    public bool mugInserted = false;
    public bool filterInserted = false;
 
    public ParticleSystem coffeeParticles;
    public AudioSource coffeeSound;
 
    public void SetMug(bool state)
    {
        mugInserted = state;
    }
 
    public void SetFilter(bool state)
    {
        filterInserted = state;
    }
 
    public void StartCoffee()
    {
        if (!mugInserted || !filterInserted)
        {
            Debug.Log("Missing mug or filter!");
            return;
        }
 
        Debug.Log("Coffee started!");
 
        if (coffeeParticles != null)
            coffeeParticles.Play();
 
        if (coffeeSound != null)
            coffeeSound.Play();
    }
}