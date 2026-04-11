using System.Collections;
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
        StartCoroutine(SpawnLaserCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnLaserCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            ObjectPooler.Instance.SpawnFromPool(
                "laser",
                new Vector3(
                    spawnX,
                    Random.Range(minSpawnY, maxSpawnY)
                ),
                Quaternion.Euler(0, 0, Random.Range(0, 360))
            );
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
