using UnityEngine;
 
public class CoffeeGrinderTrigger : MonoBehaviour
{
    public ParticleSystem coffeeParticles;
    public AudioSource grinderSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("CoffeeBeans"))
        {
            coffeeParticles.Play();
            grinderSound.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("CoffeeBeans"))
        {
            coffeeParticles.Stop();
            grinderSound.Stop();
        }
    }
}