using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // These are the variables needed to know what to spawn, how many have spawned,
    // and if the spacing prefab has been spawned
    public GameObject[] prefabPool;
    private List<GameObject> spawnedPrefabs;
    private bool lastPrefabWasSpacing = false;
    
    private void Awake()
    {
        // this was made just so i didn't have to add each new prefab into the game manager manually
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
        // This makes sure to only keep 14 prefabs in the game at a time, no reason to keep used prefabs around
        if (spawnedPrefabs.Count > 14)
        {
            Destroy(spawnedPrefabs[0]);
            spawnedPrefabs.RemoveAt(0);
        }    
    }

    // I found that starting this method allows me to make as many new prefabs without having to worry adding
    // each one to the game manager manually
    private void loadPrefabs()
    {
        prefabPool = Resources.LoadAll<GameObject>("Prefabs");
    }

    // This is only used to spawn the starting prefabs
    private void StartSpawn()
    {
        int prefabIndex = Random.Range(0, prefabPool.Length);

        // This makes sure that no the spacing prefab can't be spawned, want the player to build some decent speed to start
        if (prefabPool[prefabIndex].name == "Spacing")        
        {
            prefabIndex--;
        }
        
        GameObject prefab = prefabPool[prefabIndex];

        // This float is just to know how far apart the prefabs gotta spawn
        float prefabSpacing = 100f;

        // Calculate the spawn position based on the number of spawned prefabs, as it loops
        Vector3 spawnPosition = new Vector3(0, 0, (spawnedPrefabs.Count + 1) * prefabSpacing);

        GameObject newPrefabInstance = Instantiate(prefab, spawnPosition, Quaternion.identity);

        spawnedPrefabs.Add(newPrefabInstance);
    }
    
    public void SpawnPrefab(Vector3 position)
    {
        int prefabIndex;

        // This just makes sure that after the first 6 prefabs that in between each prefab piece, there is the spacing,
        // just so it doesn't clutter the game and allows more swinging and movement space
        if (lastPrefabWasSpacing)
        {
            prefabIndex = Random.Range(0, prefabPool.Length);
            // Just checks if the name was Spacing, because sometimes it can randomly choose the spacing prefab again
            if (prefabPool[prefabIndex].name == "Spacing")
            {
                // If this happens, it just sets the index down one, still random really
                prefabIndex--;
            }
            lastPrefabWasSpacing = false;
        }
        else
        {
            prefabIndex = Array.FindIndex(prefabPool, prefab => prefab.name == "Spacing");
            lastPrefabWasSpacing = true;
        }
        
        // Spawning the new prefab 
        GameObject prefab = prefabPool[prefabIndex];
        
        Vector3 newPos = new Vector3(0, 0, position.z + 600f);
        
        GameObject newPrefab = Instantiate(prefab, newPos, Quaternion.identity);
        spawnedPrefabs.Add(newPrefab);
    }
}
