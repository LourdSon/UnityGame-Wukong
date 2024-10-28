


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

    void Start()
    {
        
        playerTransform = player.transform;
        
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        
        
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
        MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
        AttackHitBoxKamikaze attackHitBoxKamikaze = GetComponentInChildren<AttackHitBoxKamikaze>();
        if (playerTransform == null && player != null)
        {
            playerTransform = player.transform;
        }
        if(!monsterHealth.isTakingDamage  && attackHitBoxKamikaze.isAttacking == false && playerTransform != null)
        {
            
            // Vérifie la distance entre l'ennemi et le joueur
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);        
            if (distanceToPlayer <= detectionRange)
            {
                // Déplace l'ennemi vers le joueur uniquement sur l'axe X
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
                animator.SetFloat("Speed",Mathf.Abs(speed * Time.deltaTime));
                animator.SetBool("IsWalking", true);
                if(playerTransform.position.x > transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);
                } else 
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);
                }
                
            } else
            {
                animator.SetBool("IsWalking", false);
            }
            
        }
        
    }

    void SeparateFromOtherEnemies()
    {
        // Récupère tous les ennemis dans un rayon autour de cet ennemi
        Collider2D[] enemiesNearby = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (Collider2D other in enemiesNearby)
        {
            if (other != null && other.gameObject != this.gameObject && other.CompareTag("Enemy"))
            {
                // Calcule une force de répulsion si trop proche
                Vector3 repelDirection = transform.position - other.transform.position;
                float distance = repelDirection.magnitude;

                // Applique la force de répulsion
                if (distance < separationRadius)
                {
                    transform.position += repelDirection.normalized * separationForce * Time.deltaTime;
                }
            }
        }
    }
}
