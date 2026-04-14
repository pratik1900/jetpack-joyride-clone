using UnityEngine;

public class CoinSpawnHandler : SpawnHandler
{
    public override string ObjectTag => "Coin";
    public override void Spawn(Vector3 position)
    {
        ObjectPooler.Instance.SpawnFromPool(
            ObjectTag,
            position,
            Quaternion.identity
        );
    }
}
