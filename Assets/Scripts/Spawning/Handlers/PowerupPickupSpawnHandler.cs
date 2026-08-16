using UnityEngine;

public class PowerupPickupSpawnHandler : SpawnHandler
{
    public override ObjectTags ObjectTag => ObjectTags.PowerupPickup;

    public override void Spawn(Vector3 position)
    {
        GameObject spawnedPickupObject = ObjectPooler.Instance.SpawnFromPool(
            ObjectTag.ToString(),
            position,
            Quaternion.identity
        );
    }
}
