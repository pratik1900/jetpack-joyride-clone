using UnityEngine;

public class ScrollLeft : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 10.0f;

    void Update()
    {
        // IF NOT GAME OVER
        transform.Translate(Vector3.left * Time.deltaTime * scrollSpeed, Space.World);
    }
}
