using UnityEngine;

public class EspressoMachineController : MonoBehaviour
{
    public bool mugInserted = false;
    public bool filterInserted = false;

    public ParticleSystem coffeeParticles;
    public AudioSource coffeeSound;

    private bool hasStarted = false;

    public void SetMug(bool state)
    {
        mugInserted = state;
        Debug.Log("MUG: " + state);
        TryStart();
    }

    public void SetFilter(bool state)
    {
        filterInserted = state;
        Debug.Log("FILTER: " + state);
        TryStart();
    }

    private void TryStart()
    {
        if (hasStarted) return;

        if (mugInserted && filterInserted)
        {
            StartCoffee();
        }
    }

    private void StartCoffee()
    {
        hasStarted = true;

        Debug.Log("Coffee started automatically!");

        if (coffeeParticles != null)
            coffeeParticles.Play();

        if (coffeeSound != null)
            coffeeSound.Play();
    }
}