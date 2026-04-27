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

    private GrinderSnapZone currentSnapZone;

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
        if (currentSnapZone != null)
        {
            currentSnapZone.TrySnapPortafilter(this);
        }
    }

    public void SetSnapZone(GrinderSnapZone zone)
    {
        currentSnapZone = zone;
    }

    public void ClearSnapZone(GrinderSnapZone zone)
    {
        if (currentSnapZone == zone)
            currentSnapZone = null;
    }
    
  
    public void AddGrounds()
    {
        hasGrounds = true;
        if (coffeeGroundsVisual != null)
        {
            coffeeGroundsVisual.SetActive(true);
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