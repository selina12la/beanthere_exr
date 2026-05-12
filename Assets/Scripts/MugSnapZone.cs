using UnityEngine;

public class MugSnapZone : MonoBehaviour
{
    public EspressoMachineController espressoMachine;
    public Transform snapPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Mug"))
        {
            return;
        }

        MugXR mug = other.GetComponent<MugXR>();
        if (mug == null)
            mug = other.GetComponentInParent<MugXR>();
        if (mug != null)
        {
            mug.SetSnapZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MugXR mug = other.GetComponent<MugXR>();
        if (mug == null)
            mug = other.GetComponentInParent<MugXR>();
        if (mug != null)
        {
            mug.ClearSnapZone(this);
        }
    }

    public void TrySnapMug(MugXR mug)
    {
        if (mug == null)
        {
            return;
        }

        if (mug.grabInteractable.isSelected)
        {
            return;
        }

        if (mug.snapAnchor == null)
        {
            return;
        }

        if (espressoMachine != null)
        {
            espressoMachine.SetMug(true, mug);
            
            TaskManagerLocator.Current?.OnMugInMachine();
        }

        Transform root = mug.transform;
        Transform anchor = mug.snapAnchor;
        root.SetParent(null);
        root.rotation = snapPoint.rotation * Quaternion.Inverse(anchor.localRotation);
        root.position = snapPoint.position - (root.rotation * anchor.localPosition);
        root.SetParent(snapPoint, true);
    }
}