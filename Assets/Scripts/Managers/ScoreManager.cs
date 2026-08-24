using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score { get; private set; }
    public int highScore { get; private set; }

    // For powerups
    private int scoreMultiplier = 1;

    [SerializeField]
    private TMP_Text scoreTextValue;

    [SerializeField]
    private TMP_Text highScoreTextValue;

    [SerializeField]
    private DistanceTracker distanceTracker;

    [Header("Passive Score")]
    [SerializeField]
    private float passiveScorePerSecond = 1f;

    [SerializeField]
    private AnimationCurve passiveMultiplierCurve = AnimationCurve.Linear(0, 1f, 1, 3f);

    private float passiveScoreAccumulator = 0f;

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

    private void Update()
    {
        if (GameManager.Instance.isGameOver)
            return;
        AccumulatePassiveScore(Time.deltaTime);
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

    public void IncrementScore(int val)
    {
        score = score + (val * scoreMultiplier);

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

    private void AccumulatePassiveScore(float deltaTime)
    {
        float passiveMultiplier = passiveMultiplierCurve.Evaluate(
            DifficultyManager.Instance.NormalizedDifficulty
        );
        passiveScoreAccumulator += passiveScorePerSecond * deltaTime * passiveMultiplier;

        int wholePoints = Mathf.FloorToInt(passiveScoreAccumulator);
        if (wholePoints > 0)
        {
            passiveScoreAccumulator -= wholePoints;
            IncrementScore(wholePoints);
        }
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
