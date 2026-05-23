using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;

    private int lives = 3;

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
        GameEvents.TriggerLivesChanged(lives);
    }

    void OnEnable()
    {
        GameEvents.OnPlayerHit += ProcessPlayerHit;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerHit -= ProcessPlayerHit;
    }

    public void AddLife()
    {
        lives++;
        GameEvents.TriggerLivesChanged(lives);
    }

    public void LoseLife()
    {
        lives--;
        GameEvents.TriggerLivesChanged(lives);

        if (lives <= 0)
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
