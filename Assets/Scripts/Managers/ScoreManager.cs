using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;
    public int highScore;

    [SerializeField] private TMP_Text scoreTextValue;


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void IncrementScore()
    {
        score++;
        scoreTextValue.text = score.ToString();
    }
}