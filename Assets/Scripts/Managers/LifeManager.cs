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
    }

    public void AddLife() => lives++;

    public void LoseLife()
    {
        lives--;

        if (lives <= 0)
        {
            // Trigger Game Over Event 
        }
    }
}
