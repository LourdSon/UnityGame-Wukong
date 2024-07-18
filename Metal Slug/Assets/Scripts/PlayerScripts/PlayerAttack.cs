using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEditor.Experimental.GraphView;
using Cinemachine;

//using System.Numerics;
using UnityEngine;
using Unity.Mathematics;


public class PlayerAttack : MonoBehaviour
{
    public float forceMagnitudeForward = 10f; // Magnitude de la force à appliquer pour la projection vers l'avant
    public float selfForceMagnitudeForward = 10f;
    public float forceMagnitudeUpward = 10f; // Magnitude de la force à appliquer pour la projection vers le haut
    public float forceMagnitudeDownward = 50f;
    public float detectionOffset = 1f; // Décalage du rayon de détection par rapport au joueur
    public Vector2 detectionOffsetAir = new Vector2(1f, 1.5f);
    public float detectionRadius = 2f; // Rayon de détection des ennemis
    public float detectionRadiusTP = 20f;
    public float offsetTP = 2f;
    public KeyCode forwardAttackKey = KeyCode.F; // Touche pour attaque vers l'avant
    public KeyCode TeleportOnEnemy = KeyCode.T;
    public LayerMask enemyLayerMask; // Masque de layer pour les ennemis

    public SpriteRenderer spriteRenderer; // Référence au composant SpriteRenderer du joueur


    private Animator animator;

    [Header("Attacks timer")] 
    public float timeBtwAttacks = 2f;
    public float attackTimeCounter;

    public float timeBtwAttacksUpward = 2f;
    public float attackTimeCounterUpward;

    public float timeBtwAttacksDownward = 2f;
    public float attackTimeCounterDownward;

    public float timeBtwAttacksSlam = 2f;
    public float attackTimeCounterSlam;



    public int damage = 10;
    private Rigidbody2D playerRb;

    public float slamForce = 10f;
    public Vector2 detectionRadiusSlam = new Vector2(15f, 3f);

    public LayerMask groundLayerMask;
    public TileDestroyer tileDestroyer;
    private CinemachineImpulseSource impulseSource;
    public float rayDistance = 5f;
    public float doubleChocTimer = 0.2f;


    private Vector2 detectionPosition;
    private Vector2 detectionPositionDown;

    public bool attack1;
    public bool attack2;
    public bool attack3;
    public bool attack4;

    public ParticleSystem slamParticles;
    public Quaternion rotation;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Récupérer le composant SpriteRenderer
        animator = GetComponent<Animator>();
        attackTimeCounter = 0f;
        attackTimeCounterUpward = 0f;
        attackTimeCounterDownward = 0f;
        attackTimeCounterSlam = 0f;

        attackTimeCounterSlam = timeBtwAttacksSlam;
        playerRb = GetComponent<Rigidbody2D>();
        tileDestroyer = GetComponentInChildren<TileDestroyer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        

