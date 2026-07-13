using UnityEngine;

public class PowerupPickup : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private Powerup[] availablePowerupPrefabs;

    private Powerup selectedPowerupPrefab;

    [SerializeField]
    private float leftBoundary = -15.0f;

    // public void Initialize(Powerup powerup)
    // {
    //     selectedPowerupPrefab = powerup;
    // }

    void Update()
    {
        if (gameObject.transform.position.x < leftBoundary)
        {
            ReturnToPool();
        }
    }

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

        PlayerPowerupController powerupController =
            collision.GetComponentInChildren<PlayerPowerupController>();

        if (powerupController == null)
        {
            Debug.LogWarning("Player has no PlayerPowerupController.");
            ReturnToPool();
            return;
        }
        Debug.Log("Check 3 - Parent Obj: " + powerupController.ActivePowerupRoot);

        Powerup activePowerup = Instantiate(
            selectedPowerupPrefab,
            // collision.transform.position,
            powerupController.ActivePowerupRoot.position,
            Quaternion.identity,
            // collision.transform
            powerupController.ActivePowerupRoot
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

        ObjectPooler.Instance.ReturnToPool(ObjectTags.PowerupPickup.ToString(), gameObject);
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
