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
        
        // WICHTIG: Stelle sicher dass der Portafilter einen Collider hat
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

    // NEU: Trigger Erkennung für SnapZones
    private void OnTriggerEnter(Collider other)
    {
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
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"🔴 PORTAFILTER: OnTriggerExit with {other.gameObject.name}");
        
        GrinderSnapZone grinderZone = other.GetComponent<GrinderSnapZone>();
        if (grinderZone != null && currentGrinderZone == grinderZone)
        {
            ClearGrinderSnapZone();
        }
        
        FilterSnapZone espressoZone = other.GetComponent<FilterSnapZone>();
        if (espressoZone != null && currentEspressoZone == espressoZone)
        {
            ClearEspressoSnapZone();
            if (currentEspressoZone == null)
            {
                // Optional: Benachrichtige Espresso Machine dass Filter weg ist
                // Finde die Espresso Machine und rufe SetFilter(false) auf
            }
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
    
        // Benachrichtige dass der Portafilter jetzt in der Espresso Machine ist
        NotifyFilterDetector(false);
    }

    public void ClearEspressoSnapZone()
    {
        currentEspressoZone = null;
        Debug.Log("Espresso snap zone cleared");
    
        // Benachrichtige dass der Portafilter nicht mehr in der Espresso Machine ist
        NotifyFilterDetector(true);
    }

    private void NotifyFilterDetector(bool isAvailableForGrinder)
    {
        FilterDetector detector = FindFirstObjectByType<FilterDetector>();
        if (detector != null)
        {
            // Hier könntest du eine Methode aufrufen die den Detector deaktiviert
            // Oder einfach eine Variable setzen
        }
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
    
    public bool HasGrinderZone()
    {
        return currentGrinderZone != null;
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