using UnityEngine;

public class FilterDetector : MonoBehaviour
{
    public CoffeeGrinderController grinder;
    public GrinderSnapZone grinderSnapZone;
    private PortafilterXR currentPortafilter;

    public void OnFilterInserted()
    {
        if (currentPortafilter != null && grinder != null)
        {
            if (currentPortafilter.HasGrinderZone())
            {
                grinder.AddFilter(currentPortafilter);
            }
        }
    }

    public void OnFilterRemoved()
    {
        if (grinder != null)
        {
            grinder.RemoveFilter();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter != null)
        {
            currentPortafilter = portafilter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter != null && currentPortafilter == portafilter)
        {
            currentPortafilter = null;
        }
    }
}