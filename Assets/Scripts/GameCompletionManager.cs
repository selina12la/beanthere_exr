using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameCompletionManager : MonoBehaviour
{
    [Header("References")] public EspressoMachineController espressoMachine;
    public GameObject mugPrefab;
    public Transform mugSpawnPoint;
    public GameObject completionPanel;
    public Button nextLevelButton;

    [Header("Effects")] public ParticleSystem completionParticles;
    public AudioSource completionSound;

    [Header("Settings")] public float delayBeforeReset = 1f;

    private bool isLevelComplete = false;

    private void Start()
    {
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);
        }
    }

    public void CompleteLevel(GameObject deliveredMug)
    {
        if (isLevelComplete)
        {
            return;
        }

        isLevelComplete = true;

        if (completionParticles != null)
        {
            completionParticles.Play();
        }

        if (completionSound != null)
        {
            completionSound.Play();
        }

        if (espressoMachine != null)
        {
            espressoMachine.ResetMachine();
        }

        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }
    }

    private void OnNextLevelClicked()
    {
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }

        if (completionParticles != null)
        {
            completionParticles.Stop();
        }

        isLevelComplete = false;
    }
}