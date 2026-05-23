using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;

    private int lives_current = 3;
    private int lives_max = 3;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // set up initial life display
        GameEvents.TriggerLivesChanged(lives_current);
    }

    void OnEnable()
    {
        GameEvents.OnPlayerHit += ProcessPlayerHit;
        GameEvents.OnPlayerLifeGain += AddLife;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerHit -= ProcessPlayerHit;
        GameEvents.OnPlayerLifeGain -= AddLife;
    }

    public void AddLife()
    {
        if (!(lives_current < lives_max))
        {
            return;
        }
        lives_current++;
        GameEvents.TriggerLivesChanged(lives_current);
    }

    public void LoseLife()
    {
        lives_current--;
        GameEvents.TriggerLivesChanged(lives_current);

        if (lives_current <= 0)
        {
            // Trigger Game Over Event 
            GameEvents.TriggerGameOver();
        }
    }

    public void ProcessPlayerHit()
    {
        LoseLife();
    }
}
