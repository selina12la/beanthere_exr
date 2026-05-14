using UnityEngine;
using System.Collections;

public class Tray : MonoBehaviour
{
    [Header("References")] 
    public GameCompletionManager gameManager;
    public LevelCompletionManager levelCompletion;  
    [Header("Mug Spawning")] 
    public EspressoMachineController espressoMachine;
    
    [Header("Settings")] 
    public float requiredStayTime = 2f;
    public string requiredTag = "Mug";
    
    private GameObject currentMug = null;
    private float stayTimer = 0f;
    private bool isProcessing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isProcessing) return;
        
        if (other.CompareTag(requiredTag))
        {
            currentMug = other.gameObject;
            stayTimer = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isProcessing) return;
    
        if (currentMug != null && other.gameObject == currentMug)
        {
            stayTimer += Time.deltaTime;
        
            if (stayTimer >= requiredStayTime)
            {
                isProcessing = true;
            
                TaskManagerLocator.Current?.OnMugOnTray();

                if (gameManager != null)
                    gameManager.CompleteLevel(currentMug);
            
                Destroy(currentMug);
                currentMug = null;
            
                if (espressoMachine != null)
                    espressoMachine.ResetMachine();
            
                if (levelCompletion != null)
                {
                    levelCompletion.CompleteLevel("Level 1");
                }
            
                isProcessing = false;
                stayTimer = 0f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isProcessing)
        {
            return;
        }
        
        if (currentMug != null && other.gameObject == currentMug)
        {
            currentMug = null;
            stayTimer = 0f;
        }
    }
    
    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneLoader.LoadLevel(1); 
    }
}