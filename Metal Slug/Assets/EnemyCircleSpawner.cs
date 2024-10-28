using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircleSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;     // Prefab de l'ennemi à spawner
    public int enemyCount = 30;        // Nombre d'ennemis à faire apparaître
    public float circleRadius = 5f;    // Rayon du cercle autour du joueur

    public void SpawnEnemiesAroundPlayer(Transform playerTransform)
    {
        float angleStep = 360f / enemyCount; // Calculer l'angle entre chaque ennemi

        for (int i = 0; i < enemyCount; i++)
        {
            // Calculer la position de l'ennemi en cercle autour du joueur
            float angle = i * angleStep;
            Vector2 spawnPosition = new Vector2(
                playerTransform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * circleRadius,
                playerTransform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * circleRadius);

            // Instancier l'ennemi à la position calculée
            // Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            GameObject enemy = EnemyPoolManager.Instance.GetEnemy(enemyPrefab);
            if(enemy != null)
            {
                enemy.transform.position = spawnPosition;
            }
            if(enemy.transform.position.y < -10 || enemy.transform.position.y > 230 || enemy.transform.position.x < 30 || enemy.transform.position.x > 555)
            {
                EnemyPoolManager.Instance.ReturnEnemyToPool(enemy);
            }
        }
        

        Debug.Log("Enemies spawned in a circle around the player.");
    }
}
