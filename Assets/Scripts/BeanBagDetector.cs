using UnityEngine;

public class BeanBagDetector : MonoBehaviour
{
    public CoffeeGrinderController grinder;
    private bool beansWereDetected = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!beansWereDetected && other.transform.root.CompareTag("CoffeeBeans"))
        {
            beansWereDetected = true;
            grinder.AddBeans();
        }
    }
}