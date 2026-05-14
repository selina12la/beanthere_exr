using UnityEngine;
using System.Collections;
using TMPro;

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
        
        ShowCompletionText("LEVEL COMPLETED!");
        StartCoroutine(LoadNextLevelAfterDelay(completionTextDuration));
    }
    
    private void ShowCompletionText(string text)
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        
        if (completionTextPrefab != null)
        {
            GameObject textObj = Instantiate(completionTextPrefab, canvas.transform);
            TextMeshProUGUI tmpText = textObj.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
                tmpText.text = text;
            
            Destroy(textObj, completionTextDuration);
        }
    }
    
    private IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelIndex);
    }
}