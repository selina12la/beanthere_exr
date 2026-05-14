using UnityEngine;
using TMPro;

public class LevelStartText : MonoBehaviour
{
    [Header("Settings")]
    public float displayDuration = 2f;
    public string levelText = "LEVEL 1";
    public GameObject textPrefab;
    
    private void Start()
    {
        ShowLevelText();
    }
    
    private void ShowLevelText()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        
        if (textPrefab != null)
        {
            GameObject textObj = Instantiate(textPrefab, canvas.transform);
            TextMeshProUGUI tmpText = textObj.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.text = levelText;
            }
            
            Destroy(textObj, displayDuration);
        }
    }
}