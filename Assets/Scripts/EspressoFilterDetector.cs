using UnityEngine;

public class EspressoFilterDetector : MonoBehaviour
{
    public EspressoMachineController machine;

    public void OnFilterInserted()
    {
        machine.SetFilter(true);
    }

    public void OnFilterRemoved()
    {
        machine.SetFilter(false);
    }
}