using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public float MetersTraveled { get; private set; }

    void Update()
    {
        MetersTraveled += GameManager.Instance.CurrentGameSpeed * Time.deltaTime;
    }
}
