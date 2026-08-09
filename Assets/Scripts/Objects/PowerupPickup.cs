using Unity.VisualScripting;
using UnityEngine;

public class PowerupPickup : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private Powerup[] availablePowerupPrefabs;

    private Powerup selectedPowerupPrefab;

    [SerializeField]
    private float leftBoundary = -15.0f;

    [SerializeField]
    private PlayerPowerupController powerupController;

    // public void Initialize(Powerup powerup)
    // {
    //     selectedPowerupPrefab = powerup;
    // }

    void Awake()
    {
        powerupController = GameObject
            .FindWithTag("PowerupController")
            ?.GetComponent<PlayerPowerupController>();
    }

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
            if (selectedPowerupPrefab == null)
            {
                Debug.LogWarning("Powerup pickup has no configured powerup prefab.");
                ReturnToPool();
                return;
            }
        }

        // PlayerPowerupController powerupController =
        //     collision.GetComponentInChildren<PlayerPowerupController>();

        if (powerupController == null)
        {
            Debug.LogWarning("Player has no PlayerPowerupController.");
            ReturnToPool();
            return;
        }

        //Check if Powerup of the same type is already active, if so refresh the timer and return to pool
        Transform overlappingPowerup = GetOverlappingPowerup();
        if (overlappingPowerup != null)
        {
            overlappingPowerup.GetComponent<Powerup>().RefreshExpiryTimerIfPresent();
            ReturnToPool();
            return;
        }

        Powerup activePowerup = Instantiate(
            selectedPowerupPrefab,
            powerupController.ActivePowerupRoot.position,
            Quaternion.identity,
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
        Powerup selectedPowerup = availablePowerupPrefabs[
            Random.Range(0, availablePowerupPrefabs.Length)
        ];
        Debug.Log($"Picked random powerup: {selectedPowerup.name}");
        SetIconForPickup(selectedPowerup);

        return selectedPowerup;
    }

    private void SetIconForPickup(Powerup powerup)
    {
        if (powerup == null)
        {
            return;
        }

        Sprite icon = powerup.GetComponent<Powerup>().icon;
        SpriteRenderer iconRenderer = transform.Find("Icon").GetComponent<SpriteRenderer>();

        if (iconRenderer != null && icon != null)
        {
            iconRenderer.sprite = icon;
        }
    }

    private Transform GetOverlappingPowerup()
    {
        foreach (Transform child in powerupController.ActivePowerupRoot)
        {
            string childName = child.name.Replace("(Clone)", "").Trim();
            if (selectedPowerupPrefab.name == childName)
            {
                return child;
            }
        }

        return null;
    }
}
