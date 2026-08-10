using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("--- Audio Sources ---")]
    [SerializeField]
    AudioSource SFXSource;

    [SerializeField]
    AudioSource musicSource;

    [Header("--- Audio Clips ---")]
    [SerializeField]
    public AudioClip coinSFX;

    [SerializeField]
    public AudioClip playerHitSFX;

    [SerializeField]
    public AudioClip playerLifeGainSFX;

    // [SerializeField] AudioClip pSFX;
    // [SerializeField] AudioClip deathSFX;
    // [SerializeField] AudioClip thrusterSFX;
    [SerializeField]
    public AudioClip backgroundMusic;

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
        GameEvents.OnPlayerHit += OnPlayerHit;
        GameEvents.OnPlayerLifeGain += OnPlayerLifeGain;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerHit -= OnPlayerHit;
        GameEvents.OnPlayerLifeGain -= OnPlayerLifeGain;
    }

    private void OnPlayerHit()
    {
        PlaySFX(playerHitSFX);
    }

    private void OnPlayerLifeGain()
    {
        PlaySFX(playerLifeGainSFX);
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
}
