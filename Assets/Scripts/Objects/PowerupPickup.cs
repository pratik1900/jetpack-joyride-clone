using UnityEngine;

public class PowerupPickup : MonoBehaviour, IPooledObject
{
    private PowerupEffect powerupEffect;

    public void Initialize(PowerupEffect effect)
    {
        powerupEffect = effect;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (powerupEffect == null)
        {
            Debug.LogWarning("PowerupPickup triggered without an assigned powerup effect.");
            ReturnToPool();
            return;
        }

        powerupEffect.Apply(collision.gameObject);
        ReturnToPool();
    }

    public void OnObjectSpawn() { }

    public void ReturnToPool()
    {
        powerupEffect = null;

        ObjectPooler.Instance.ReturnToPool(
            ObjectTags.PowerupPickup.ToString(),
            gameObject
        );
    }
}