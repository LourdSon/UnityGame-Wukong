using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;

    [System.Serializable]
    public class EnemyPool
    {
        public GameObject enemyPrefab; // Le prefab correspondant à l'ennemi
        public int poolSize = 100;      // Taille du pool pour ce type
    }

    public EnemyPool[] enemyPools;               // Tableau des pools d'ennemis
    private List<GameObject>[] poolArrays;       // Tableau de listes pour stocker chaque pool




    private GameObject enemy;
    private int enemyTypeIndex;
    private List<GameObject> enemyPool;
    private MonsterHealth monsterHealth;
    private FlyingMonsterMovement flyingMonsterMovement;
    private KamikazeMovement kamikazeMovement;
    private AttackHitBoxSide attackHitBoxSide;
    private AttackHitBoxKamikaze attackHitBoxKamikaze;




    void Awake()
    {
        // S'assurer qu'il n'y a qu'une seule instance de EnemyPoolManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        poolArrays = new List<GameObject>[enemyPools.Length];

        // Initialiser les pools pour chaque type d'ennemi
        for (int i = 0; i < enemyPools.Length; i++)
        {
            poolArrays[i] = new List<GameObject>();

            for (int j = 0; j < enemyPools[i].poolSize; j++)
            {
                enemy = Instantiate(enemyPools[i].enemyPrefab);
                enemy.SetActive(false); // Désactive l'ennemi pour l'ajouter au pool
                poolArrays[i].Add(enemy);
            }
        }
    }

    // Méthode pour récupérer un ennemi d'un pool spécifique
    public GameObject GetEnemy(GameObject enemy2)
    {
        enemyTypeIndex = GetEnemyTypeIndex(enemy2);
        
        if (enemyTypeIndex < 0)
        {
            Debug.LogError("Enemy prefab not found in pools: " + enemy2.name);
            return null;
        }

        enemyPool = poolArrays[enemyTypeIndex];

        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                // Récupération des composants nécessaires
                monsterHealth = enemy.GetComponent<MonsterHealth>();
                flyingMonsterMovement = enemy.GetComponent<FlyingMonsterMovement>();
                kamikazeMovement = enemy.GetComponent<KamikazeMovement>();
                attackHitBoxSide = enemy.GetComponentInChildren<AttackHitBoxSide>();
                attackHitBoxKamikaze = enemy.GetComponentInChildren<AttackHitBoxKamikaze>();

                
                if (monsterHealth != null)
                {
                    monsterHealth.health = monsterHealth.maxHealth;
                    monsterHealth.healthBar.UpdateHealthBar(monsterHealth.health, monsterHealth.maxHealth);
                    monsterHealth.currentShield = monsterHealth.maxShield;
                    monsterHealth.healthBar.UpdateShieldBar(monsterHealth.currentShield, monsterHealth.maxShield);
                }
                
                if (flyingMonsterMovement != null)
                {
                    flyingMonsterMovement.target = flyingMonsterMovement.player.transform;
                    // flyingMonsterMovement.DetectPlayer();
                }
                
                if (kamikazeMovement != null)
                {
                    kamikazeMovement.playerTransform = kamikazeMovement.player.transform;
                    // kamikazeMovement.DetectPlayer();
                }

                
                if (attackHitBoxSide != null)
                {
                    attackHitBoxSide.isAttacking = false;
                }
                if (attackHitBoxKamikaze != null)
                {
                    attackHitBoxKamikaze.isAttacking = false;
                }
                
                return enemy;
            }
        }
        return null;
        // // Si tous les ennemis du pool sont utilisés, en instancier un nouveau (optionnel)
        // GameObject newEnemy = Instantiate(enemyPools[enemyTypeIndex].enemyPrefab);
        // newEnemy.SetActive(true); // Activer le nouvel ennemi
        // enemyPool.Add(newEnemy);
        // return newEnemy;
    }

    // Méthode pour renvoyer un ennemi dans le pool
    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false); // Désactive simplement l'ennemi
    }

    // Méthode pour obtenir l'index du type d'ennemi basé sur le prefab
    private int GetEnemyTypeIndex(GameObject enemyPrefab)
    {
        for (int i = 0; i < enemyPools.Length; i++)
        {
            if (enemyPools[i].enemyPrefab == enemyPrefab)
            {
                return i;
            }
        }
        return 0; // Si le prefab n'est pas trouvé dans les pools
    }
}
