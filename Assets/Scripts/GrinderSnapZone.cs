using UnityEngine;

public class GrinderSnapZone : MonoBehaviour
{
    public Transform snapPoint;

    private void OnTriggerEnter(Collider other)
    {
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();

        if (portafilter != null)
        {
            portafilter.SetSnapZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PortafilterXR portafilter = other.GetComponent<PortafilterXR>();

        if (portafilter != null)
        {
            portafilter.ClearSnapZone(this);
        }
    }

    public void TrySnapPortafilter(PortafilterXR portafilter)
    {
        if (portafilter == null)
            return;

        if (portafilter.grabInteractable.isSelected)
            return;

        if (portafilter.snapAnchor == null)
        {
            Debug.LogWarning("PortafilterSnapAnchor fehlt!");
            return;
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