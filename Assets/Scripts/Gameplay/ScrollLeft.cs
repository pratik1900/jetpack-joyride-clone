using UnityEngine;

public class ScrollLeft : MonoBehaviour
{
    void Update()
    {
        // IF NOT GAME OVER
        transform.Translate(
            Vector3.left * Time.deltaTime * DifficultyManager.Instance.CurrentGameSpeed,
            Space.World
        );
    }
}
