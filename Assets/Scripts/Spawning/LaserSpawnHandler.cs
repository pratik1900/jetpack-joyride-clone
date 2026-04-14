using UnityEngine;

public class LaserSpawnHandler : SpawnHandler
{
    public override string ObjectTag => "Laser";
    public override void Spawn(Vector3 position)
    {
        ObjectPooler.Instance.SpawnFromPool(
            ObjectTag,
            position,
            Quaternion.Euler(0, 0, Random.Range(0, 360))
        );
    }
}
