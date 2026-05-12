using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;

    private int lives = 3;
    private bool isShielded = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // set up initial life display
        GameEvents.TriggerLivesChanged(lives);
    }

    void OnEnable()
    {
        GameEvents.OnPlayerHit += ProcessPlayerHit;
        GameEvents.OnShieldToggled += ToggleShield;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerHit -= ProcessPlayerHit;
        GameEvents.OnShieldToggled -= ToggleShield;
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
        if (isShielded)
        {
            GameEvents.TriggerShieldToggled(false);
        }
        else
        {
            LoseLife();
        }
    }

    public void ToggleShield(bool isShieldEnabled)
    {
        isShielded = isShieldEnabled;
        Debug.Log("Shield: " + isShielded);
    }
}
