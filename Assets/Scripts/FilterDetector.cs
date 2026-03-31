using UnityEngine;

public class FilterDetector : MonoBehaviour
{
    public AudioSource filterSound;
    private bool hasFilter = false;

    public void OnFilterInserted()
    {
        hasFilter = true;
        filterSound.Play();
    }

    public void OnFilterRemoved()
    {
        hasFilter = false;
        filterSound.Stop();
    }
}
