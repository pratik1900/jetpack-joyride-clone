using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score { get; private set; }
    public int highScore { get; private set; }

    [SerializeField] private TMP_Text scoreTextValue;
    [SerializeField] private TMP_Text highScoreTextValue;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetScore();
        HighScoreInitialLoad();
    }

    void OnEnable()
    {
        GameEvents.OnGameOver += SaveHighScore;
    }

    void OnDisable()
    {
        GameEvents.OnGameOver -= SaveHighScore;
    }

    public void IncrementScore()
    {
        score++;

        if (score >= highScore)
        {
            highScore = score;
        }
        UpdateScoreUI();
    }

    public void HighScoreInitialLoad()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreTextValue.text = score.ToString();
        highScoreTextValue.text = highScore.ToString();
    }
}