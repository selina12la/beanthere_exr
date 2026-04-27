using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FilterDetector : MonoBehaviour
{
    public CoffeeGrinderController grinder;
    public PortafilterXR currentPortafilter;
    
    public void OnFilterInserted()
    {
        grinder.AddFilter(currentPortafilter);
    }
    
    public void OnFilterRemoved()
    {
        grinder.RemoveFilter();
    }
}