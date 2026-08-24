using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    [Header("Timing")]
    [Tooltip("Seconds of survival to reach maximum difficulty.")]
    [SerializeField]
    private float timeToMaxDifficulty = 300f;

    [Header("Speed Ramp")]
    [SerializeField]
    private float baseGameSpeed = 5f;

    [SerializeField]
    private float maxGameSpeed = 15f;

    [SerializeField]
    private AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Scenario Difficulty")]
    [SerializeField]
    private int maxScenarioDifficultyLevel = 10;

    private float elapsedTime;

    public float NormalizedDifficulty { get; private set; }
    public int CurrentScenarioDifficultyLevel { get; private set; }
    public float CurrentGameSpeed { get; private set; }

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

    private void Start()
    {
        SetGameSpeed(baseGameSpeed);
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver)
            return;

        elapsedTime += Time.deltaTime;
        NormalizedDifficulty = Mathf.Clamp01(elapsedTime / timeToMaxDifficulty);

        // For scenario selection, we want an integer difficulty level that scales with the normalized difficulty.
        CurrentScenarioDifficultyLevel = Mathf.FloorToInt(
            NormalizedDifficulty * maxScenarioDifficultyLevel
        );

        float speedT = speedCurve.Evaluate(NormalizedDifficulty);
        // GameManager.Instance.SetGameSpeed(Mathf.Lerp(baseGameSpeed, maxGameSpeed, speedT));
        SetGameSpeed(Mathf.Lerp(baseGameSpeed, maxGameSpeed, speedT));
    }

    public void SetGameSpeed(float newSpeed)
    {
        CurrentGameSpeed = newSpeed;
    }

    // needs to be used somewhere
    public void ResetDifficulty()
    {
        elapsedTime = 0f;
        NormalizedDifficulty = 0f;
        CurrentScenarioDifficultyLevel = 0;
        SetGameSpeed(baseGameSpeed);
    }
}
