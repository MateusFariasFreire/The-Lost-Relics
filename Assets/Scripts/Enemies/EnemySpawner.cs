using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnCooldown = 1f;
    private float spawnTimer = 0f;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float spawnHeight = 1f;

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection.y = spawnHeight;

            Instantiate(enemyPrefab, transform.position + randomDirection, transform.rotation,transform);

            spawnTimer = spawnCooldown;
        }
    }
}
