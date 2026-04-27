using UnityEngine;

public class FilterSnapZone : MonoBehaviour
{
    public EspressoMachineController espressoMachine;
    public TaskListManager taskManager;
    public Transform snapPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter != null)
        {
            portafilter.SetEspressoSnapZone(this);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter != null)
        {
            portafilter.ClearEspressoSnapZone();
            if (espressoMachine != null)
                espressoMachine.SetFilter(false);
        }
    }
    
    public void TrySnapPortafilter(PortafilterXR portafilter)
    {
        if (portafilter == null) return;
        if (portafilter.grabInteractable.isSelected) return;
        if (portafilter.snapAnchor == null)
        {
            Debug.LogWarning("PortafilterSnapAnchor fehlt!");
            return;
        }
        
        // Espresso Machine benachrichtigen
        if (espressoMachine != null)
        {
            espressoMachine.SetFilter(true);
        }
        
        // Task 3: Filter in Espresso Machine
        if (taskManager != null)
        {
            taskManager.OnFilterInMachine();
        }
        
        // Snapping Code
        Transform root = portafilter.transform;
        Transform anchor = portafilter.snapAnchor;
        
        Rigidbody rb = portafilter.rb;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        root.SetParent(null);
        root.rotation = snapPoint.rotation * Quaternion.Inverse(anchor.localRotation);
        root.position = snapPoint.position - (root.rotation * anchor.localPosition);
        root.SetParent(snapPoint, true);
    }
}