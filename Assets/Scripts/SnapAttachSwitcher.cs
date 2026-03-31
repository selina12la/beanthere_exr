using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapAttachSwitcher : MonoBehaviour
{
    public Transform grabAttach;
    public Transform snapAttach;

    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    public void OnSnap()
    {
        grab.attachTransform = snapAttach;
    }

    public void OnUnsnap()
    {
        grab.attachTransform = grabAttach;
    }
}