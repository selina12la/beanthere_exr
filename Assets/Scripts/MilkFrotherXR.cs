using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
 
public class MilkFrotherXR : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Rigidbody rb;
    public Transform snapAnchor;
    
    [Header("Milk Frother Visual")]
    public GameObject milkVisual;  // Milch Visual
    
    private bool hasMilk = true;   // ✅ MILCH IST SCHON VON ANFANG AN DA
    private bool isSteamed = false;
    private MilkFrotherSnapZone currentSnapZone;
 
    private void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        
        // Milch Visual am Anfang ANZEIGEN
        if (milkVisual != null)
            milkVisual.SetActive(true);
        
        gameObject.tag = "MilkFrother";
        
        Debug.Log($"🥛 Milk frother initialized - hasMilk={hasMilk}, milkVisual active");
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
            currentSnapZone.TrySnapMilkFrother(this);
        else
            Debug.Log("MilkFrother released - no snap zone!");
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MilkDetector"))
        {
            return;
        }
        
        if (!other.CompareTag("MilkFrotherZone")) return;
        
        MilkFrotherSnapZone frotherZone = other.GetComponent<MilkFrotherSnapZone>();
        if (frotherZone != null)
        {
            currentSnapZone = frotherZone;
        }
    }
 
    public void SetSnapZone(MilkFrotherSnapZone zone) => currentSnapZone = zone;
    public void ClearSnapZone(MilkFrotherSnapZone zone)
    {
        if (currentSnapZone == zone)
            currentSnapZone = null;
    }
    
    // ❌ ENTFERNT: AddMilk() wird nicht mehr benötigt!
    
    // Wird vom MilkFrotherController aufgerufen wenn gesteamt wurde
    public void SetSteamed()
    {
        if (hasMilk && !isSteamed)
        {
            isSteamed = true;
            Debug.Log("🥛 Milk steamed and ready to pour! (isSteamed=true)");
        }
    }
    
    public void RemoveMilk()
    {
        hasMilk = false;
        isSteamed = false;
        if (milkVisual != null)
            milkVisual.SetActive(false);
        Debug.Log("🥛 Milk removed from frother - poured into coffee!");
    }
    
    public bool HasMilk() => hasMilk;
    public bool IsSteamed() => isSteamed;
}