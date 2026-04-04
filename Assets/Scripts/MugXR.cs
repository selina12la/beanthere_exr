using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MugXR : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Rigidbody rb;

    public Transform snapAnchor;

    private MugSnapZone currentSnapZone;

    private void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        grabInteractable.selectExited.AddListener(OnReleased);
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        grabInteractable.selectExited.RemoveListener(OnReleased);
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (currentSnapZone != null)
        {
            currentSnapZone.TrySnapMug(this);
        }
    }

    public void SetSnapZone(MugSnapZone zone)
    {
        currentSnapZone = zone;
    }

    public void ClearSnapZone(MugSnapZone zone)
    {
        if (currentSnapZone == zone)
            currentSnapZone = null;
    }
}