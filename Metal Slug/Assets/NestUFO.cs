using System.Collections;
using UnityEngine;

public class NestUFO : MonoBehaviour
{
    // public GameObject energyBallPrefab;  // Associe ton prefab de boule d'énergie ici
    // public Transform firePoint;  // Point à partir duquel la boule d'énergie sera tirée
    public float shootCooldown = 2f;  // Temps entre chaque tir
    private float timeSinceLastShot = 0f;
    public bool isShooting;
    private Transform myTransform;
    private GameObject enemy;
    private MonsterHealth monsterHealth;
    public GameObject enemyPrefab;
    public Vector3 offset;
    public GameObject myLight;


    void Start()
    {
        myTransform = transform;
        timeSinceLastShot = shootCooldown;
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= shootCooldown)
        {
            isShooting = true;
            StartCoroutine(lightingUFO());
            Shoot();
            timeSinceLastShot = 0f;  // Reset le temps depuis le dernier tir
        } else
        {
            
            isShooting = false;
        }
    }

    void Shoot()
    {
        enemy = EnemyPoolManager.Instance.GetEnemy(enemyPrefab);

        if (enemy != null)
        {
            enemy.transform.position = myTransform.position + offset;
            monsterHealth = enemy.GetComponent<MonsterHealth>();
            monsterHealth.health = monsterHealth.maxHealth;
        }
    }

    private IEnumerator lightingUFO()
    {
        myLight.SetActive(true);
        yield return new WaitForSeconds(4);
        myLight.SetActive(false);
    }
}
