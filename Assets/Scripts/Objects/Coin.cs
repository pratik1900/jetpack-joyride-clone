using UnityEngine;

public class Coin : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private float leftBoundary = -15.0f;

    // Magnet effect variables
    private bool isMagnetized = false;

    [SerializeField]
    private float magnetBaseSpeed = 20.0f;
    private float currentSpeed;
    private float acceleration = 15.0f; // Adjust the accelerations as needed

    void Update()
    {
        if (gameObject.transform.position.x < leftBoundary)
        {
            ReturnToPool();
        }

        if (isMagnetized)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                currentSpeed += acceleration * Time.deltaTime;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.transform.position,
                    currentSpeed * Time.deltaTime
                );
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    public void OnObjectSpawn()
    {
        // Reset the coin's state when it is spawned from the pool
        isMagnetized = false;
        currentSpeed = magnetBaseSpeed;
    }

    public void ReturnToPool()
    {
        isMagnetized = false;
        ObjectPooler.Instance.ReturnToPool(ObjectTags.Coin.ToString(), gameObject);
    }

    private void CollectCoin()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.coinSFX);
        ScoreManager.Instance.IncrementScore();
        ReturnToPool();
    }

    public void StartMovingTowardsPlayer()
    {
        if (isMagnetized)
            return;

        isMagnetized = true;
    }
}
