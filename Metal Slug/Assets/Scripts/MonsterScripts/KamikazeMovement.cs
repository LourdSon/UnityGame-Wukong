


using UnityEngine;

public class KamikazeMovement : MonoBehaviour
{

    public float detectionRange = 500f;
    public float speed = 20f; // Vitesse de déplacement de l'ennemi

    public Transform playerTransform;
        
    private Rigidbody2D enemyRb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject player;

    public float separationRadius = 5f; // Rayon pour éviter la superposition
    public float separationForce = 15f; // Force pour éviter la superposition






    private MonsterHealth monsterHealth;
    private AttackHitBoxKamikaze attackHitBoxKamikaze;
    private float distanceToPlayer;
    private Collider2D[] enemiesNearby;
    private Vector3 repelDirection;
    private float distance;
    private Transform myTransform;
    private float nextSeparationCheckTime = 0f;
    private float separationCheckInterval = 0.1f;
    private float separationRadiusSqr;
    private float distanceSqr;

    void Start()
    {
        
        playerTransform = player.transform;
        
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterHealth = GetComponent<MonsterHealth>();
        attackHitBoxKamikaze = GetComponentInChildren<AttackHitBoxKamikaze>();
        myTransform = transform;
        
        
    }

    void FixedUpdate()
    {
        if (playerTransform != null && player != null)
        {
            playerTransform = player.transform;
        }if(playerTransform == null && player != null)
        {
            playerTransform = player.transform;
        }
        DetectPlayer();
        SeparateFromOtherEnemies();
        
    }


    public void DetectPlayer()
    {
        
        if (playerTransform == null && player != null)
        {
            playerTransform = player.transform;
        }
        if(!monsterHealth.isTakingDamage  && attackHitBoxKamikaze.isAttacking == false && playerTransform != null)
        {
            
            // Vérifie la distance entre l'ennemi et le joueur
            distanceToPlayer = Vector2.Distance(myTransform.position, playerTransform.position);        
            if (distanceToPlayer <= detectionRange)
            {
                // Déplace l'ennemi vers le joueur uniquement sur l'axe X
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
