
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
    public float dashDistance = 15f;

    public float dashTimeCounter = 0;
    public float timeBtwDash = 1f;
    private MonsterHealth monsterHealth;

    

    

    void Start()
    {
        
        // Trouve le joueur par son tag au démarrage
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        enemyRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dashTimeCounter = 0;
        monsterHealth = GetComponent<MonsterHealth>();
        
        
    }

    void FixedUpdate()
    {
        
        DetectPlayer();
        
    }
    void Update()
    {
        /* dashTimeCounter = TimerDecrement(dashTimeCounter); */
        
    }

    public void DetectPlayer()
    {
        AttackHitBoxSide attackHitBoxSide = GetComponentInChildren<AttackHitBoxSide>();
        if (playerTransform == null)
        {
            return;
        }
        if(!monsterHealth.isTakingDamage  && attackHitBoxSide.isAttacking == false)
        {
            
            // Vérifie la distance entre l'ennemi et le joueur
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
               
            if (distanceToPlayer <= detectionRange /* && dashTimeCounter <= 0 */)
            {
                 
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
                

                
                /* transform.position = new Vector2(
                    transform.position.x + dashDirection.x * dashDistance,
                    transform.position.y + dashDirection.y * dashDistance
                ); 
                Vector2 dashDirection = (playerTransform.position - transform.position).normalized;
                enemyRb.AddForce(dashDirection * dashDistance, ForceMode2D.Impulse); */
                
                /* dashTimeCounter = timeBtwDash; */
                

                animator.SetFloat("Speed",Mathf.Abs(speed * Time.deltaTime));
                animator.SetBool("IsWalking", true);
                if(playerTransform.position.x > transform.position.x)
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

    private float TimerDecrement(float timeCounter)
    {
        if (timeCounter >= 0f)
            timeCounter -= Time.deltaTime;

        return timeCounter;
    }

    

    
}
