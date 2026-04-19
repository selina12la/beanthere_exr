using UnityEngine;

public class MugSnapZone : MonoBehaviour
{
    public EspressoMachineController machine;
    public Transform snapPoint;
    
    private bool isMugSnapped = false;

    private void OnTriggerEnter(Collider other)
    {
        MugXR mug = other.GetComponentInParent<MugXR>();
        
        if (mug != null && !isMugSnapped)
        {
            isMugSnapped = true;
            mug.SetSnapZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MugXR mug = other.GetComponentInParent<MugXR>();
        
        if (mug != null && isMugSnapped)
        {
            isMugSnapped = false;
            mug.ClearSnapZone(this);
        }
    }

    public void TrySnapMug(MugXR mug)
    {
        if (mug == null) return;
        if (mug.grabInteractable.isSelected) return;
        if (mug.snapAnchor == null)
        {
            return;
        }
        
        if (machine != null)
        {
            machine.SetMug(true);
        }

        Transform root = mug.transform;
        Transform anchor = mug.snapAnchor;

        Rigidbody rb = mug.rb;
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