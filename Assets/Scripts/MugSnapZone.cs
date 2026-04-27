using UnityEngine;

public class MugSnapZone : MonoBehaviour
{
    public EspressoMachineController espressoMachine;
    public TaskListManager taskManager;
    public Transform snapPoint;

    private void OnTriggerEnter(Collider other)
    {
        MugXR mug = other.GetComponent<MugXR>();
        if (mug != null)
        {
            mug.SetSnapZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MugXR mug = other.GetComponent<MugXR>();
        if (mug != null)
        {
            mug.ClearSnapZone(this);
            if (espressoMachine != null)
                espressoMachine.SetMug(false);
        }
    }

    public void TrySnapMug(MugXR mug)
    {
        if (mug == null) return;
        if (mug.grabInteractable.isSelected) return;
        if (mug.snapAnchor == null)
        {
            Debug.LogWarning("MugSnapAnchor fehlt!");
            return;
        }
        
        Debug.Log("✅ Mug snapped into Espresso Machine");
        
        if (espressoMachine != null)
        {
            espressoMachine.SetMug(true);
            
            if (taskManager != null)
            {
                taskManager.OnMugInMachine();
                Debug.Log("📋 Task 4 completed: Mug in Espresso Machine");
            }
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