using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeListener : MonoBehaviour
{
    private static SceneChangeListener instance;
    
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("SceneChangeListener");
            instance = go.AddComponent<SceneChangeListener>();
            DontDestroyOnLoad(go);
        }
    }
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset task manager cache when loading a new scene
        TaskManagerLocator.Reset();
        Debug.Log($"Scene loaded: {scene.name}, TaskManager cache reset");
    }
}