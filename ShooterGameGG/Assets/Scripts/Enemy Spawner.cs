using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 2f;
    public int SpawnLimit = 10;
    public int maxEnemies = 70;
    public float spawnRadius = 10f;
    public Transform player;

    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnRate);
    }

    private void SpawnEnemy()
    {

        if(GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemies)
        {
            return;
        }

        for (int i = 0; i < SpawnLimit; i++)
        {
            Vector2 spawnPosition = GetRandomSpawnPos();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector2 GetRandomSpawnPos()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;


        bool SpawnOutsideHoriz = UnityEngine.Random.value > 0.5f;

        Vector2 spawnPosition;

        if (SpawnOutsideHoriz)
        {
            //spawn left or right of the camera view
            float x = player.position.x + (UnityEngine.Random.value > 0.5f ? 1 : -1) * (cameraWidth / 2 + spawnRadius);
            float y = player.position.y + UnityEngine.Random.Range(-cameraHeight / 2, cameraHeight / 2);
            spawnPosition = new Vector3(x, y, 0f);
        }
        else
        {
            //spawn above or below the camera view
            float x = player.position.x + UnityEngine.Random.Range(-cameraWidth / 2, cameraWidth / 2);
            float y = player.position.y + (UnityEngine.Random.value > 0.5f ? 1 : -1) * (cameraHeight / 2 + spawnRadius);
            spawnPosition = new Vector3(x, y, 0f);
        }
        
        return spawnPosition;
    }
}
