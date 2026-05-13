using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;
    
    [Header("Level Text Settings")]
    public float levelTextDisplayTime = 2f;
    public GameObject levelTextPrefab; 
    
    private GameObject currentLevelText;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string levelText = GetLevelTextForScene(scene.buildIndex);
        if (!string.IsNullOrEmpty(levelText))
        {
            ShowLevelText(levelText);
        }
    }
    
    private string GetLevelTextForScene(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 0: return "LEVEL 1";
            case 1: return "LEVEL 2";
            default: return "";
        }
    }
    
    private void ShowLevelText(string text)
    {
        if (currentLevelText != null)
            Destroy(currentLevelText);
        
        if (levelTextPrefab != null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                currentLevelText = Instantiate(levelTextPrefab, canvas.transform);
                var textComponent = currentLevelText.GetComponent<UnityEngine.UI.Text>();
                if (textComponent != null)
                    textComponent.text = text;
            }
        }
        
        StartCoroutine(HideLevelTextAfterDelay(levelTextDisplayTime));
    }
    
    private IEnumerator HideLevelTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (currentLevelText != null)
            Destroy(currentLevelText);
    }
    
    public static void LoadLevel(int levelIndex)
    {
        if (instance != null)
            instance.StartCoroutine(instance.LoadSceneWithDelay(levelIndex, 2f));
    }
    
    public static void LoadLevelImmediate(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    
    private IEnumerator LoadSceneWithDelay(int levelIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(levelIndex);
    }
}