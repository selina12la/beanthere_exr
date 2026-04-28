using UnityEngine;

public class FilterSnapZone : MonoBehaviour
{
    public EspressoMachineController espressoMachine;
    public TaskListManager taskManager;
    public Transform snapPoint;
    
    void Start()
    {
        Debug.Log($"🔥 FilterSnapZone STARTED on {gameObject.name}");
        Debug.Log($"   - Position: {transform.position}");
        Debug.Log($"   - Has Collider: {GetComponent<Collider>() != null}");
        
        if (GetComponent<Collider>() != null)
        {
            Debug.Log($"   - Collider IsTrigger: {GetComponent<Collider>().isTrigger}");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // NUR auf Portafilter reagieren!
        if (!other.CompareTag("Portafilter")) return;
    
        Debug.Log($"🔥🔥🔥 ONTRIGGERENTER: {other.gameObject.name} (Tag: {other.tag})");
    
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter == null)
            portafilter = other.GetComponentInParent<PortafilterXR>();
    
        if (portafilter != null)
        {
            Debug.Log("✅✅✅ Portafilter found in trigger! ✅✅✅");
            portafilter.SetEspressoSnapZone(this);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"🔥 ONTRIGGEREXIT: {other.gameObject.name}");
        
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter == null)
            portafilter = other.GetComponentInParent<PortafilterXR>();
        
        if (portafilter != null)
        {
            portafilter.ClearEspressoSnapZone();
            // KEIN espressoMachine.SetFilter(false) hier!
        }
    }
    
    public void TrySnapPortafilter(PortafilterXR portafilter)
    {
        Debug.Log("🔥 TrySnapPortafilter called on FilterSnapZone!");
        
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
            Debug.Log("✅ Espresso Machine SetFilter(true) called");
        }
        
        // Task 3: Filter in Espresso Machine
        if (taskManager != null)
        {
            taskManager.OnFilterInMachine();
            Debug.Log("📋 Task 3 completed!");
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
        
        Debug.Log("✅ Portafilter snapped to Espresso Machine!");
    }
}