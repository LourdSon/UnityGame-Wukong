using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEditor.Experimental.GraphView;
using Cinemachine;

//using System.Numerics;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float forceMagnitudeForward = 10f; // Magnitude de la force à appliquer pour la projection vers l'avant
    public float selfForceMagnitudeForward = 10f;
    public float forceMagnitudeUpward = 10f; // Magnitude de la force à appliquer pour la projection vers le haut
    public float forceMagnitudeDownward = 50f;
    public float detectionOffset = 1f; // Décalage du rayon de détection par rapport au joueur
    public Vector2 detectionOffsetAir = new Vector2(1f, -1f);
    public float detectionRadius = 2f; // Rayon de détection des ennemis
    public float detectionRadiusTP = 20f;
    public float offsetTP = 2f;
    public KeyCode forwardAttackKey = KeyCode.F; // Touche pour attaque vers l'avant
    public KeyCode TeleportOnEnemy = KeyCode.T;
    public LayerMask enemyLayerMask; // Masque de layer pour les ennemis

    public SpriteRenderer spriteRenderer; // Référence au composant SpriteRenderer du joueur
    private bool isHoldingUpwardKey = false; // Indique si la touche pour l'attaque vers le haut est maintenue enfoncée
    private bool isHoldingDownwardKey = false;


    private Animator animator;
    [Header ("Attacks timer")]

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
    public Vector2 detectionRadiusSlam = new Vector2(15f,3f);

    public LayerMask groundLayerMask;
    public TileDestroyer tileDestroyer;
    private CinemachineImpulseSource impulseSource;
    public float rayDistance = 5f;
    public float doubleChocTimer = 0.2f;

    

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
    }

    void Update()
    {
        Attackp();
        
    }


    private void Attackp()
    {
        float upwardAttackKey = Input.GetAxisRaw("Vertical");

        int upwardAttackKeyInt = Mathf.RoundToInt(upwardAttackKey);

        // Vérifier si la touche pour l'attaque vers le haut est maintenue enfoncée
        if (upwardAttackKey == 1)
        {
            isHoldingUpwardKey = true;
            isHoldingDownwardKey = false;
        }
        else if(upwardAttackKey == -1)
        {
            isHoldingUpwardKey = false;
            isHoldingDownwardKey = true;
        }

        
        if (Input.GetKeyDown(forwardAttackKey) && isHoldingUpwardKey && attackTimeCounterUpward <= 0f)
        {
            // Obtenir la direction actuelle du sprite du joueur
            int direction = spriteRenderer.flipX ? -1 : 1;

            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterUpward = timeBtwAttacksUpward;

            
            Vector2 detectionPosition = (Vector2)transform.position + Vector2.right * direction * detectionOffset;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, enemyLayerMask);

            // Appliquer une force pour projeter les ennemis vers le haut
            foreach (Collider2D collider in colliders)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemyRb.AddForce(Vector2.up * forceMagnitudeUpward, ForceMode2D.Impulse);
                    playerRb.AddForce(Vector2.up * forceMagnitudeUpward, ForceMode2D.Impulse);
                    

                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                    monsterHealth.TakeDamage(damage);
                    
                    
                    //Debug.Log("enemy velocity :" + enemyRb.velocity);
                    Debug.Log("enemy velocity :" + enemyRb.velocity.magnitude);
                }
            }
        }

        else if (Input.GetKeyDown(forwardAttackKey) && attackTimeCounter <= 0f)
        {
            // Obtenir la direction actuelle du sprite du joueur
            int direction = spriteRenderer.flipX ? -1 : 1;

            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounter = timeBtwAttacks;

            Vector2 detectionPosition = (Vector2)transform.position + Vector2.right * direction * detectionOffset;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, enemyLayerMask);

            // Appliquer une force pour projeter les ennemis vers l'avant
            foreach (Collider2D collider in colliders)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                    enemyRb.AddForce(directionVector * forceMagnitudeForward, ForceMode2D.Impulse);
                    playerRb.AddForce(Vector2.right * -selfForceMagnitudeForward, ForceMode2D.Impulse);

                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                    monsterHealth.TakeDamage(damage);

                    //Debug.Log("enemy velocity :" + enemyRb.velocity);
                    Debug.Log("enemy velocity :" + enemyRb.velocity.magnitude);
                }
            }
        }

        else if(Input.GetKeyDown(forwardAttackKey) && isHoldingDownwardKey  && attackTimeCounterDownward <= 0f)
        {
            int direction = spriteRenderer.flipX ? -1 : 1;
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterDownward = timeBtwAttacksDownward;
            Vector2 detectionPosition = (Vector2)transform.position + Vector2.down * detectionOffsetAir.y + Vector2.right * direction * detectionOffsetAir.x;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius,enemyLayerMask);
            if(colliders.Length >= 1)
            {
                playerRb.AddForce(Vector2.up * selfForceMagnitudeForward*2.5f, ForceMode2D.Impulse);
            }
            foreach(Collider2D collider in colliders)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if(enemyRb != null)
                {
                    enemyRb.AddForce(Vector2.down * forceMagnitudeDownward, ForceMode2D.Impulse);

                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();

                    monsterHealth.TakeDamage(damage);
                    monsterHealth.ContactDamage();

                    //Debug.Log("enemy velocity :" + enemyRb.velocity);
                    Debug.Log("enemy velocity :" + enemyRb.velocity.magnitude);
                }

            }
        }

        else if(Input.GetKeyDown(KeyCode.C) && attackTimeCounterSlam <= 0f)
        {
            //animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterSlam = timeBtwAttacksSlam;
            
            StartCoroutine(WaitForLanding());
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
        CameraShakeManager.instance.CameraShake(impulseSource);        
        // Détecter les ennemis dans la zone d'attaque
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, detectionRadiusSlam, 0f, enemyLayerMask);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                enemyRb.AddForce(Vector2.right * directionVector * forceMagnitudeForward,ForceMode2D.Impulse);
                // Infliger des dégâts aux ennemis
                MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                if (monsterHealth != null)
                {
                    monsterHealth.TakeDamage(damage);
                }
            }
            
        }
        //tileDestroyer.OGDestructionMouse();
        yield return null;
        
    }

    private float TimerDecrement(float timeCounter)
    {
        if (timeCounter >= 0f)
            timeCounter -= Time.deltaTime;

        return timeCounter;
    }








    // Afficher le rayon de détection dans l'éditeur Unity
    private void OnDrawGizmosSelected()
    {
        // Dessiner le rayon de détection dans l'éditeur
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, detectionRadiusSlam);





    }

    public bool IsGrounded()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayerMask);
         // Dessiner le raycast dans la vue de scène pour le débogage
        Debug.DrawRay(transform.position, Vector2.down * rayDistance, Color.red);
        return hit.collider != null;
    }
}