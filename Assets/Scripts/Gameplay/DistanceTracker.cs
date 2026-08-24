using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public float MetersTraveled { get; private set; }

    void Update()
    {
        if (GameManager.Instance.isGameOver)
            return;

        MetersTraveled += DifficultyManager.Instance.CurrentGameSpeed * Time.deltaTime;
    }
}
