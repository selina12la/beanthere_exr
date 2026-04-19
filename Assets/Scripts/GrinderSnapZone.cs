using UnityEngine;

public class GrinderSnapZone : MonoBehaviour
{
    public EspressoMachineController machine;
    public Transform snapPoint;
    
    private bool isFilterSnapped = false;

    private void OnTriggerEnter(Collider other)
    {
        PortafilterXR portafilter = other.GetComponentInParent<PortafilterXR>();
        
        if (portafilter != null && !isFilterSnapped)
        {
            isFilterSnapped = true;
            portafilter.SetSnapZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PortafilterXR portafilter = other.GetComponentInParent<PortafilterXR>();
        
        if (portafilter != null && isFilterSnapped)
        {
            isFilterSnapped = false;
            portafilter.ClearSnapZone(this);
        }
    }

    public void TrySnapPortafilter(PortafilterXR portafilter)
    {
        if (portafilter == null) return;
        if (portafilter.grabInteractable.isSelected) return;
        if (portafilter.snapAnchor == null)
        {
            return;
        }
        
        if (machine != null)
        {
            machine.SetFilter(true);
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
    }
}