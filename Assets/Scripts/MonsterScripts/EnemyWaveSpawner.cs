using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EnemyWaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 20;
    public float spawnRadius = 5f;
    public int enemyIndex;



    private Vector2 randomPosition;
    private Vector2 spawnPosition;
    private GameObject enemy;
    private Transform myTransform;
    private Transform enemyTransform;
    void Start()
    {
        myTransform = transform;
        SpawnBigWave();
    }

    void SpawnBigWave()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Choisir une position aléatoire dans un rayon autour du spawner
            randomPosition = Random.insideUnitCircle * spawnRadius;
            spawnPosition = new Vector2(myTransform.position.x + randomPosition.x, myTransform.position.y + randomPosition.y);

            // Instancier l'ennemi
            // Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            // Récupérer l'ennemi en fonction de son prefab depuis l'object pool
            enemy = EnemyPoolManager.Instance.GetEnemy(enemyPrefab);
            enemyTransform = enemy.transform;
            if (enemy != null)
            {
                // Placer l'ennemi à la position de spawn
                enemyTransform.position = spawnPosition;
            }
            else
            {
                Debug.LogError("Failed to spawn enemy.");
            }
        }
        
    }
}
