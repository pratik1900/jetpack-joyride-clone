using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score { get; private set; }
    public int highScore { get; private set; }

    private int scoreMultiplier = 1;

    [SerializeField]
    private TMP_Text scoreTextValue;

    [SerializeField]
    private TMP_Text highScoreTextValue;

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
        GameEvents.OnUpdateScoreMultiplier += UpdateScoreMultiplier;
    }

    void OnDisable()
    {
        GameEvents.OnGameOver -= SaveHighScore;
        GameEvents.OnUpdateScoreMultiplier -= UpdateScoreMultiplier;
    }

    public void IncrementScore()
    {
        score = score + (1 * scoreMultiplier);

        if (score >= highScore)
        {
            highScore = score;
        }
        UpdateScoreUI();
    }

    private void UpdateScoreMultiplier(int multiplier)
    {
        scoreMultiplier = multiplier;
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
