using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject[] prefabPool;

    private List<GameObject> spawnedPrefabs;
    
    private bool lastPrefabWasSpacing = false;
    
    private void Awake()
    {
        loadPrefabs();
    }

    private void Start()
    {
        spawnedPrefabs = new List<GameObject>();

        // Spawn the initial 6 prefabs
        for (int i = 0; i < 6; i++)
        {
            StartSpawn();
        }
    }

    private void Update()
    {
        if (spawnedPrefabs.Count > 14)
        {
            Destroy(spawnedPrefabs[0]);
            spawnedPrefabs.RemoveAt(0);
        }    
    }

    private void loadPrefabs()
    {
        prefabPool = Resources.LoadAll<GameObject>("Prefabs");
    }

    private void StartSpawn()
    {
        int prefabIndex = Random.Range(0, prefabPool.Length);

        if (prefabPool[prefabIndex].name == "Spacing")        
        {
            prefabIndex--;
        }
        
        GameObject prefab = prefabPool[prefabIndex];

        float prefabSpacing = 100f;

        // Calculate the spawn position based on the number of spawned prefabs
        Vector3 spawnPosition = new Vector3(0, 0, (spawnedPrefabs.Count + 1) * prefabSpacing);

        GameObject newPrefabInstance = Instantiate(prefab, spawnPosition, Quaternion.identity);

        spawnedPrefabs.Add(newPrefabInstance);
    }
    
    public void SpawnPrefab(Vector3 position)
    {
        int prefabIndex;

        if (lastPrefabWasSpacing)
        {
            prefabIndex = Random.Range(0, prefabPool.Length);
            if (prefabPool[prefabIndex].name == "Spacing")
            {
                prefabIndex--;
            }
            lastPrefabWasSpacing = false;
        }
        else
        {
            prefabIndex = Array.FindIndex(prefabPool, prefab => prefab.name == "Spacing");
            lastPrefabWasSpacing = true;
        }
        
        GameObject prefab = prefabPool[prefabIndex];
        
        Vector3 newPos = new Vector3(0, 0, position.z + 600f);
        
        GameObject newPrefab = Instantiate(prefab, newPos, Quaternion.identity);
        spawnedPrefabs.Add(newPrefab);
    }


}
