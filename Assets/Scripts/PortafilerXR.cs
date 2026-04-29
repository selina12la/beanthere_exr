using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PortafilterXR : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Rigidbody rb;

    public Transform snapAnchor;
    
    [Header("Coffee Grounds")]
    public GameObject coffeeGroundsVisual; 
    public bool hasGrounds = false;
    
    private GrinderSnapZone currentGrinderZone;
    private FilterSnapZone currentEspressoZone;

    private void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
            
        if (coffeeGroundsVisual != null)
            coffeeGroundsVisual.SetActive(false);
        
        Debug.Log("CoffeeGrounds visual found and hidden");
        
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("❌ Portafilter has NO COLLIDER! Add a BoxCollider!");
        }
        else
        {
            Debug.Log($"✅ Portafilter has Collider: {col.GetType().Name}, IsTrigger: {col.isTrigger}");
        }
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"🔍 PORTAFILTER TRIGGER DETECTED: {other.gameObject.name} | Tag: {other.tag} | Has Component: {other.GetComponent<PortafilterXR>() != null}");
    
        // IGNORIERE MugSnapZone komplett!
        if (other.CompareTag("MugSnapZone"))
        {
            Debug.Log("Portafilter entered MugSnapZone - IGNORING!");
            return;
        }
    
        if (other.CompareTag("GrinderZone"))
        {
            GrinderSnapZone grinderZone = other.GetComponent<GrinderSnapZone>();
            if (grinderZone != null)
            {
                Debug.Log("Entered GRINDER zone");
                SetGrinderSnapZone(grinderZone);
            }
        }
        else if (other.CompareTag("EspressoZone"))
        {
            FilterSnapZone espressoZone = other.GetComponent<FilterSnapZone>();
            if (espressoZone != null)
            {
                Debug.Log("Entered ESPRESSO zone");
                SetEspressoSnapZone(espressoZone);
            }
        }
        else
        {
            Debug.Log($"Portafilter entered unknown zone: {other.gameObject.name} (Tag: {other.tag})");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // IGNORIERE MugSnapZone komplett!
        if (other.CompareTag("MugSnapZone")) return;
        
        Debug.Log($"🔴 PORTAFILTER: OnTriggerExit with {other.gameObject.name}");
        
        if (other.CompareTag("GrinderZone"))
        {
            ClearGrinderSnapZone();
        }
        else if (other.CompareTag("EspressoZone"))
        {
            ClearEspressoSnapZone();
        }
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
        Debug.Log($"🎯 Portafilter released. currentEspressoZone={currentEspressoZone != null}, currentGrinderZone={currentGrinderZone != null}");
        
        if (currentEspressoZone != null)
        {
            Debug.Log("Snapping to Espresso Machine Zone");
            currentEspressoZone.TrySnapPortafilter(this);
        }
        else if (currentGrinderZone != null)
        {
            Debug.Log("Snapping to Grinder Zone");
            currentGrinderZone.TrySnapPortafilter(this);
        }
        else
        {
            Debug.Log("No snap zone available!");
        }
    }

    public void SetGrinderSnapZone(GrinderSnapZone zone)
    {
        currentGrinderZone = zone;
        Debug.Log("Grinder snap zone set");
    }
    
    public void ClearGrinderSnapZone()
    {
        currentGrinderZone = null;
        Debug.Log("Grinder snap zone cleared");
    }
    
    public void SetEspressoSnapZone(FilterSnapZone zone)
    {
        currentEspressoZone = zone;
        Debug.Log("Espresso snap zone set");
    }

    public void ClearEspressoSnapZone()
    {
        currentEspressoZone = null;
        Debug.Log("Espresso snap zone cleared");
    }
    
    public void AddGrounds()
    {
        hasGrounds = true;
        if (coffeeGroundsVisual != null)
        {
            coffeeGroundsVisual.SetActive(true);
            Debug.Log("☕ Coffee grounds VISIBLE in portafilter!");
        }
    }
    
    public void RemoveGrounds()
    {
        hasGrounds = false;
        if (coffeeGroundsVisual != null)
            coffeeGroundsVisual.SetActive(false);
        Debug.Log("Coffee grounds removed from portafilter");
    }
    
    public bool HasGrounds()
    {
        return hasGrounds;
    }
    
    public bool HasGrinderZone()
    {
        return currentGrinderZone != null;
    }
}