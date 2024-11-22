using UnityEngine;

public class FlyingMonsterMovement : MonoBehaviour
{
    public float detectionRange = 500f;
    public float speed = 20f;  // Vitesse de déplacement de l'ennemi
    public Transform player;
    public GameObject npcTarget;
    public float separationRadius = 1f;
    public float separationForce = 1f;

    public Transform target;
    private Rigidbody2D enemyRb;
    private Animator animator;
    private MonsterHealth monsterHealth;
    private AttackHitBoxSide attackHitBoxSide;
    private EnemyShoot enemyShoot;
    private Collider2D[] enemiesNearby;
    private Transform myTransform;
    private Vector3 repelDirection;
    private float distance;
    private float distanceToTarget;

    private float nextSeparationCheckTime = 0f;
    private float separationCheckInterval = 0.1f;
    private float separationRadiusSqr;
    private float distanceSqr;

    void Start()
    {
        myTransform = transform;
        target = player?.transform;
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponent<Animator>();
        monsterHealth = GetComponent<MonsterHealth>();
        attackHitBoxSide = GetComponentInChildren<AttackHitBoxSide>();
        enemyShoot = GetComponentInChildren<EnemyShoot>();
        npcTarget = GameObject.FindGameObjectWithTag("NPCs");
    }

    void FixedUpdate()
    {
        UpdateTarget();
        if (target != null)
        {
            MoveTowardsTarget();
        }
        SeparateFromOtherEnemies();
    }

    private void UpdateTarget()
    {
        if (npcTarget != null)
        {
            SaveableNPC saveableNPC = npcTarget.GetComponent<SaveableNPC>();
            target = saveableNPC != null && !saveableNPC.playerInZone ? npcTarget.transform : player.transform;
        }
        else
        {
            target = player?.transform;
        }
    }

    private void MoveTowardsTarget()
    {
        if (monsterHealth.isTakingDamage || 
            (attackHitBoxSide != null && attackHitBoxSide.isAttacking) || 
            (enemyShoot != null && enemyShoot.isShooting))
        {
            animator.SetBool("IsWalking", false);
            return;
        }

        distanceToTarget = Vector2.Distance(myTransform.position, target.position);
        if (distanceToTarget <= detectionRange)
        {
            myTransform.position = Vector2.MoveTowards(myTransform.position, target.position, speed * Time.deltaTime);
            animator.SetBool("IsWalking", true);
            myTransform.rotation = target.position.x > myTransform.position.x ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    /* private void SeparateFromOtherEnemies()
    {
        enemiesNearby = Physics2D.OverlapCircleAll(myTransform.position, separationRadius);

        foreach (Collider2D other in enemiesNearby)
        {
            if (other != null && other.gameObject != gameObject && other.CompareTag("Enemy"))
            {
                repelDirection = myTransform.position - other.transform.position;
                distance = repelDirection.magnitude;

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
