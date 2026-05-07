using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PortafilterXR : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Rigidbody rb;

    public Transform snapAnchor;

    [Header("Coffee Grounds")] public GameObject coffeeGroundsVisual;
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
        {
            coffeeGroundsVisual.SetActive(false);
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
        if (other.CompareTag("MugSnapZone"))
        {
            return;
        }

        if (other.CompareTag("GrinderZone"))
        {
            GrinderSnapZone grinderZone = other.GetComponent<GrinderSnapZone>();
            if (grinderZone != null)
            {
                SetGrinderSnapZone(grinderZone);
            }
        }
        else if (other.CompareTag("EspressoZone"))
        {
            FilterSnapZone espressoZone = other.GetComponent<FilterSnapZone>();
            if (espressoZone != null)
            {
                SetEspressoSnapZone(espressoZone);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MugSnapZone"))
        {
            return;
        }

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
        if (currentEspressoZone != null)
        {
            currentEspressoZone.TrySnapPortafilter(this);
        }
        else if (currentGrinderZone != null)
        {
            currentGrinderZone.TrySnapPortafilter(this);
        }
    }

    public void SetGrinderSnapZone(GrinderSnapZone zone)
    {
        currentGrinderZone = zone;
    }

    public void ClearGrinderSnapZone()
    {
        currentGrinderZone = null;
    }

    public void SetEspressoSnapZone(FilterSnapZone zone)
    {
        currentEspressoZone = zone;
    }

    public void ClearEspressoSnapZone()
    {
        currentEspressoZone = null;
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
        {
            coffeeGroundsVisual.SetActive(false);
        }
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