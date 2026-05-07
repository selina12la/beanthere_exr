using UnityEngine;
 
public class MilkFrotherSnapZone : MonoBehaviour
{
    public MilkFrotherController milkFrotherController; 
    public Transform snapPoint;
    public MilkSteamer milkSteamer;
 
    private ITaskListManager taskManager;  // ← Interface
 
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
        Debug.Log($"MilkFrotherSnapZone STARTED on {gameObject.name}");
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("MilkFrother")) return;
        Debug.Log($" MILK FROTHER TRIGGER: {other.gameObject.name}");
        MilkFrotherXR frother = other.GetComponent<MilkFrotherXR>();
        if (frother == null)
            frother = other.GetComponentInParent<MilkFrotherXR>();
        if (frother != null)
        {
            Debug.Log(" MilkFrother found! Setting snap zone");
            frother.SetSnapZone(this);
        }
    }
 
    private void OnTriggerExit(Collider other)
    {
        MilkFrotherXR frother = other.GetComponent<MilkFrotherXR>();
        if (frother == null)
            frother = other.GetComponentInParent<MilkFrotherXR>();
        if (frother != null)
        {
            frother.ClearSnapZone(this);
        }
    }
 
    public void TrySnapMilkFrother(MilkFrotherXR frother)
    {
        Debug.Log("TrySnapMilkFrother called!");
        if (frother == null) return;
        if (frother.grabInteractable.isSelected) return;
        if (frother.snapAnchor == null)
        {
            Debug.LogWarning("MilkFrotherSnapAnchor fehlt!");
            return;
        }
        if (milkFrotherController != null)
        {
            milkFrotherController.SetMilkFrotherSnapped(true, frother);
            Debug.Log("MilkFrotherController: SetMilkFrotherSnapped(true) called");
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
        Debug.Log(" MilkFrother snapped to station!");
    }
}