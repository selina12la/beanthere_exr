using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FilterDetector : MonoBehaviour
{
    public CoffeeGrinderController grinder;
    
    public void OnFilterInserted()
    {
        grinder.AddFilter();
    }
    
    public void OnFilterRemoved()
    {
        grinder.RemoveFilter();
    }
}