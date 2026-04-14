using UnityEngine;

public class CoinSpawnHandler : SpawnHandler
{
    public override ObjectTags ObjectTag => ObjectTags.Coin;
    public override void Spawn(Vector3 position)
    {
        ObjectPooler.Instance.SpawnFromPool(
            ObjectTag.ToString(),
            position,
            Quaternion.identity
        );
    }
}
