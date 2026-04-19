using UnityEngine;

public class EspressoFilterDetector : MonoBehaviour
{
    public EspressoMachineController machine;
    
    public void OnFilterInserted()
    {
        if (machine != null)
            machine.SetFilter(true);
    }
    
    public void OnFilterRemoved()
    {
        if (machine != null)
            machine.SetFilter(false);
    }
}