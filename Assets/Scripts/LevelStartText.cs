using UnityEngine;
using System.Collections;
using TMPro;

public class LevelStartText : MonoBehaviour
{
    [Header("Settings")]
    public float displayDuration = 2f;
    public string levelText = "LEVEL 1";
    public int fontSize = 1;
    public Color textColor = Color.white;
    
    private void Start()
    {
        ShowLevelText();
    }
    
    private void ShowLevelText()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ No Canvas found in scene!");
            return;
        }
        
        GameObject textObj = new GameObject("LevelStartText");
        textObj.transform.SetParent(canvas.transform);
        
        TextMeshProUGUI tmpText = textObj.AddComponent<TextMeshProUGUI>();
        tmpText.text = levelText;
        tmpText.fontSize = fontSize;
        tmpText.alignment = TextAlignmentOptions.Center;
        tmpText.color = textColor;
        tmpText.fontStyle = FontStyles.Bold;
        
        // Optional: Shadow Effekt
        tmpText.outlineWidth = 0.2f;
        tmpText.outlineColor = Color.black;
        
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        Destroy(textObj, displayDuration);
        
        Debug.Log($"✅ Level start text shown: '{levelText}'");
    }
}