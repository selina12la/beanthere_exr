using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MugXR : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Rigidbody rb;

    public Transform snapAnchor;
    
    [Header("Coffee Liquid")]
    public GameObject coffeeLiquidVisual;  // Der braune Zylinder
    public bool hasCoffee = false;

    private MugSnapZone currentSnapZone;

    private void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
            
        // Coffee Liquid am Anfang ausblenden
        if (coffeeLiquidVisual != null)
            coffeeLiquidVisual.SetActive(false);
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
        else
        {
            Debug.Log("Mug released - no snap zone!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"🔍 MUG TRIGGER DETECTED: {other.gameObject.name} | Tag: {other.tag}");
    
        // NUR auf MugSnapZone reagieren!
        if (!other.CompareTag("MugSnapZone"))
        {
            Debug.Log($"Mug ignored zone: {other.gameObject.name} (Tag: {other.tag})");
            return;
        }
        
        Debug.Log($"Mug entered MugSnapZone: {other.gameObject.name}");
        
        MugSnapZone mugZone = other.GetComponent<MugSnapZone>();
        if (mugZone != null)
        {
            SetSnapZone(mugZone);
        }
    }

    public void SetSnapZone(MugSnapZone zone)
    {
        currentSnapZone = zone;
        Debug.Log("Mug snap zone set");
    }

    public void ClearSnapZone(MugSnapZone zone)
    {
        if (currentSnapZone == zone)
            currentSnapZone = null;
        Debug.Log("Mug snap zone cleared");
    }
    
    public void ResetMug()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        currentSnapZone = null;
        transform.SetParent(null);
    }
    
    // NEUE METHODEN für Coffee Liquid
    public void AddCoffee()
    {
        hasCoffee = true;
        if (coffeeLiquidVisual != null)
        {
            coffeeLiquidVisual.SetActive(true);
            Debug.Log("☕ Coffee liquid VISIBLE in mug!");
        }
    }
    
    public void RemoveCoffee()
    {
        hasCoffee = false;
        if (coffeeLiquidVisual != null)
            coffeeLiquidVisual.SetActive(false);
        Debug.Log("Coffee liquid removed from mug");
    }
    
    public bool HasCoffee()
    {
        return hasCoffee;
    }
}