using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

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
        public GameObject eventPrefab;
        public float chanceToOccur;
    }

    public List<RandomEvent> randomEvents;
    public Transform[] eventSpawnPoints;
    public List<Wave> waves;
    public Transform[] spawnPoints;

    public int currentWaveIndex = 0;
    private int enemiesRemainingToSpawn;
    public GameObject[] enemiesRemainingAlive;
    private bool spawningEnemies = false;
    public PlayerMovement playerMovement;
    public int numerOfWaveDone;
    public Transform playerTransform;

    private Wave wave;
    private Transform spawnPoint;
    private GameObject enemy;
    private MonsterHealth monsterHealth;
    private float randomValue;
    private EnemyCircleSpawner spawner;
    public Camera myCamera;
    private Color tempColor, tempColorMoon, tempColorFuji;
    private Color[] tempColorBuildings;
    public float redA, redB, redC;
    public PlayerScore playerScore;
    public SpriteRenderer myMoon;
    public SpriteRenderer myFuji;
    public SpriteRenderer[] myBuildings;
    

    void Start()
    {
        if(myCamera != null)
        tempColor = myCamera.backgroundColor;
        if(myMoon != null)
        tempColorMoon = myMoon.color;
        if(myFuji != null)
        tempColorFuji = myFuji.color;

        if(myBuildings != null)
        {
            tempColorBuildings = new Color[myBuildings.Length];
            for (int i = 0; i < myBuildings.Length; i++)
            {
                tempColorBuildings[i] = myBuildings[i].color;
            }
        }
    }
    void Update()
    {
        enemiesRemainingAlive = GameObject.FindGameObjectsWithTag("Enemy");

        if (playerMovement.wantToFight)
        {
            if (!spawningEnemies && enemiesRemainingAlive.Length == 0)
            {
                if (currentWaveIndex < waves.Count)
                {
                    StartCoroutine(SpawnWave());
                }
                else
                {
                    GetComponent<EndMission>().EnemyNumber();
                }
            }
        }
        else if (!spawningEnemies && enemiesRemainingAlive.Length != 0)
        {
            // Désactivation des ennemis présents si le joueur ne souhaite plus combattre
            foreach (GameObject enemy in enemiesRemainingAlive)
            {
                EnemyPoolManager.Instance.ReturnEnemyToPool(enemy);
            }
        }

        if(currentWaveIndex == 5 || currentWaveIndex == 11)
        {
            myCamera.backgroundColor = new Color(redA, redB, redC);
            myMoon.color = new Color(redA, redB, redC);
            myFuji.color = new Color(1, redB, redC);
            for (int i = 0; i < myBuildings.Length; i++)
            {
                myBuildings[i].color = new Color(redA, redB, redC);
            }
            
        } else
        {
            myCamera.backgroundColor = tempColor;
            myMoon.color = tempColorMoon;
            myFuji.color = tempColorFuji;
            for (int i = 0; i < myBuildings.Length; i++)
            {
                myBuildings[i].color = tempColorBuildings[i];
            }
        }
    }

    IEnumerator SpawnWave()
    {
        spawningEnemies = true;
        wave = waves[currentWaveIndex];
        playerScore.roundsMax += 1;
        currentWaveIndex++;
        numerOfWaveDone++;
        if(numerOfWaveDone >= waves.Count && currentWaveIndex != 5 && currentWaveIndex != 11)
            wave.numberOfEnemies += 5;
        enemiesRemainingToSpawn = wave.numberOfEnemies;
        for (int i = 0; i < enemiesRemainingToSpawn; i++)
        {
            SpawnEnemy(wave.enemyPrefab[Random.Range(0, wave.enemyPrefab.Length)]);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }

        spawningEnemies = false;
        

        if (currentWaveIndex < waves.Count && numerOfWaveDone >= 6 && currentWaveIndex != 5 && currentWaveIndex != 11)
        {
            TryTriggerRandomEvent();
        }
        else if (currentWaveIndex == waves.Count)
        {
            Debug.Log("All waves completed!");
            yield return null;
        }

        
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy = EnemyPoolManager.Instance.GetEnemy(enemyPrefab);

        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            monsterHealth = enemy.GetComponent<MonsterHealth>();
            monsterHealth.health = monsterHealth.maxHealth;
        }
    }

    void TryTriggerRandomEvent()
    {
        foreach (RandomEvent randomEvent in randomEvents)
        {
            if (Random.Range(0f, 1f) <= randomEvent.chanceToOccur)
            {
                spawnPoint = eventSpawnPoints[Random.Range(0, eventSpawnPoints.Length)];

                if (randomEvent.eventName == "CircleOfEnemies" || randomEvent.eventName == "CircleOfEnemies2" || randomEvent.eventName == "CircleOfEnemies3" || randomEvent.eventName == "CircleOfEnemies4")
                {
                    spawner = randomEvent.eventPrefab.GetComponent<EnemyCircleSpawner>();
                    spawner.SpawnEnemiesAroundPlayer(playerTransform);
                }
                else
                {
                    Instantiate(randomEvent.eventPrefab, spawnPoint.position, spawnPoint.rotation);
                    Debug.Log("Event triggered: " + randomEvent.eventName);
                }
            }
        }
    }
}
