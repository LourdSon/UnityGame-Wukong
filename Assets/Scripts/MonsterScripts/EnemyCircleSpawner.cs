using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircleSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;     // Prefab de l'ennemi à spawner
    public int enemyCount = 30;        // Nombre d'ennemis à faire apparaître
    public float circleRadius = 5f;    // Rayon du cercle autour du joueur

    private float angleStep;
    private float angle;
    private Vector2 spawnPosition;
    private GameObject enemy;
    private Transform enemyTransform;

    public void SpawnEnemiesAroundPlayer(Transform playerTransform)
    {
        angleStep = 360f / enemyCount; // Calculer l'angle entre chaque ennemi

        for (int i = 0; i < enemyCount; i++)
        {
            // Calculer la position de l'ennemi en cercle autour du joueur
            angle = i * angleStep;
            spawnPosition = new Vector2(
                playerTransform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * circleRadius,
                playerTransform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * circleRadius);

            // Instancier l'ennemi à la position calculée
            // Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy = EnemyPoolManager.Instance.GetEnemy(enemyPrefab);
            enemyTransform = enemy.transform;
            if(enemy != null)
            {
                enemyTransform.position = spawnPosition;
            }
            if(enemyTransform.position.y < -10 || enemyTransform.position.y > 230 || enemyTransform.position.x < 30 || enemyTransform.position.x > 555)
            {
                EnemyPoolManager.Instance.ReturnEnemyToPool(enemy);
            }
        }
        

        Debug.Log("Enemies spawned in a circle around the player.");
    }
}
