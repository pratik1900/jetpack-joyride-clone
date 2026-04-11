using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region  Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // This class stores metadata that will be used to create the actual pools 
    [System.Serializable] //makes the class visible in the inspector
    public class PoolDetails
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<PoolDetails> poolsDetailsList;

    void Start()
    {
        // will store all pools as k-v pairs (string (pool name) keys, the pools as the values)
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Creates all the different pools (for storing different kinds on objects) 
        foreach (PoolDetails poolDetails in poolsDetailsList)
        {
            // Create the actual pool
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Populte the current pool with objects
            for (int i = 0; i < poolDetails.size; i++)
            {
                GameObject obj = Instantiate(poolDetails.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(poolDetails.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " does not exist.");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning($"Pool '{tag}' is exhausted!");
            return null;
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        // Post-spawn logic (resetting values, etc.) 
        Debug.Log(objToSpawn.GetComponent<IPooledObject>());
        objToSpawn.GetComponent<IPooledObject>()?.OnObjectSpawn();

        // poolDictionary[tag].Enqueue(objToSpawn);  // This was there in the tutorial, but i separated it out in a different function (ReturnToPool)

        return objToSpawn;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }

}
