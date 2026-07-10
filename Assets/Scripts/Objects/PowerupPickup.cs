using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    [SerializeField] private Powerup powerupPrefab;

    public void Initialize(Powerup powerup)
    {
        powerupPrefab = powerup;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (powerupPrefab == null)
        {
            Debug.LogWarning("PowerupPickup triggered without an assigned powerup.");
            ReturnToPool();
            return;
        }

        powerupPrefab.Activate();
        ReturnToPool();
    }

    public void OnObjectSpawn() { }

    public void ReturnToPool()
    {
        powerupPrefab = null;

        ObjectPooler.Instance.ReturnToPool(
            ObjectTags.PowerupPickup.ToString(),
            gameObject
        );
    }
}