using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject crabPrefab;
    public float spawnRate = 2f;
    public float spawnHeight = 10f; 

    private float nextSpawnTime;

    private MyProjectGameManager gameManager;


    void Start()
    {
        gameManager = FindObjectOfType<MyProjectGameManager>();
    }

    void Update()
    {
        if (gameManager.gameOverPanel.activeSelf)
        {
            return; 
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnCrab();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnCrab()
    {
        float randomX = Random.Range(100f, 1100f); 
        float randomY = Random.Range(500f, 500f); 

        Vector3 spawnPosition = new Vector3(randomX, Camera.main.transform.position.y + randomY, 0);

        Instantiate(crabPrefab, spawnPosition, Quaternion.identity);
    }

}
