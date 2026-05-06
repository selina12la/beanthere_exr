using DefaultNamespace;
using UnityEngine;
 
public class MugSnapZone : MonoBehaviour
{
    public EspressoMachineController espressoMachine;
    public Transform snapPoint;
    private ITaskListManager taskManager;  // ← Interface
 
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        Debug.Log($"MugSnapZone STARTED on {gameObject.name}");
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Mug")) return;
        Debug.Log($"MUG TRIGGER: {other.gameObject.name}");
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
        Debug.Log("TrySnapMug called!");
        if (mug == null) return;
        if (mug.grabInteractable.isSelected) return;
        if (mug.snapAnchor == null)
        {
            Debug.LogWarning("MugSnapAnchor fehlt!");
            return;
        }
        if (espressoMachine != null)
        {
            espressoMachine.SetMug(true, mug);
            Debug.Log("Espresso Machine: SetMug(true) called");
            if (taskManager != null)
            {
                taskManager.OnMugInMachine();
            }
        }
        Transform root = mug.transform;
        Transform anchor = mug.snapAnchor;
        root.SetParent(null);
        root.rotation = snapPoint.rotation * Quaternion.Inverse(anchor.localRotation);
        root.position = snapPoint.position - (root.rotation * anchor.localPosition);
        root.SetParent(snapPoint, true);
        Debug.Log("Mug snapped to Espresso Machine!");
    }
}