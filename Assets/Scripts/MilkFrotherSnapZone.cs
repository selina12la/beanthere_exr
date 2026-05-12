using UnityEngine;

public class MilkFrotherSnapZone : MonoBehaviour
{
    public MilkFrotherController milkFrotherController;
    public Transform snapPoint;
    public MilkSteamer milkSteamer;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("MilkFrother"))
        {
            return;
        }

        MilkFrotherXR frother = other.GetComponent<MilkFrotherXR>();
        if (frother == null)
        {
            frother = other.GetComponentInParent<MilkFrotherXR>();
        }

        if (frother != null)
        {
            frother.SetSnapZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MilkFrotherXR frother = other.GetComponent<MilkFrotherXR>();
        if (frother == null)
        {
            frother = other.GetComponentInParent<MilkFrotherXR>();
        }

        if (frother != null)
        {
            frother.ClearSnapZone(this);
        }
    }

    public void TrySnapMilkFrother(MilkFrotherXR frother)
    {
        if (frother == null)
        {
            return;
        }

        if (frother.grabInteractable.isSelected)
        {
            return;
        }

        if (frother.snapAnchor == null)
        {
            return;
        }

        if (milkFrotherController != null)
        {
            milkFrotherController.SetMilkFrotherSnapped(true, frother);
        }

        Transform root = frother.transform;
        Transform anchor = frother.snapAnchor;

        Rigidbody rb = frother.rb;
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