using UnityEngine;

public class Coin : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private float leftBoundary = -15.0f;

    void Start() { }

    void Update()
    {
        if (gameObject.transform.position.x < leftBoundary)
        {
            ReturnToPool();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.coinSFX);
            ScoreManager.Instance.IncrementScore();
            ReturnToPool();
        }
    }

    public void OnObjectSpawn() { }

    public void ReturnToPool()
    {
        ObjectPooler.Instance.ReturnToPool(ObjectTags.Coin.ToString(), gameObject);
    }
}
