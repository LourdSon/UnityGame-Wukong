#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Callbacks;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{

    public float maxHealth = 50;
    public float health;

    public MonsterHealthBar healthBar;
    private CinemachineImpulseSource impulseSource;

    public ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;
    public bool isTakingDamage;
    public float knockBackDuration = 0.5f;
    public float knockBackCounter;

    private Rigidbody2D enemyRb;
    public float additionalDamageMultiplier = 1f;

    public float rayDistance = 5f;
    public LayerMask groundLayerMask;
    

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar = GetComponentInChildren<MonsterHealthBar>();
        healthBar.UpdateHealthBar(health, maxHealth);
        
        impulseSource = GetComponent<CinemachineImpulseSource>();
        isTakingDamage = false;
        
        knockBackCounter = 0;
        enemyRb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        knockBackTest();
        
    }

    public void TakeDamage(float damage)
    {
        knockBackCounter = 0;
        isTakingDamage = true;
        knockBackTest();
        CameraShakeManager.instance.CameraShake(impulseSource);

        SpawnDamageParticles();

        //ContactDamage();

        health -= damage;
        healthBar.UpdateHealthBar(health,maxHealth);
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    public void knockBackTest()
    {
        if (isTakingDamage)
        {
            knockBackCounter += Time.deltaTime;
            if(knockBackCounter >= knockBackDuration)
            {
                isTakingDamage = false;
                knockBackCounter = 0;
            }
        }
    }

    private void SpawnDamageParticles()
    {
        GameObject player = GameObject.FindWithTag("Player");
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        int direction = playerSpriteRenderer.flipX ? 1 : -1;
        Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 180f : 0f);  
        damageParticlesInstance = Instantiate(damageParticles,transform.position, rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        // Log pour débogage
        //Debug.Log($"{gameObject.name} collided with {collision.gameObject.name}");


        if (collision.gameObject.CompareTag("Enemy"))
        {
             // Log pour débogage
            //Debug.Log($"{gameObject.name} collided with Ground or Enemy");

            // Vérifie si la collision est suffisamment forte pour causer des dégâts supplémentaires
            MonsterHealth otherEnemy = collision.gameObject.GetComponent<MonsterHealth>();
            if (enemyRb != null && enemyRb.velocity.magnitude > 15f && otherEnemy != null)
            {
                
                float impactForce = collision.relativeVelocity.magnitude;
                float additionalDamage = impactForce * additionalDamageMultiplier;
                // Log pour débogage
                //Debug.Log($"{gameObject.name} impact force: {impactForce}");
                //Debug.Log(additionalDamage);
                // Applique les dégâts supplémentaires
                otherEnemy.TakeDamage(300f + additionalDamage);
                

                
            }

        }
    }

    public void ContactDamage()
    {

        if(enemyRb.velocity.magnitude > 15f && !IsGrounded())
        {
            StartCoroutine(DamageGrounded());
        }
    }

    public IEnumerator DamageGrounded()
    {
        float impactForce = enemyRb.velocity.magnitude;
        float additionalDamage =  impactForce * additionalDamageMultiplier;
        yield return new WaitUntil(() => IsGrounded());
        TakeDamage(300f + additionalDamage);
        yield return null;
    }

    public bool IsGrounded()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayerMask);
         // Dessiner le raycast dans la vue de scène pour le débogage
        Debug.DrawRay(transform.position, Vector2.down * rayDistance, Color.red);
        return hit.collider != null;
    }
}

#endif