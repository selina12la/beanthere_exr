using UnityEngine;
using System.Collections;

public class LevelCompletionManager : MonoBehaviour
{
    [Header("Completion Settings")]
    public float completionTextDuration = 3f;
    public int nextLevelIndex = 1;
    public GameObject completionTextPrefab;
    
    private bool isCompleting = false;
    
    public void CompleteLevel(string levelName)
    {
        if (isCompleting) return;
        isCompleting = true;
        
        ShowCompletionText($"LEVEL COMPLETED!");
        
        StartCoroutine(LoadNextLevelAfterDelay(completionTextDuration));
    }
    
    private void ShowCompletionText(string text)
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas != null && completionTextPrefab != null)
        {
            GameObject completionText = Instantiate(completionTextPrefab, canvas.transform);
            var textComponent = completionText.GetComponent<UnityEngine.UI.Text>();
            if (textComponent != null)
                textComponent.text = text;
            
            Destroy(completionText, completionTextDuration);
        }
    }
    
    private IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneLoader.LoadLevel(nextLevelIndex);
    }
}