

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int numberOfEnemies;
        public GameObject[] enemyPrefab;
        public float spawnRate;
    }

    [System.Serializable]
    public class RandomEvent
    {
        public string eventName;
        public GameObject eventPrefab; // Le prefab de l'événement (par exemple un PNJ ou un objet)
        public float chanceToOccur; // Probabilité que l'événement se produise
    }

    public List<RandomEvent> randomEvents;
    public Transform[] eventSpawnPoints; // Points de spawn pour les événements aléatoires
    public List<Wave> waves;
    public Transform[] spawnPoints;

    public int currentWaveIndex = 0;
    private int enemiesRemainingToSpawn;
    private GameObject[] enemiesRemainingAlive;
    private bool spawningEnemies = false;
    public PlayerMovement playerMovement;
    public int numerOfWaveDone;
    public Transform playerTransform;

    void Update()
    {
        if(playerMovement.wantToFight)
        {
            enemiesRemainingAlive = GameObject.FindGameObjectsWithTag("Enemy");
            if (!spawningEnemies && enemiesRemainingAlive.Length == 0 && currentWaveIndex < waves.Count)
            {
                StartCoroutine(SpawnWave());
            } else if (!spawningEnemies && enemiesRemainingAlive.Length == 0 && currentWaveIndex == waves.Count )
            {
                GetComponent<EndMission>().EnemyNumber();
            }
        } if(!playerMovement.wantToFight)
        {
            enemiesRemainingAlive = GameObject.FindGameObjectsWithTag("Enemy");
            if(!spawningEnemies && enemiesRemainingAlive.Length != 0)
            {
                for(int i = 0; i < enemiesRemainingAlive.Length; i++)
                    EnemyPoolManager.Instance.ReturnEnemyToPool(enemiesRemainingAlive[i]);
            }
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
            SpawnEnemy(wave.enemyPrefab[Random.Range(0, wave.enemyPrefab.Length)]);
            // Tenter de déclencher un événement aléatoire
            
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        spawningEnemies = false;

        if (currentWaveIndex < waves.Count)
        {
            currentWaveIndex++;
            numerOfWaveDone++;
            TryTriggerRandomEvent();
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
        // GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject enemy = EnemyPoolManager.Instance.GetEnemy(enemyPrefab);
        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            MonsterHealth monsterHealth = enemy.GetComponent<MonsterHealth>();
            monsterHealth.health = monsterHealth.maxHealth;
        }

        
    }
    void TryTriggerRandomEvent()
    {
        foreach (RandomEvent randomEvent in randomEvents)
        {
            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= randomEvent.chanceToOccur)
            {
                if (randomEvent.eventName == "CircleOfEnemies" || randomEvent.eventName == "CircleOfEnemies2")
                {
                    // Déclencher l'événement "Cercle d'ennemis autour du joueur"
                    EnemyCircleSpawner spawner = randomEvent.eventPrefab.GetComponent<EnemyCircleSpawner>();
                    spawner.SpawnEnemiesAroundPlayer(playerTransform);
                }
                else
                {
                    // Si l'événement est déclenché, on le spawn à un point aléatoire
                    Transform spawnPoint = eventSpawnPoints[Random.Range(0, eventSpawnPoints.Length)];
                    Instantiate(randomEvent.eventPrefab, spawnPoint.position, spawnPoint.rotation);
                    Debug.Log("Event triggered: " + randomEvent.eventName);
                }
            }
        }
    }
    
    
}
