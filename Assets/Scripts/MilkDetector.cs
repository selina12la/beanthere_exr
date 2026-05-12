using UnityEngine;

public class MilkDetector : MonoBehaviour
{
    private MugXR mugXR;

    [Header("Audio")] 
    public AudioSource milkPourSound;

    private void Start()
    {
        mugXR = GetComponentInParent<MugXR>();

        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MilkFrotherXR frother = other.GetComponent<MilkFrotherXR>();
        if (frother == null)
        {
            frother = other.GetComponentInParent<MilkFrotherXR>();
        }

        if (frother != null)
        {
            if (frother.HasMilk() && frother.IsSteamed())
            {
                if (milkPourSound != null)
                {
                    milkPourSound.Play();
                }

                if (mugXR != null && mugXR.HasCoffee())
                {
                    mugXR.AddMilk();
                    frother.RemoveMilk();

                    TaskManagerLocator.Current?.OnMilkPoured();
                }
            }
        }
    }
}