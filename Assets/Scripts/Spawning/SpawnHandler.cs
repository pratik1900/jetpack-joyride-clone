using UnityEngine;

public abstract class SpawnHandler : MonoBehaviour
{
    public abstract ObjectTags ObjectTag { get; }

    // Stores the custom spawn logic for different handlers (corresponding to different obj types)
    public abstract void Spawn(Vector3 position);
}