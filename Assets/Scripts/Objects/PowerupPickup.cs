using UnityEngine;

public class PowerupPickup : MonoBehaviour, IPooledObject
{
    [SerializeField] private Powerup[] availablePowerupPrefabs;

    private Powerup selectedPowerupPrefab;

    // public void Initialize(Powerup powerup)
    // {
    //     selectedPowerupPrefab = powerup;
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (selectedPowerupPrefab == null)
        {
            selectedPowerupPrefab = PickRandomPowerupPrefab();
        }

        Powerup activePowerup = Instantiate(
            selectedPowerupPrefab,
            collision.transform.position,
            Quaternion.identity,
            collision.transform
        );

        activePowerup.Activate();
        ReturnToPool();
    }

    public void OnObjectSpawn()
    {
        selectedPowerupPrefab = PickRandomPowerupPrefab();
    }

    public void ReturnToPool()
    {
        selectedPowerupPrefab = null;

        ObjectPooler.Instance.ReturnToPool(
            ObjectTags.PowerupPickup.ToString(),
            gameObject
        );
    }

    private Powerup PickRandomPowerupPrefab()
    {
        if (availablePowerupPrefabs == null || availablePowerupPrefabs.Length == 0)
        {
            return null;
        }

        return availablePowerupPrefabs[Random.Range(0, availablePowerupPrefabs.Length)];
    }
}
