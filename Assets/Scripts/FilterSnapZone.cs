using DefaultNamespace;
using UnityEngine;
 
public class FilterSnapZone : MonoBehaviour
{
    public EspressoMachineController espressoMachine;
    public Transform snapPoint;
    private ITaskListManager taskManager;  // ← Interface
 
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        Debug.Log($"FilterSnapZone STARTED on {gameObject.name}");
        Debug.Log($"   - Position: {transform.position}");
        Debug.Log($"   - Has Collider: {GetComponent<Collider>() != null}");
        if (GetComponent<Collider>() != null)
        {
            Debug.Log($"   - Collider IsTrigger: {GetComponent<Collider>().isTrigger}");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Portafilter")) return;
        Debug.Log($"ONTRIGGERENTER: {other.gameObject.name} (Tag: {other.tag})");
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter == null)
            portafilter = other.GetComponentInParent<PortafilterXR>();
        if (portafilter != null)
        {
            Debug.Log("Portafilter found in trigger!");
            portafilter.SetEspressoSnapZone(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"ONTRIGGEREXIT: {other.gameObject.name}");
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter == null)
            portafilter = other.GetComponentInParent<PortafilterXR>();
        if (portafilter != null)
        {
            portafilter.ClearEspressoSnapZone();
        }
    }
    public void TrySnapPortafilter(PortafilterXR portafilter)
    {
        Debug.Log("TrySnapPortafilter called on FilterSnapZone!");
        if (portafilter == null) return;
        if (portafilter.grabInteractable.isSelected) return;
        if (portafilter.snapAnchor == null)
        {
            Debug.LogWarning("PortafilterSnapAnchor fehlt!");
            return;
        }
        if (espressoMachine != null)
        {
            espressoMachine.SetFilter(true);
            Debug.Log("Espresso Machine SetFilter(true) called");
        }
        if (taskManager != null)
        {
            taskManager.OnFilterInMachine();
            Debug.Log("Task 3 completed!");
        }
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
        Debug.Log("Portafilter snapped to Espresso Machine!");
    }
}