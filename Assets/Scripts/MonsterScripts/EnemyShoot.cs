using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    // public GameObject energyBallPrefab;  // Associe ton prefab de boule d'énergie ici
    // public Transform firePoint;  // Point à partir duquel la boule d'énergie sera tirée
    public float shootCooldown = 2f;  // Temps entre chaque tir

    private float timeSinceLastShot = 0f;
    public Transform player;
    public ObjectPool objectPool;
    public bool isShooting;



    private Vector2 direction;
    private GameObject energyBall;
    private Transform myTransform;


    void Start()
    {
        myTransform = transform;
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= shootCooldown)
        {
            isShooting = true;
            Shoot();
            timeSinceLastShot = 0f;  // Reset le temps depuis le dernier tir
        } else
        {
            isShooting = false;
        }
    }

    void Shoot()
    {
        // Calcule la direction du tir vers le joueur
        direction = (player.position - myTransform.position).normalized;

        // Crée la boule d'énergie et définis sa direction
        energyBall = objectPool.GetPooledObject();
        if (energyBall != null)
        {
            energyBall.SetActive(true);
            energyBall.transform.position = myTransform.position;
            energyBall.GetComponent<EnergyBallEnemyScript>().SetDirection(direction);
        }
    }
}