        attack1 = false;
        attack2 = false;
        attack3 = false;
        attack4 = false;

    }

    void Update()
    {
        Attackp();
    }

    void FixedUpdate()
    {
        Attack1234();
    }

    private void Attackp()
    {
        float upwardAttackKey = Input.GetAxisRaw("Vertical");
        int direction = spriteRenderer.flipX ? -1 : 1;
        detectionPosition = (Vector2)transform.position + Vector2.right * direction * detectionOffset; 
        detectionPositionDown = (Vector2)transform.position + Vector2.down * detectionOffsetAir.y + Vector2.right * direction * detectionOffsetAir.x;
        
        rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
        

        if (Input.GetButtonDown("Fire1") && upwardAttackKey == 1 && attackTimeCounterUpward <= 0f)
        {
            // Obtenir la direction actuelle du sprite du joueur
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterUpward = timeBtwAttacksUpward;
            attack1 = true;
            
        } else if (Input.GetButtonDown("Fire1")  && upwardAttackKey == 0 && attackTimeCounter <= 0f)
        {
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounter = timeBtwAttacks;
            attack2 = true;
        } else if (Input.GetButtonDown("Fire1") && upwardAttackKey == -1 && attackTimeCounterDownward <= 0f)
        {
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterDownward = timeBtwAttacksDownward;
            attack3 = true;  
        } else if (Input.GetButtonDown("Fire2")  && attackTimeCounterSlam <= 0f)
        {
            //animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterSlam = timeBtwAttacksSlam;
            attack4 = true;  
        }


        attackTimeCounter = TimerDecrement(attackTimeCounter);
        attackTimeCounterSlam = TimerDecrement(attackTimeCounterSlam);
        attackTimeCounterDownward = TimerDecrement(attackTimeCounterDownward);
        attackTimeCounterUpward = TimerDecrement(attackTimeCounterUpward);
    }



    private IEnumerator WaitForLanding()
    {
        BoxCollider2D playerBox = GetComponent<BoxCollider2D>();
        // Appliquer une force descendante aux ennemis
        playerRb.AddForce(Vector2.down * slamForce, ForceMode2D.Impulse);
        CameraShakeManager.instance.CameraShake(impulseSource);
        playerBox.enabled = false;
        yield return new WaitUntil(() => IsGrounded());
        playerBox.enabled = true;
        yield return new WaitForSeconds(doubleChocTimer);
        Instantiate(slamParticles,transform.position,rotation);
        CameraShakeManager.instance.CameraShake(impulseSource);
        // Détecter les ennemis dans la zone d'attaque
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, detectionRadiusSlam, 0f, enemyLayerMask);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                enemyRb.AddForce(Vector2.right * directionVector * forceMagnitudeForward, ForceMode2D.Impulse);
                // Infliger des dégâts aux ennemis
                MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                if (monsterHealth != null)
                {
                    monsterHealth.TakeDamage(damage);
                }
            }
        }
        attack4 = false;
        //tileDestroyer.OGDestructionMouse();
        yield return null;

    }

    private float TimerDecrement(float timeCounter)
    {
        if (timeCounter >= 0f)
            timeCounter -= Time.deltaTime;

        return timeCounter;
    }



    public bool IsGrounded()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayerMask);
        // Dessiner le raycast dans la vue de scène pour le débogage
        Debug.DrawRay(transform.position, Vector2.down * rayDistance, Color.red);
        return hit.collider != null;
    }

    
    private void Attack1234()
    {
        if (attack1 == true)
        {
            StartCoroutine(Attack1Co());
            attack1 = false;
        }
        else if (attack2 == true)
        {
            StartCoroutine(Attack2Co());
            attack2 = false;
        }
        else if (attack3 == true)
        {
            StartCoroutine(Attack3Co());
            attack3 = false;
        }
        else if (attack4 == true)
        {
            StartCoroutine(WaitForLanding());
            attack4 = false;
        }
    }

    private IEnumerator Attack1Co()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, enemyLayerMask);
        if (colliders.Length >= 1)
        {
            playerRb.AddForce(Vector2.up * forceMagnitudeUpward, ForceMode2D.Impulse);
        }
        // Appliquer une force pour projeter les ennemis vers le haut
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(Vector2.up * forceMagnitudeUpward, ForceMode2D.Impulse);
                MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                monsterHealth.TakeDamage(damage);
            }
        }
        attack1 = false;
        yield return null;
    }

    private IEnumerator Attack2Co()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, enemyLayerMask);
        if (colliders.Length >= 1)
            {
                int direction = spriteRenderer.flipX ? -1 : 1;
                //playerRb.velocity = new Vector2(diagonal.x * selfForceMagnitudeForward,playerRb.velocity.y);
                playerRb.AddForce(Vector2.right * selfForceMagnitudeForward * -direction, ForceMode2D.Impulse);
            }
            // Appliquer une force pour projeter les ennemis vers l'avant
            foreach (Collider2D collider in colliders)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                    enemyRb.AddForce(directionVector * forceMagnitudeForward, ForceMode2D.Impulse);
                    //playerRb.AddForce(Vector2.right * -selfForceMagnitudeForward, ForceMode2D.Impulse);

                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                    monsterHealth.TakeDamage(damage);
                }
            }
            attack2 = false;
            yield return null;
    }

    private IEnumerator Attack3Co()
    {
        Collider2D[] collidersDown = Physics2D.OverlapCircleAll(detectionPositionDown, detectionRadius, enemyLayerMask);
        if (collidersDown.Length >= 1)
            {
                playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
            }
            foreach (Collider2D collider in collidersDown)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemyRb.AddForce(Vector2.down * forceMagnitudeDownward, ForceMode2D.Impulse);

                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();

                    monsterHealth.TakeDamage(damage);
                    //monsterHealth.ContactDamage();
                }

            }
            attack3 = false;
            yield return null;
    }

    // Afficher le rayon de détection dans l'éditeur Unity
    private void OnDrawGizmosSelected()
    {
        // Dessiner le rayon de détection dans l'éditeur
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectionPositionDown, detectionRadius);
    }
}