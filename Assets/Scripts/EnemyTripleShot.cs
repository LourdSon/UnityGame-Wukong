using UnityEngine;

public class EnemyTripleShot : MonoBehaviour
{
    // public GameObject energyBallPrefab;  // Associe ton prefab de boule d'énergie ici
    // public Transform firePoint;  // Point à partir duquel la boule d'énergie sera tirée
    public float shootCooldown = 7f;  // Temps entre chaque tir
    public float angleOffset = 15f;
    private float timeSinceLastShot = 0f;
    public Transform player;
    public ObjectPool3 objectPool;
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

        // Crée le tir principal
        CreateEnergyBall(direction);

        // Crée deux tirs supplémentaires de chaque côté avec une légère rotation (exemple : ±15 degrés)
        angleOffset = 15f;

        Vector2 directionLeft = Quaternion.Euler(0, 0, angleOffset) * direction;
        Vector2 directionRight = Quaternion.Euler(0, 0, -angleOffset) * direction;

        CreateEnergyBall(directionLeft);
        CreateEnergyBall(directionRight);
    }

    void CreateEnergyBall(Vector2 dir)
    {
        energyBall = objectPool.GetPooledObject();
        if (energyBall != null)
        {
            energyBall.SetActive(true);
            energyBall.transform.position = myTransform.position;
            energyBall.GetComponent<EnergyBallEnemyScript>().SetDirection(dir);
        }
    }
}
