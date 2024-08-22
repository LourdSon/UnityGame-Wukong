using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int numberOfEnemies;
        public GameObject enemyPrefab;
        public float spawnRate;
    }

    public List<Wave> waves;
    public Transform[] spawnPoints;

    private int currentWaveIndex = 0;
    private int enemiesRemainingToSpawn;
    private GameObject[] enemiesRemainingAlive;
    private bool spawningEnemies = false;

    void Update()
    {
        enemiesRemainingAlive = GameObject.FindGameObjectsWithTag("Enemy");
        if (!spawningEnemies && enemiesRemainingAlive.Length == 0 && currentWaveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave());
        } else if (!spawningEnemies && enemiesRemainingAlive.Length == 0 && currentWaveIndex == waves.Count )
        {
            GetComponent<EndMission>().EnemyNumber();
        }
    }

    IEnumerator SpawnWave()
    {
        spawningEnemies = true;

        Wave wave = waves[currentWaveIndex];
        enemiesRemainingToSpawn = wave.numberOfEnemies;
        //enemiesRemainingAlive = wave.numberOfEnemies;

        for (int i = 0; i < enemiesRemainingToSpawn; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        spawningEnemies = false;

        if (currentWaveIndex < waves.Count)
        {
            currentWaveIndex++;
        }
        else
        {
            Debug.Log("All waves completed!");
            //spawningEnemies = false;
            //GetComponent<EndMission>().YouWinScreen();
            yield return null;
            // Code à exécuter une fois que toutes les vagues sont terminées
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        
    }

    
}