// using UnityEngine;

// public class PowerupPickupSpawnHandler : SpawnHandler
// {
//     [SerializeField] private PowerupEffect[] availablePowerupEffects;

//     public override ObjectTags ObjectTag => ObjectTags.PowerupPickup;

//     public override void Spawn(Vector3 position)
//     {

//         GameObject spawnedPickupObject = ObjectPooler.Instance.SpawnFromPool(
//             ObjectTag.ToString(),
//             position,
//             Quaternion.identity
//         );

//         PowerupPickup pickup = spawnedPickupObject?.GetComponent<PowerupPickup>();

//         PowerupEffect chosenPowerup = GetRandomPowerupEffect();

//         pickup.Initialize(chosenPowerup);
//     }

//     private PowerupEffect GetRandomPowerupEffect()
//     {
//         return availablePowerupEffects[Random.Range(0, availablePowerupEffects.Length)];
//     }
// }
