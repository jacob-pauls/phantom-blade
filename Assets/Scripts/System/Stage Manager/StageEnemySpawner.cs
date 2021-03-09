using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private GameObject instantiatedEnemy;
    [Space]
    [SerializeField] private Transform spawnTransform;
    private Vector2 spawnPosition;
    [SerializeField] private float respawnTimer = 5;
    private float remainingTime;
    [Space]
    [SerializeField] private bool enableSpawnerOnStart;
    private bool isSpawnerEnabled;

    public void StartSpawner()
    {
        remainingTime = respawnTimer;
        isSpawnerEnabled = true;
    }

    public void StopSpawner()
    {
        isSpawnerEnabled = false;
    }

    private void Update()
    {
        if (isSpawnerEnabled && enemy != null)
        {
            if (instantiatedEnemy == null)
            {
                if (remainingTime > 0)
                {
                    remainingTime -= Time.deltaTime;
                }
                else
                {
                    // Checking for where to place the enemy
                    Vector2 position = transform.position;
                    if (spawnTransform != null) { position = spawnTransform.position; }

                    // Creating the enemy
                    instantiatedEnemy = Instantiate(enemy, position, Quaternion.identity);

                    // Changing the name to make it cleaner
                    instantiatedEnemy.name = enemy.name;

                    // Resetting the timer
                    remainingTime = respawnTimer;
                }
            }

        }
    }
}
