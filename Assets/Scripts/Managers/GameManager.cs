using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        // GameEvents.OnGameOver +=
    }

    void OnDisable() { }
}
