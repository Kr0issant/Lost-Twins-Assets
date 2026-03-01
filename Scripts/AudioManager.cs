using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [Tooltip("Requires an AudioSource set to Loop")]
    public AudioSource bgmSource;
    [Tooltip("Requires an AudioSource NOT set to Loop")]
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip normalBGM;
    public AudioClip trollBGM;
    public AudioClip bounceSFX;
    public AudioClip deathSFX;
    public AudioClip finishSFX;
    public AudioClip grappleSFX;
    // Add more clips here as you expand (e.g., public AudioClip grappleSFX;)

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // Call this to change the background music
    public void PlayBGM(AudioClip clip)
    {
        // Prevent restarting the track if it's already playing the requested clip
        if (bgmSource.clip == clip) return; 
        
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    // Call this to play a one-off sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}