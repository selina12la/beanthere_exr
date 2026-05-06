using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
 
public class MilkFrotherXR : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Rigidbody rb;
    public Transform snapAnchor;
    [Header("Milk Frother Visual")]
    public GameObject milkVisual;
    private bool hasMilk = false;
    private bool isSteamed = false;
 
    private MilkFrotherSnapZone currentSnapZone;
 
    private void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        if (milkVisual != null)
            milkVisual.SetActive(false);
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
            currentSnapZone.TrySnapMilkFrother(this);
        }
        else
        {
            Debug.Log("MilkFrother released - no snap zone!");
        }
    }
 
    private void OnTriggerEnter(Collider other)
    {
        // Milchdetector (für Pouring)
        if (other.CompareTag("MilkDetector"))
        {
            MilkDetector detector = other.GetComponent<MilkDetector>();
            if (detector != null && hasMilk && isSteamed)
            {
                // Das Pouring wird vom MilkDetector behandelt
                // Hier passiert nichts extra
            }
            return;
        }
        // SnapZone Erkennung
        if (!other.CompareTag("MilkFrotherZone")) return;
        MilkFrotherSnapZone frotherZone = other.GetComponent<MilkFrotherSnapZone>();
        if (frotherZone != null)
        {
            SetSnapZone(frotherZone);
        }
    }
 
    public void SetSnapZone(MilkFrotherSnapZone zone)
    {
        currentSnapZone = zone;
    }
 
    public void ClearSnapZone(MilkFrotherSnapZone zone)
    {
        if (currentSnapZone == zone)
            currentSnapZone = null;
    }
    public void AddMilk()
    {
        hasMilk = true;
        isSteamed = true;
        if (milkVisual != null)
            milkVisual.SetActive(true);
        Debug.Log("🥛 Milk frothed and ready to pour!");
    }
    public void RemoveMilk()
    {
        hasMilk = false;
        isSteamed = false;
        if (milkVisual != null)
            milkVisual.SetActive(false);
        Debug.Log("Milk removed from frother");
    }
    public bool HasMilk()
    {
        return hasMilk;
    }
    public bool IsSteamed()
    {
        return isSteamed;
    }
}