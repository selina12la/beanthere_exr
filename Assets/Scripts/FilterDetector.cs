using UnityEngine;

public class FilterDetector : MonoBehaviour
{
    public CoffeeGrinderController grinder;
    public GrinderSnapZone grinderSnapZone;  // NEU: Referenz zur Grinder SnapZone
    
    private PortafilterXR currentPortafilter;
    
    public void OnFilterInserted()
    {
        // Nur ausführen wenn der Portafilter tatsächlich im Grinder ist
        if (currentPortafilter != null && grinder != null)
        {
            // Prüfe ob der Portafilter wirklich die GrinderSnapZone referenziert
            if (currentPortafilter.HasGrinderZone())
            {
                grinder.AddFilter(currentPortafilter);
                Debug.Log("✅ FilterDetector: Added filter to GRINDER");
            }
            else
            {
                Debug.Log("❌ FilterDetector: Portafilter is NOT in grinder zone, ignoring");
            }
        }
        else
        {
            Debug.LogError("Portafilter or Grinder is null!");
        }
    }
    
    public void OnFilterRemoved()
    {
        if (grinder != null)
            grinder.RemoveFilter();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter != null)
        {
            currentPortafilter = portafilter;
            Debug.Log($"FilterDetector: Portafilter entered trigger: {other.gameObject.name}");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter != null && currentPortafilter == portafilter)
        {
            currentPortafilter = null;
            Debug.Log("FilterDetector: Portafilter left trigger");
        }
    }
}