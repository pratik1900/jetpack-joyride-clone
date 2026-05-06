using UnityEngine;

public class Coin : MonoBehaviour, IPooledObject
{
    void Start() { }

    void Update() { }

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
        ObjectPooler.Instance.ReturnToPool(
            ObjectTags.Coin.ToString(),
            gameObject
        );
    }
}
