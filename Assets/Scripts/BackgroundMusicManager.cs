using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)]
    public float musicVolume = 0.3f;
    
    [Header("Ambiance Settings")]
    public AudioClip ambianceSound; // Coffee shop sounds, rain, etc.
    [Range(0f, 1f)]
    public float ambianceVolume = 0.5f;
    
    private AudioSource musicSource;
    private AudioSource ambianceSource;
    private static BackgroundMusicManager instance;
    
    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Create audio sources
            musicSource = gameObject.AddComponent<AudioSource>();
            ambianceSource = gameObject.AddComponent<AudioSource>();
            
            // Setup music
            musicSource.clip = backgroundMusic;
            musicSource.volume = musicVolume;
            musicSource.loop = true;
            
            // Setup ambiance
            ambianceSource.clip = ambianceSound;
            ambianceSource.volume = ambianceVolume;
            ambianceSource.loop = true;
            
            // Play both
            if (backgroundMusic != null)
            {
                musicSource.Play();
                Debug.Log("Background music started");
            }
            
            if (ambianceSound != null)
            {
                ambianceSource.Play();
                Debug.Log("Ambiance sound started");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;
    }
    
    public void SetAmbianceVolume(float volume)
    {
        ambianceVolume = Mathf.Clamp01(volume);
        if (ambianceSource != null)
            ambianceSource.volume = ambianceVolume;
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }
    
    public void StartMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.Play();
    }
    
    public void StopAmbiance()
    {
        if (ambianceSource != null)
            ambianceSource.Stop();
    }
    
    public void StartAmbiance()
    {
        if (ambianceSource != null && !ambianceSource.isPlaying)
            ambianceSource.Play();
    }
}
