using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TriggerExit : MonoBehaviour
{
    // This is set to the triggers of each prefab and uses the spawnprefab method from
    // Game Manager to keep spawning new pieces
    private GameManager GameManager;

    private void Awake()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.SpawnPrefab(transform.position);
        }
    }
}
