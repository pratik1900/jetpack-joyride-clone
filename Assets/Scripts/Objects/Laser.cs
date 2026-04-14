using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour, IPooledObject
{
    [SerializeField] private float leftBoundary = -15.0f;

    private void Update()
    {
        if (gameObject.transform.position.x < leftBoundary)
        {
            ReturnToPool();
        }
    }

    public void OnObjectSpawn()
    {
        // Post-Spawn Logic
        Debug.Log("Laser Spawned!");
    }

    public void ReturnToPool()
    {
        ObjectPooler.Instance.ReturnToPool("Laser", gameObject);
    }
}
