using UnityEngine;

public class FilterDetector : MonoBehaviour
{
    public CoffeeGrinderController grinder;
    public PortafilterXR currentPortafilter;  // NEU: Referenz zum Portafilter
    
    public void OnFilterInserted()
    {
        // Finde den Portafilter
        if (currentPortafilter == null)
            currentPortafilter = FindObjectOfType<PortafilterXR>();
        
        if (currentPortafilter != null && grinder != null)
        {
            grinder.AddFilter(currentPortafilter);  // JETZT mit Parameter
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
    
    // Automatisch den Portafilter erkennen wenn er in Trigger kommt
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