using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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

    

    void Start()
    {
        
        // Trouve le joueur par son tag au démarrage
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        
        
    }

    void FixedUpdate()
    {
        
        DetectPlayer();
        
    }
    void Update()
    {
        
    }

    private void DetectPlayer()
    {
        MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
        AttackHitBoxSide attackHitBoxSide = GetComponentInChildren<AttackHitBoxSide>();
        if (playerTransform == null)
        {
            return;
        }
        if(!monsterHealth.isTakingDamage  && attackHitBoxSide.isAttacking == false)
        {
            
            // Vérifie la distance entre l'ennemi et le joueur
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);        
            if (distanceToPlayer <= detectionRange)
            {
                // Déplace l'ennemi vers le joueur uniquement sur l'axe X
                float newXPosition = Mathf.MoveTowards(transform.position.x, playerTransform.position.x, speed * Time.deltaTime);
                transform.position = new Vector2(newXPosition, transform.position.y);
                animator.SetFloat("Speed",Mathf.Abs(newXPosition));
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
}
