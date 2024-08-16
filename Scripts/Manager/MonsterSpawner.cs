using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoint
{
    public Transform spawnTransform;
    public GameObject monsterPrefab;
    public int monsterCount;
    public float spawnRadius;
    [HideInInspector]
    public int spawnedCount;
}

public class MonsterSpawner : MonoBehaviour
{
    public SpawnPoint[] spawnPoints;
    public float spawnInterval = 5f;

    private float timeSinceLastSpawn;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Transform playerTransform;

    void Start()
    {
        // 레이어로 플레이어 찾기
        playerTransform = FindPlayerByLayer("PlayerLayer");
        if (playerTransform == null)
        {
            //Debug.LogError("PlayerLayer 레이어를 가진 오브젝트를 찾을 수 없습니다.");
            return;
        }

        timeSinceLastSpawn = spawnInterval;
        InitializeSpawnPoints();
        InitializePool();
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnMonsters();
            timeSinceLastSpawn = 0f;
        }
    }

    private void InitializeSpawnPoints()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.spawnedCount = 0;
        }
    }

    private void InitializePool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var spawnPoint in spawnPoints)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < spawnPoint.monsterCount; i++)
            {
                GameObject obj = Instantiate(spawnPoint.monsterPrefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(spawnPoint.monsterPrefab.name, objectPool);
        }
    }

    void SpawnMonsters()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (Vector3.Distance(playerTransform.position, spawnPoint.spawnTransform.position) <= spawnPoint.spawnRadius)
            {
                SpawnMonstersAtPoint(spawnPoint);
            }
        }
    }

    private void SpawnMonstersAtPoint(SpawnPoint spawnPoint)
    {
        for (int i = spawnPoint.spawnedCount; i < spawnPoint.monsterCount; i++)
        {
            GameObject monster = SpawnFromPool(spawnPoint.monsterPrefab.name, spawnPoint.spawnTransform.position, spawnPoint.spawnTransform.rotation);
            if (monster != null)
            {
                spawnPoint.spawnedCount++;
            }
        }
    }

    private GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            //Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        var objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    private Transform FindPlayerByLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        GameObject[] objects = FindObjectsOfType<GameObject>();

        foreach (var obj in objects)
        {
            if (obj.layer == layer)
            {
                return obj.transform;
            }
        }

        return null;
    }
}