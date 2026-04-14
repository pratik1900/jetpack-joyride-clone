using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 2.0f;


    private float maxSpawnY = 1.75f;
    private float minSpawnY = -1.75f;
    private float spawnX = 15.0f;

    private Dictionary<string, SpawnHandler> _handlers;


    void Start()
    {
        // Build a lookup table from all handlers attached to child GameObjects
        _handlers = new Dictionary<string, SpawnHandler>();
        foreach (SpawnHandler handler in GetComponentsInChildren<SpawnHandler>())
        {
            _handlers[handler.ObjectTag.ToString()] = handler;
        }
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPos = new Vector3(
                spawnX,
                Random.Range(minSpawnY, maxSpawnY)
            );

            string objTagToSpawn = RandomObjectTagToSpawn();

            if (string.IsNullOrEmpty(objTagToSpawn) ||
                !_handlers.TryGetValue(objTagToSpawn, out SpawnHandler handler)
            )
            {
                Debug.LogWarning("No Object Tag found for spawning");
                continue;
            }

            handler.Spawn(spawnPos);

            // ObjectPooler.Instance.SpawnFromPool(
            //     objTagToSpawn,
            //     spawnPos,
            //     Quaternion.Euler(0, 0, Random.Range(0, 360))
            // );
        }
    }

    private string RandomObjectTagToSpawn()
    {
        ObjectPooler pooler = ObjectPooler.Instance;

        if (pooler == null || pooler.poolsDetailsList == null || pooler.poolsDetailsList.Count == 0)
        {
            Debug.LogWarning("No Pools Configured for Spawning");
            return null;
        }

        string objTagToSpawn = pooler.poolsDetailsList[Random.Range(0, pooler.poolsDetailsList.Count)].tag;
        return objTagToSpawn;
    }
}
