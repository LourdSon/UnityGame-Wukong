
using System;
using System.Collections;

using Cinemachine;

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

    public float knockBackCounter;
    public float knockBackDuration;

    private Rigidbody2D enemyRb;
    public float additionalDamageMultiplier = 0.1f;
    public float normalDamage = 10f;

    public float rayDistance = 5f;
    public LayerMask groundLayerMask;
    public ParticleSystem impactParticles;

    public Light impactLight;
    public float flashDuration = 0.1f;
    private GameObject player;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSpriteRenderer;
    public float dashDistance = 50f;
    public ParticleSystem dashExplosionParticles;
    public Quaternion rotation;
    public int direction;
    private SpriteRenderer spriteRenderer;
    public GameObject ComicBoomEffect;
    public GameObject XpForPlayer;
    public PlayerLevel playerLevel;
    private bool damageIncreased;
    public float percentage = 20;
    public GameObject enemyPrefab;
    public GameObject attackHitBox;

    private AttackHitBoxSide attackHitBoxSide;
    private AttackHitBoxKamikaze attackHitBoxKamikaze;
    private Vector2 dashDirection;
    private float angleInRadians;
    private float angleInDegrees;
    private MonsterHealth otherEnemy;
    private float impactForce;
    private float additionalDamage;
    private GameObject comicBoom;
    private Light lighter;
    private RaycastHit2D hit;
    private Transform myTransform;
    public GameObject shield;
    public GameObject shieldSprite;
    public float maxShield = 500;
    public float currentShield;
    public int levelThreshold;
    public GameObject guardBreakEffect;
    private GameObject guardBreak;
    public Timer timer;
    public int bonusSeconds = 5;
    public GameObject circleArea;
    public PlayerScore playerScore;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        
        healthBar = GetComponentInChildren<MonsterHealthBar>();
        healthBar.UpdateHealthBar(health, maxHealth);
        if(shield != null)
        {
            shield.SetActive(false);
            shieldSprite.SetActive(false);
            currentShield = maxShield;
            healthBar.UpdateShieldBar(currentShield, maxShield);
        }

        impulseSource = GetComponentInChildren<CinemachineImpulseSource>();
        isTakingDamage = false;
        knockBackCounter = 0;
        enemyRb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerLevel = player.GetComponent<PlayerLevel>();
        myTransform = transform;
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreLayerCollision(11,14,true);
        knockBackTest();
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        moreLevelMoreHealth();
        
    }

    public void moreLevelMoreHealth()
    {
        if(playerLevel.isLevelingUp && !damageIncreased)
        {
            maxHealth *= percentage;
            // health = maxHealth;
            healthBar.UpdateHealthBar(health,maxHealth);
            // health = Mathf.Clamp(health, 0, maxHealth); 
            damageIncreased = true;
        }else if (!playerLevel.isLevelingUp)
        {
            damageIncreased = false;
        }

        if(playerLevel.currentLvl >= levelThreshold)
        {
            if(shield != null && currentShield > 0)
            {
                shield.SetActive(true);
                shieldSprite.SetActive(true);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        knockBackCounter = 0;
        isTakingDamage = true;
        
        SpawnDamageParticles();
        if(shield != null && !shield.activeSelf)
        {
            knockBackTest();
            health -= damage;
            healthBar.UpdateHealthBar(health,maxHealth);
            health = Mathf.Clamp(health, 0, maxHealth);
        } else if(shield.activeSelf)
        {
            currentShield -= damage;
            healthBar.UpdateShieldBar(currentShield, maxShield);
            currentShield = Mathf.Clamp(currentShield, 0, maxShield);
        }
        if(currentShield <= 0 && shield.activeSelf)
        {
            StartCoroutine(DestroyGuardBreakEffect());
            shield.SetActive(false);
            shieldSprite.SetActive(false);
        }
        if (health <= 0)
        {
            playerScore.enemiesKilled += 1;
            Instantiate(XpForPlayer, myTransform.position, Quaternion.identity);
            if(gameObject.activeSelf)
            StartCoroutine(timer.bonusTimer(bonusSeconds));
            EnemyPoolManager.Instance.ReturnEnemyToPool(gameObject);
            attackHitBoxSide = attackHitBox.GetComponent<AttackHitBoxSide>();
            attackHitBoxKamikaze = attackHitBox.GetComponent<AttackHitBoxKamikaze>();
            if (attackHitBoxSide != null)
            {
                attackHitBoxSide.isAttacking = false;
            }
            if (attackHitBoxKamikaze != null && circleArea != null)
            {
                circleArea.SetActive(false);
                attackHitBoxKamikaze.isAttacking = false;
            }
        }
        
    }

    public void TakeDamage2(float damage)
    {
        knockBackCounter = 0;
        
        
        SpawnDamageParticles();
        if(shield != null && !shield.activeSelf)
        {
            health -= damage;
            healthBar.UpdateHealthBar(health,maxHealth);
            health = Mathf.Clamp(health, 0, maxHealth);
        } else if(shield.activeSelf)
        {
            currentShield -= damage;
            healthBar.UpdateShieldBar(currentShield, maxShield);
            currentShield = Mathf.Clamp(currentShield, 0, maxShield);
        }
        if(currentShield <= 0 && shield.activeSelf)
        {
            StartCoroutine(DestroyGuardBreakEffect());
            shield.SetActive(false);
            shieldSprite.SetActive(false);
        }
        if (health <= 0)
        {
            Instantiate(XpForPlayer, myTransform.position, Quaternion.identity);
            EnemyPoolManager.Instance.ReturnEnemyToPool(gameObject);
            attackHitBoxSide = attackHitBox.GetComponent<AttackHitBoxSide>();
            attackHitBoxKamikaze = attackHitBox.GetComponent<AttackHitBoxKamikaze>();
            if (attackHitBoxSide != null)
            {
                attackHitBoxSide.isAttacking = false;
            }
            if (attackHitBoxKamikaze != null && circleArea != null)
            {
                circleArea.SetActive(false);
                attackHitBoxKamikaze.isAttacking = false;
            }
        }
        
    }
   


    public void knockBackTest()
    {
        if (isTakingDamage)
        {
            knockBackCounter += Time.deltaTime;
            if(knockBackCounter < knockBackDuration)
            {
                Physics2D.IgnoreLayerCollision(11,14,false);
            }
            if(knockBackCounter >= knockBackDuration && playerRb != null)
            {
                enemyRb.velocity = Vector3.zero;
                isTakingDamage = false;
                knockBackCounter = 0;
                dashDirection = (playerRb.transform.position - myTransform.position).normalized;
                angleInRadians = Mathf.Atan2(dashDirection.y, dashDirection.x);
                angleInDegrees = angleInRadians * Mathf.Rad2Deg;
                rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f + angleInDegrees + 180 : 180f + angleInDegrees);
                Instantiate(dashExplosionParticles, myTransform.position, rotation);
                enemyRb.AddForce(dashDirection * dashDistance, ForceMode2D.Impulse);
            }
        }
    }

    private void SpawnDamageParticles()
    {
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 180f : 0f);  
        damageParticlesInstance = Instantiate(damageParticles, myTransform.position, rotation);
        Instantiate(impactParticles, myTransform.position, rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyRb.velocity.magnitude > 15f)
        {
            ContactDamage(collision);
        }
    }

    public IEnumerator DestroyComicBoomEffect()
    {
        comicBoom = Instantiate(ComicBoomEffect,new Vector2(myTransform.position.x, myTransform.position.y + 2f), Quaternion.identity);
        lighter = comicBoom.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(comicBoom);
        Destroy(lighter);
        
        yield return null;
    }

    public IEnumerator DestroyGuardBreakEffect()
    {
        guardBreak = Instantiate(guardBreakEffect,new Vector2(myTransform.position.x, myTransform.position.y + 2f), Quaternion.identity);
        lighter = guardBreak.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(guardBreak);
        Destroy(lighter);
        
        yield return null;
    }

    public void ContactDamage(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Roofs"))
        {
            if (enemyRb.velocity.magnitude > 15f && !IsGrounded())
            {
                StartCoroutine(HandleCollisionDamage(collision));
            }
        }
    }

    private IEnumerator HandleCollisionDamage(Collision2D collision)
    {
        impactForce = enemyRb.velocity.magnitude;
        additionalDamage = impactForce * additionalDamageMultiplier;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            MonsterHealth otherEnemy = collision.gameObject.GetComponent<MonsterHealth>();
            if (otherEnemy != null)
            {
                otherEnemy.TakeDamage(normalDamage + additionalDamage);
                Instantiate(ComicBoomEffect, new Vector2(transform.position.x, transform.position.y + 2f), Quaternion.identity);
            }
        }
        else if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Roofs"))
        {
            yield return new WaitUntil(() => IsGrounded());
            TakeDamage(normalDamage + additionalDamage);
            Instantiate(ComicBoomEffect, new Vector2(transform.position.x, transform.position.y + 2f), Quaternion.identity);
        }

        yield return null;
    }


    public IEnumerator FlashCoroutine()
    {
        impactLight.enabled = true;  // Activer la lumière
        yield return new WaitForSeconds(flashDuration);  // Attendre pendant la durée du flash
        impactLight.enabled = false;  // Désactiver la lumière
    }

    public bool IsGrounded()
    {
        
        hit = Physics2D.Raycast(myTransform.position, Vector2.down, rayDistance, groundLayerMask);
         // Dessiner le raycast dans la vue de scène pour le débogage
        Debug.DrawRay(myTransform.position, Vector2.down * rayDistance, Color.red);
        return hit.collider != null;
    }

}
