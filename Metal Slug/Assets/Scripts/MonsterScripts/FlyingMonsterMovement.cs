


using UnityEngine;

public class FlyingMonsterMovement : MonoBehaviour
{

    public float detectionRange = 500f;
    public float speed = 20f; // Vitesse de déplacement de l'ennemi

    private Transform target;
        
    private Rigidbody2D enemyRb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameObject npcTarget;
    public GameObject player;

    

    void Start()
    {
        
        // Trouve le joueur par son tag au démarrage
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        
        
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
        
        
    }

    void FixedUpdate()
    {
        // Si un PNJ est présent, les ennemis le ciblent
        npcTarget = GameObject.FindGameObjectWithTag("NPCs");

        if (npcTarget != null)
        {
            target = npcTarget.transform;
        } else 
        {
            target = player.transform;
        }

        DetectPlayer();
        
    }
    void Update()
    {
        
    }

    public void DetectPlayer()
    {
        MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
        AttackHitBoxSide attackHitBoxSide = GetComponentInChildren<AttackHitBoxSide>();
        if (target == null)
        {
            return;
        }
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
                    spriteRenderer.flipX = false;
                } else 
                {
                    spriteRenderer.flipX = true;
                }
                
            } else
            {
                animator.SetBool("IsWalking", false);
            }
            
        }
        
    }
}
