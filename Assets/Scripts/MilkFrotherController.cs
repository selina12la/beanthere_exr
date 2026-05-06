using DefaultNamespace;
using UnityEngine;
public class MilkFrotherController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource milkPourSound;
    public AudioSource frotherSound;
    [Header("Visual")]
    public ParticleSystem milkParticles;
    public ParticleSystem frothParticles;
    [Header("MilkFrother Reference")]
    public MilkFrotherXR currentMilkFrother;
    private ITaskListManager taskManager;  // ← Interface
    private bool hasMilk = false;
    private bool hasFrother = false;
    private void Start()
    {
        taskManager = FindFirstObjectByType<MonoBehaviour>() as ITaskListManager;
    }
    public void AddMilk()
    {
        if (!hasMilk)
        {
            hasMilk = true;
            if (milkPourSound != null) milkPourSound.Play();
            if (milkParticles != null) milkParticles.Play();
            CheckFrotherReady();
        }
    }
    public void SetMilkFrotherSnapped(bool state, MilkFrotherXR frother)
    {
        if (!hasFrother && state)
        {
            hasFrother = true;
            currentMilkFrother = frother;
            CheckFrotherReady();
        }
        else if (!state)
        {
            hasFrother = false;
            currentMilkFrother = null;
            StopFrother();
        }
    }
    private void CheckFrotherReady()
    {
        if (hasMilk && hasFrother)
        {
            StartFrothing();
        }
    }
    private void StartFrothing()
    {
        if (frotherSound != null) frotherSound.Play();
        if (frothParticles != null) frothParticles.Play();
        Invoke(nameof(FinishFrothing), 2f);
    }
    private void FinishFrothing()
    {
        if (currentMilkFrother != null)
        {
            currentMilkFrother.AddMilk();
            Debug.Log("Milk frothed!");
            if (taskManager != null)
                taskManager.OnMilkSteamed();
        }
        StopFrother();
    }
    private void StopFrother()
    {
        if (frotherSound != null) frotherSound.Stop();
        if (frothParticles != null) frothParticles.Stop();
    }
}