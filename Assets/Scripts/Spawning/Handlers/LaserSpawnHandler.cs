using UnityEngine;

public class LaserSpawnHandler : SpawnHandler
{
    public override ObjectTags ObjectTag => ObjectTags.Laser;

    public override void Spawn(Vector3 position)
    {
        ObjectPooler.Instance.SpawnFromPool(
            ObjectTag.ToString(),
            position,
            Quaternion.Euler(0, 0, Random.Range(0f, 360f))
        );
    }

    public override void Spawn(Vector3 position, SpawnOptions options)
    {
        if (options != null && options.useCustomRotation)
        {
            ObjectPooler.Instance.SpawnFromPool(
                ObjectTag.ToString(),
                position,
                Quaternion.Euler(0, 0, options.rotationZ)
            );
        }
        else
        {
            Spawn(position);
        }
    }
}
