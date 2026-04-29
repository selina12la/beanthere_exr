using UnityEngine;

public class MugSnapZone : MonoBehaviour
{
    public EspressoMachineController espressoMachine;
    public TaskListManager taskManager;
    public Transform snapPoint;

    void Start()
    {
        Debug.Log($"🔥 MugSnapZone STARTED on {gameObject.name}");
        Debug.Log($"   - Position: {transform.position}");
        Debug.Log($"   - Has Collider: {GetComponent<Collider>() != null}");
    }

    private void OnTriggerEnter(Collider other)
    {
        // NUR auf Mug reagieren!
        if (!other.CompareTag("Mug")) return;
    
        Debug.Log($"🔥🔥🔥 MUG TRIGGER: {other.gameObject.name} (Tag: {other.tag})");
    
        MugXR mug = other.GetComponent<MugXR>();
        if (mug == null)
            mug = other.GetComponentInParent<MugXR>();
    
        if (mug != null)
        {
            Debug.Log("✅ Mug found! Setting snap zone");
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
            // KEIN espressoMachine.SetMug(false) hier!
        }
    }

    public void TrySnapMug(MugXR mug)
    {
        Debug.Log("🔥 TrySnapMug called!");
    
        if (mug == null) return;
        if (mug.grabInteractable.isSelected) return;
        if (mug.snapAnchor == null)
        {
            Debug.LogWarning("MugSnapAnchor fehlt!");
            return;
        }
    
        if (espressoMachine != null)
        {
            espressoMachine.SetMug(true, mug);  // HIER den mug übergeben!
            Debug.Log("✅ Espresso Machine: SetMug(true) called");
        
            if (taskManager != null)
            {
                taskManager.OnMugInMachine();
                Debug.Log("📋 Task 4 completed: Mug in Espresso Machine");
            }
        }

        // Snapping Code...
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
    
        Debug.Log("✅ Mug snapped to Espresso Machine!");
    }
}