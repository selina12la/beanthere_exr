using UnityEngine;

public class MatchaBowlController : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject matchaPowderVisual;
    public GameObject matchaLiquidVisual;
    public ParticleSystem matchaParticles;
    
    [Header("Audio")]
    public AudioSource addPowderSound;
    public AudioSource addWaterSound;
    
    private bool hasPowder = false;
    private bool hasWater = false;
    
    private void Start()
    {
        if (matchaPowderVisual != null) matchaPowderVisual.SetActive(false);
        if (matchaLiquidVisual != null) matchaLiquidVisual.SetActive(false);
    }
    
    public void AddMatchaPowder()
    {
        if (!hasPowder)
        {
            hasPowder = true;
            if (matchaPowderVisual != null) matchaPowderVisual.SetActive(true);
            if (addPowderSound != null) addPowderSound.Play();
            if (matchaParticles != null) matchaParticles.Play();
            Debug.Log("✅ Matcha powder added to bowl!");
        }
    }
    
    public void AddWater()
    {
        if (hasPowder && !hasWater)
        {
            hasWater = true;
            if (matchaPowderVisual != null) matchaPowderVisual.SetActive(false);
            if (matchaLiquidVisual != null) matchaLiquidVisual.SetActive(true);
            if (addWaterSound != null) addWaterSound.Play();
            if (matchaParticles != null) matchaParticles.Play();
            Debug.Log("✅ Water added to bowl! Matcha ready to pour.");
        }
    }
    
    public bool HasMatchaReady()
    {
        return hasPowder && hasWater;
    }
    
    public void ResetBowl()
    {
        hasPowder = false;
        hasWater = false;
        if (matchaPowderVisual != null) matchaPowderVisual.SetActive(false);
        if (matchaLiquidVisual != null) matchaLiquidVisual.SetActive(false);
        if (matchaParticles != null) matchaParticles.Stop();
    }
}