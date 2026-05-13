using UnityEngine;
using System.Collections;
using TMPro;

public class LevelCompletionManager : MonoBehaviour
{
    [Header("Completion Settings")]
    public float completionTextDuration = 3f;
    public int nextLevelIndex = 1;
    
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
        if (canvas == null)
        {
            Debug.LogError("❌ No Canvas found in scene!");
            return;
        }
        
        GameObject textObj = new GameObject("LevelCompleteText");
        textObj.transform.SetParent(canvas.transform);
        
        TextMeshProUGUI tmpText = textObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = text;
        tmpText.fontSize = 48;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.color = Color.green;
        tmpText.fontStyle = FontStyles.Bold;
        
        tmpText.outlineWidth = 0.2f;
        tmpText.outlineColor = Color.black;
        
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        Destroy(textObj, completionTextDuration);
        
        Debug.Log($"✅ Level completion text shown: '{text}'");
    }
    
    private IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevelIndex);
    }
}