using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float spawnInterval = 2.0f;


    private float maxSpawnY = 1.75f;
    private float minSpawnY = -1.75f;
    private float spawnX = 15.0f;



    void Start()
    {
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
            string objTagToSpawn = RandomObjectTagToSpawn();

            if (objTagToSpawn == null)
            {
                Debug.LogWarning("No Object Tag found for spawning");
                yield break;
            }

            ObjectPooler.Instance.SpawnFromPool(
                objTagToSpawn,
                new Vector3(
                    spawnX,
                    Random.Range(minSpawnY, maxSpawnY)
                ),
                Quaternion.Euler(0, 0, Random.Range(0, 360))
            );
            yield return new WaitForSeconds(spawnInterval);
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
