using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{

    public float detectionRange = 50f; // Portée de détection de l'ennemi
    public float speed = 5f; // Vitesse de déplacement de l'ennemi

    private Transform playerTransform;
        
    private Rigidbody2D enemyRb;
    

    void Start()
    {
        
        // Trouve le joueur par son tag au démarrage
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        enemyRb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        
        DetectPlayer();
        
    }

    private void DetectPlayer()
    {
        MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
        if (playerTransform == null)
        {
            return;
        }
        if(!monsterHealth.isTakingDamage)
        {
            
            // Vérifie la distance entre l'ennemi et le joueur
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);        
            if (distanceToPlayer <= detectionRange)
            {
                // Déplace l'ennemi vers le joueur uniquement sur l'axe X
                float newXPosition = Mathf.MoveTowards(transform.position.x, playerTransform.position.x, speed * Time.deltaTime);
                transform.position = new Vector2(newXPosition, transform.position.y);
            }
        }
        
    }
}
