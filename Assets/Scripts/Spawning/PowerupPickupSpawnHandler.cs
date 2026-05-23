using UnityEngine;

public class PowerupPickupSpawnHandler : SpawnHandler
{
    [SerializeField] private PowerupSO[] availablePowerupEffects;

    public override ObjectTags ObjectTag => ObjectTags.PowerupPickup;

    public override void Spawn(Vector3 position)
    {

        GameObject spawnedPickupObject = ObjectPooler.Instance.SpawnFromPool(
            ObjectTag.ToString(),
            position,
            Quaternion.identity
        );

        PowerupPickup pickup = spawnedPickupObject?.GetComponent<PowerupPickup>();

        PowerupSO chosenPowerup = GetRandomPowerupEffect();

        pickup.Initialize(chosenPowerup);
    }

    private PowerupSO GetRandomPowerupEffect()
    {
        return availablePowerupEffects[Random.Range(0, availablePowerupEffects.Length)];
    }
}
