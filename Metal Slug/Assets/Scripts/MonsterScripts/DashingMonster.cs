
using System.Collections;
using UnityEngine;

public class DashingMonster : MonoBehaviour
{

    public float detectionRange = 500f;
    public float speed = 20f; // Vitesse de déplacement de l'ennemi

    private Transform playerTransform;
        
    private Rigidbody2D enemyRb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 trajectoirePlayer;
    

    public float dashTimeCounter = 0;
    public float timeBtwDash = 1f;
    private MonsterHealth monsterHealth;

    public float separationRadius = 5f; // Rayon pour éviter la superposition
    public float separationForce = 15f; // Force pour éviter la superposition


    private AttackHitBoxSide attackHitBoxSide;
    private float distanceToPlayer;
    private Collider2D[] enemiesNearby;
    private Vector3 repelDirection;
    private float distance;
    private GameObject player;
    private Transform myTransform;
    private float nextSeparationCheckTime = 0f;
    private float separationCheckInterval = 0.1f;
    private float separationRadiusSqr;
    private float distanceSqr;
    

    

    

    void Start()
    {
        
        // Trouve le joueur par son tag au démarrage
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        enemyRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dashTimeCounter = 0;
        monsterHealth = GetComponent<MonsterHealth>();
        attackHitBoxSide = GetComponentInChildren<AttackHitBoxSide>();
        myTransform = transform;
        
        
    }

    void FixedUpdate()
    {
        
        DetectPlayer();
        SeparateFromOtherEnemies();
        
    }
    void Update()
    {
        /* dashTimeCounter = TimerDecrement(dashTimeCounter); */
        
    }

    public void DetectPlayer()
    {
        
        if (playerTransform == null)
        {
            return;
        }
        if(!monsterHealth.isTakingDamage  && attackHitBoxSide.isAttacking == false)
        {
            // Vérifie la distance entre l'ennemi et le joueur
            distanceToPlayer = Vector2.Distance(myTransform.position, playerTransform.position);
            if (distanceToPlayer <= detectionRange)
            {
                myTransform.position = Vector2.MoveTowards(myTransform.position, playerTransform.position, speed * Time.deltaTime);
                animator.SetFloat("Speed",Mathf.Abs(speed * Time.deltaTime));
                animator.SetBool("IsWalking", true);
                if(playerTransform.position.x > myTransform.position.x)
                {
                    myTransform.rotation = Quaternion.Euler(myTransform.rotation.x, 0f, myTransform.rotation.z);
                } else 
                {
                    myTransform.rotation = Quaternion.Euler(myTransform.rotation.x, 180f, myTransform.rotation.z);
                } 
            } else
            {
                animator.SetBool("IsWalking", false);
            }
            
        }
        
        
    }

    private float TimerDecrement(float timeCounter)
    {
        if (timeCounter >= 0f)
            timeCounter -= Time.deltaTime;

        return timeCounter;
    }

    /* void SeparateFromOtherEnemies()
    {
        // Récupère tous les ennemis dans un rayon autour de cet ennemi
        enemiesNearby = Physics2D.OverlapCircleAll(myTransform.position, separationRadius);

        foreach (Collider2D other in enemiesNearby)
        {
            if (other != null && other.gameObject != this.gameObject && other.CompareTag("Enemy"))
            {
                // Calcule une force de répulsion si trop proche
                repelDirection = myTransform.position - other.transform.position;
                distance = repelDirection.magnitude;

                // Applique la force de répulsion
                if (distance < separationRadius)
                {
                    myTransform.position += repelDirection.normalized * separationForce * Time.deltaTime;
                }
            }
        }
    } */

    private void SeparateFromOtherEnemies()
    {
        if (Time.time < nextSeparationCheckTime) return; // Éviter la vérification à chaque frame
        nextSeparationCheckTime = Time.time + separationCheckInterval;

        enemiesNearby = Physics2D.OverlapCircleAll(myTransform.position, separationRadius);

        separationRadiusSqr = separationRadius * separationRadius; // Évite le recalcul du rayon au carré
        foreach (Collider2D other in enemiesNearby)
        {
            if (other != null && other.gameObject != gameObject && other.CompareTag("Enemy"))
            {
                repelDirection = myTransform.position - other.transform.position;
                distanceSqr = repelDirection.sqrMagnitude; // Utilisation de sqrMagnitude au lieu de magnitude

                if (distanceSqr < separationRadiusSqr) // Comparaison avec la distance au carré
                {
                    myTransform.position += (Vector3)(repelDirection.normalized * separationForce * Time.deltaTime);
                }
            }
        }
    }

    

    
}
