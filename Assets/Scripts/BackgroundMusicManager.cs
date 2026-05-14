using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)]
    public float musicVolume = 0.01f;
    
    [Header("Ambiance Settings")]
    public AudioClip ambianceSound;
    [Range(0f, 2f)]
    public float ambianceVolume = 1.5f;
    
    private AudioSource musicSource;
    private AudioSource ambianceSource;
    private static BackgroundMusicManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            musicSource = gameObject.AddComponent<AudioSource>();
            ambianceSource = gameObject.AddComponent<AudioSource>();
            
            musicSource.clip = backgroundMusic;
            musicSource.volume = musicVolume;
            musicSource.loop = true;
            
            ambianceSource.clip = ambianceSound;
            ambianceSource.volume = ambianceVolume;
            ambianceSource.loop = true;
            
            if (backgroundMusic != null)
            {
                musicSource.Play();
            }
            
            if (ambianceSound != null)
            {
                ambianceSource.Play();
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
        {
            musicSource.volume = musicVolume;
        }
    }
    
    public void SetAmbianceVolume(float volume)
    {
        ambianceVolume = Mathf.Clamp(volume, 0f, 2f);
        if (ambianceSource != null)
        {
            ambianceSource.volume = ambianceVolume;
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    public void StartMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
    
    public void StopAmbiance()
    {
        if (ambianceSource != null)
        {
            ambianceSource.Stop();
        }
    }
    
    public void StartAmbiance()
    {
        if (ambianceSource != null && !ambianceSource.isPlaying)
        {
            ambianceSource.Play();
        }
    }
}
