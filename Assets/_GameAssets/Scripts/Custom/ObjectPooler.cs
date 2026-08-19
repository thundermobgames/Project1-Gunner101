using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {

        public enum objTags
        {
            PlayerBullet,
            EnemyBullet,
            EnemyObject1,
            EnemyObject2
        }

        public objTags tag;
        public GameObject prefab;
        public Transform objsParent;
        public int size;
    }

    #region Singleton

    public static ObjectPooler Instance;
    public void Awake()
    {
        Instance = this;
    }

    #endregion
    
    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.objsParent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag.ToString(), objectPool);
        }
    }

    public GameObject spawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject spawnedObject = poolDictionary[tag].Dequeue();
        spawnedObject.SetActive(true);
        spawnedObject.transform.position = position;
        spawnedObject.transform.rotation = rotation;

        return spawnedObject;
    }

    public void returnToPool(string tag, GameObject spawnedObject)
    {
        spawnedObject.SetActive(false);
        poolDictionary[tag].Enqueue(spawnedObject);
    }
    
}
