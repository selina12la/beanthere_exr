using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MugXR : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Rigidbody rb;
    public Transform snapAnchor;

    [Header("Liquid Visuals")] 
    public GameObject coffeeVisual;
    public GameObject latteVisual;

    [Header("States")] 
    public bool hasCoffee = false;
    public bool hasMilk = false;

    private MugSnapZone currentSnapZone;

    private void Awake()
    {
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (coffeeVisual != null)
        {
            coffeeVisual.SetActive(false);
        }

        if (latteVisual != null)
        {
            latteVisual.SetActive(false);
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MugSnapZone"))
        {
            MugSnapZone mugZone = other.GetComponent<MugSnapZone>();
            if (mugZone != null)
            {
                currentSnapZone = mugZone;
            }
        }
    }

    public void SetSnapZone(MugSnapZone zone) => currentSnapZone = zone;

    public void ClearSnapZone(MugSnapZone zone)
    {
        if (currentSnapZone == zone)
        {
            currentSnapZone = null;
        }
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
        ResetLiquid();
    }

    public void AddCoffee()
    {
        hasCoffee = true;
        hasMilk = false;

        if (coffeeVisual != null)
        {
            coffeeVisual.SetActive(true);
        }

        if (latteVisual != null)
        {
            latteVisual.SetActive(false);
        }
    }

    public void AddMilk()
    {
        if (!hasCoffee)
        {
            return;
        }

        hasMilk = true;

        if (coffeeVisual != null)
        {
            coffeeVisual.SetActive(false);
        }

        if (latteVisual != null)
        {
            latteVisual.SetActive(true);
        }
    }

    public void ResetLiquid()
    {
        hasCoffee = false;
        hasMilk = false;

        if (coffeeVisual != null)
        {
            coffeeVisual.SetActive(false);
        }

        if (latteVisual != null)
        {
            latteVisual.SetActive(false);
        }
    }

    public bool HasCoffee() => hasCoffee;
    public bool HasMilk() => hasMilk;
    public bool IsLatte() => hasCoffee && hasMilk;
}