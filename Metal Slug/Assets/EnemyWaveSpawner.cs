using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EnemyWaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 20;
    public float spawnRadius = 5f;
    public int enemyIndex;
    void Start()
    {
        SpawnBigWave();
    }

    void SpawnBigWave()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Choisir une position aléatoire dans un rayon autour du spawner
            Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
            Vector2 spawnPosition = new Vector2(transform.position.x + randomPosition.x, transform.position.y + randomPosition.y);

            // Instancier l'ennemi
            // Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            // Récupérer l'ennemi en fonction de son prefab depuis l'object pool
            GameObject enemy = EnemyPoolManager.Instance.GetEnemy(enemyPrefab);

            if (enemy != null)
            {
                // Placer l'ennemi à la position de spawn
                enemy.transform.position = spawnPosition;
            }
            else
            {
                Debug.LogError("Failed to spawn enemy.");
            }
        }
        
    }
}
