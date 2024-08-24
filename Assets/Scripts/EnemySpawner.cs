using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float spawnHeight = 1f;

    // Update is called once per frame
    void Update()
    {
        spawnRate -= Time.deltaTime;

        if (spawnRate <= 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection.y = spawnHeight;

            Instantiate(enemyPrefab, transform.position + randomDirection, transform.rotation,transform);

            spawnRate = 1f;
        }
    }
}
