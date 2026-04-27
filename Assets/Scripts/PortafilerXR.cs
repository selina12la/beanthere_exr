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
    
    // Zwei verschiedene SnapZones
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
        // Priorität: Espresso Zone zuerst, dann Grinder Zone
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

    // Für Grinder SnapZone
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
    
    // Für Espresso SnapZone
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
    
    // Coffee Grounds Methoden
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
}