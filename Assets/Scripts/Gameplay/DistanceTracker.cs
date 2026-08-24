using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public float MetersTraveled { get; private set; }

    void Update()
    {
        MetersTraveled += DifficultyManager.Instance.CurrentGameSpeed * Time.deltaTime;
    }
}
