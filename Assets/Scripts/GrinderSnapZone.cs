using UnityEngine;

public class GrinderSnapZone : MonoBehaviour
{
    public CoffeeGrinderController grinder;
    public Transform snapPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Portafilter"))
        {
            return;
        }

        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter != null)
        {
            portafilter.SetGrinderSnapZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();
        if (portafilter != null)
        {
            portafilter.ClearGrinderSnapZone();
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

        if (grinder != null)
        {
            grinder.AddFilter(portafilter);
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