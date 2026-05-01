using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MugXR : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Rigidbody rb;

    public Transform snapAnchor;
    
    [Header("Coffee Liquid")]
    public GameObject coffeeLiquidVisual;
    public bool hasCoffee = false;

    private MugSnapZone currentSnapZone;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
            
        if (coffeeLiquidVisual != null)
            coffeeLiquidVisual.SetActive(false);
        
        // Speichere Startposition für Reset
        startPosition = transform.position;
        startRotation = transform.rotation;
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

    private void OnReleased(SelectExitEventArgs args)
    {
        Debug.Log($"🎯 Mug OnReleased called! currentSnapZone={currentSnapZone != null}");
    
        if (currentSnapZone != null)
        {
            // NICHT hier den Rigidbody ändern!
            currentSnapZone.TrySnapMug(this);
        }
        else
        {
            Debug.Log("Mug released - no snap zone!");
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        transform.SetParent(null);
        // NICHT hier den Rigidbody ändern!
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"🔍 MUG TRIGGER DETECTED: {other.gameObject.name} | Tag: {other.tag}");
    
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
        transform.position = startPosition;
        transform.rotation = startRotation;
        RemoveCoffee(); // Kaffee entfernen beim Reset
    }
    
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