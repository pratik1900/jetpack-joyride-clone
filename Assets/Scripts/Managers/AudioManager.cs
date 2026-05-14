using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    [Header("--- Audio Sources ---")]
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource musicSource;

    [Header("--- Audio Clips ---")]
    [SerializeField] public AudioClip coinSFX;
    [SerializeField] public AudioClip playerHitSFX;
    // [SerializeField] AudioClip laserSFX;
    // [SerializeField] AudioClip deathSFX;
    // [SerializeField] AudioClip thrusterSFX;
    [SerializeField] public AudioClip backgroundMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }

    void OnEnable()
    {
        GameEvents.OnPlayerHit += () => PlaySFX(playerHitSFX);
        GameEvents.OnShieldActivated += PlayShieldEnabledSFX;
        GameEvents.OnShieldDeactivated += PlayShieldDisabledSFX;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerHit -= () => PlaySFX(playerHitSFX);
        GameEvents.OnShieldActivated -= PlayShieldEnabledSFX;
        GameEvents.OnShieldDeactivated -= PlayShieldDisabledSFX;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayShieldEnabledSFX()
    {
        // logic
    }

    public void PlayShieldDisabledSFX()
    {
        // logic
    }
}
