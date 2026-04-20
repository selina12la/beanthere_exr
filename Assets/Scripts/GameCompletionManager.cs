using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class GameCompletionManager : MonoBehaviour
{
    [Header("References")]
    public EspressoMachineController espressoMachine;
    public GameObject mugPrefab;           // Original Mug Prefab
    public Transform mugSpawnPoint;        // Wo der Mug respawnen soll
    public GameObject completionPanel;     // UI Panel für "Level Complete"
    public Button nextLevelButton;         // Button für nächsten Level
    [Header("Effects")]
    public ParticleSystem completionParticles;
    public AudioSource completionSound;
    [Header("Settings")]
    public float delayBeforeReset = 1f;
    private bool isLevelComplete = false;
    private GameObject currentMugInstance;
    private void Start()
    {
        if (completionPanel != null)
            completionPanel.SetActive(false);
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);
        // Finde den aktuellen Mug in der Szene
        currentMugInstance = GameObject.FindGameObjectWithTag("Mug");
    }
    public void CompleteLevel(GameObject deliveredMug)
    {
        if (isLevelComplete) return;
        isLevelComplete = true;
        Debug.Log("🏆 LEVEL COMPLETE! 🏆");
   
        if (completionParticles != null)
            completionParticles.Play();
        if (completionSound != null)
            completionSound.Play();
     
        if (espressoMachine != null)
        {
            espressoMachine.ResetMachine();
        }
        
        if (deliveredMug != null)
        {
            StartCoroutine(FadeAndDestroy(deliveredMug));
        }
    
        if (completionPanel != null)
            completionPanel.SetActive(true);
    }
    private IEnumerator FadeAndDestroy(GameObject mug)
    {
        
        Renderer renderer = mug.GetComponent<Renderer>();
        if (renderer != null)
        {
            float elapsed = 0f;
            Color originalColor = renderer.material.color;
            while (elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;
                float alpha = 1 - (elapsed / 0.5f);
                renderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }
        Destroy(mug);
        currentMugInstance = null;
    }
    private void OnNextLevelClicked()
    {
        Debug.Log("Loading next level / Resetting...");
        
        if (completionPanel != null)
            completionPanel.SetActive(false);
        SpawnNewMug();
        ResetLevel();
        isLevelComplete = false;
    }
    private void SpawnNewMug()
    {
        if (mugPrefab != null && mugSpawnPoint != null)
        {
            currentMugInstance = Instantiate(mugPrefab, mugSpawnPoint.position, mugSpawnPoint.rotation);
            currentMugInstance.tag = "Mug";
            if (currentMugInstance.GetComponent<MugXR>() == null)
            {
                Debug.LogError("Spawned mug has no MugXR component!");
            }
        }
        else
        {
            Debug.LogWarning("MugPrefab or MugSpawnPoint not set!");
        }
    }
    private void ResetLevel()
    {
        if (espressoMachine != null)
        {
            espressoMachine.ResetMachine();
        }
        if (completionParticles != null)
            completionParticles.Stop();
    }
}