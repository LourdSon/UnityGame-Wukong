


using UnityEngine;

public class FlyingMonsterMovement : MonoBehaviour
{

    public float detectionRange = 500f;
    public float speed = 20f; // Vitesse de déplacement de l'ennemi

    public Transform target;
        
    private Rigidbody2D enemyRb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameObject npcTarget;
    public GameObject player;
    public SaveableNPC saveableNPC;
    
    public float separationRadius = 1f; // Rayon pour éviter la superposition
    public float separationForce = 1f; // Force pour éviter la superposition


    

    void Start()
    {
        
        // Trouve le joueur par son tag au démarrage
        
        target = player.transform;
        
        
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        
        
        
    }

    void FixedUpdate()
    {
        // Si un PNJ est présent, les ennemis le ciblent
        npcTarget = GameObject.FindGameObjectWithTag("NPCs");
        

        if (npcTarget != null && player != null)
        {
            saveableNPC = npcTarget.GetComponent<SaveableNPC>();
            if(!saveableNPC.playerInZone)
            {
                target = npcTarget.transform;
            }
            else if(saveableNPC.playerInZone)
            {
                target = player.transform;
            }
        } 
        if(npcTarget == null && player != null)
        {
            target = player.transform;
        }

        DetectPlayer();
        SeparateFromOtherEnemies();
        
    }
   

    public void DetectPlayer()
    {
        MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
        AttackHitBoxSide attackHitBoxSide = GetComponentInChildren<AttackHitBoxSide>();
        EnemyShoot enemyShoot = GetComponentInChildren<EnemyShoot>();
        if(player != null)
        {

        
            if(attackHitBoxSide != null)
            {
                if(!monsterHealth.isTakingDamage  && attackHitBoxSide.isAttacking == false)
                {
                    
                    // Vérifie la distance entre l'ennemi et le joueur
                    float distanceToPlayer = Vector2.Distance(transform.position, target.position);        
                    if (distanceToPlayer <= detectionRange)
                    {
                        // Déplace l'ennemi vers le joueur uniquement sur l'axe X
                        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                        animator.SetFloat("Speed",Mathf.Abs(speed * Time.deltaTime));
                        animator.SetBool("IsWalking", true);
                        if(target.position.x > transform.position.x)
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
            if(enemyShoot != null)
            {
                if(!monsterHealth.isTakingDamage && enemyShoot.isShooting == false)
                {
                    
                    // Vérifie la distance entre l'ennemi et le joueur
                    float distanceToPlayer = Vector2.Distance(transform.position, target.position);        
                    if (distanceToPlayer <= detectionRange)
                    {
                        // Déplace l'ennemi vers le joueur uniquement sur l'axe X
                        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                        animator.SetFloat("Speed",Mathf.Abs(speed * Time.deltaTime));
                        animator.SetBool("IsWalking", true);
                        if(target.position.x > transform.position.x)
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
