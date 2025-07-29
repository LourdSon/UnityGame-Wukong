


using UnityEngine;

public class MonsterMovement : MonoBehaviour
{

    public float detectionRange = 30f;
    public float detectionRangeAttack = 10f; // Portée de détection de l'ennemi
    public float speed = 5f; // Vitesse de déplacement de l'ennemi

    private Transform playerTransform;
        
    private Rigidbody2D enemyRb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    

    private GameObject player;
    private MonsterHealth monsterHealth;
    private AttackHitBoxSide attackHitBoxSide;
    private float distanceToPlayer;
    private float newXPosition;
    private Transform myTransform;
    

    void Start()
    {
        
        // Trouve le joueur par son tag au démarrage
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterHealth = GetComponent<MonsterHealth>();
        attackHitBoxSide = GetComponentInChildren<AttackHitBoxSide>();
        myTransform = transform;
        
        
    }

    void FixedUpdate()
    {
        
        DetectPlayer();
        
    }
    

    private void DetectPlayer()
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
                // Déplace l'ennemi vers le joueur uniquement sur l'axe X
                newXPosition = Mathf.MoveTowards(myTransform.position.x, playerTransform.position.x, speed * Time.deltaTime);
                myTransform.position = new Vector2(newXPosition, myTransform.position.y);
                animator.SetFloat("Speed",Mathf.Abs(newXPosition));
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
}
