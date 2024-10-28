
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

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar = GetComponentInChildren<MonsterHealthBar>();
        healthBar.UpdateHealthBar(health, maxHealth);
        
        impulseSource = GetComponentInChildren<CinemachineImpulseSource>();
        isTakingDamage = false;
        
        knockBackCounter = 0;
        enemyRb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerLevel = player.GetComponent<PlayerLevel>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreLayerCollision(11,14,true);
        knockBackTest();
        direction = enemyRb.transform.rotation.y == 0 ? 1 : -1;
        moreLevelMoreHealth();
        
    }

    public void moreLevelMoreHealth()
    {
        if(playerLevel.isLevelingUp && !damageIncreased)
        {
            maxHealth += maxHealth/percentage;
            // health = maxHealth;
            healthBar.UpdateHealthBar(health,maxHealth);
            // health = Mathf.Clamp(health, 0, maxHealth); 
            damageIncreased = true;
        }else if (!playerLevel.isLevelingUp)
        {
            damageIncreased = false;
        }
    }

    public void TakeDamage(float damage)
    {
        knockBackCounter = 0;
        isTakingDamage = true;
         
        knockBackTest();
        //CameraShakeManager.instance.CameraShake(impulseSource);

        SpawnDamageParticles();

        //ContactDamage();
        //StartCoroutine(FlashCoroutine());

        health -= damage;
        healthBar.UpdateHealthBar(health,maxHealth);
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
        {
            Instantiate(XpForPlayer, transform.position, Quaternion.identity);
            // Destroy(gameObject);
            // envoyer l'ennemi au pool une fois vaincu
            // EnemyPoolManager.Instance.ReturnEnemyToPool(gameObject, enemyPrefab);
            // gameObject.SetActive(false);
            EnemyPoolManager.Instance.ReturnEnemyToPool(gameObject);
            AttackHitBoxSide attackHitBoxSide = attackHitBox.GetComponent<AttackHitBoxSide>();
            AttackHitBoxKamikaze attackHitBoxKamikaze = attackHitBox.GetComponent<AttackHitBoxKamikaze>();
            if (attackHitBoxSide != null)
            {
                attackHitBoxSide.isAttacking = false;
            }
            if (attackHitBoxKamikaze != null)
            {
                attackHitBoxKamikaze.isAttacking = false;
            }
        }
        
    }

    public void TakeDamage2(float damage)
    {
        knockBackCounter = 0;
        // isTakingDamage = true;
         
        // knockBackTest();
        //CameraShakeManager.instance.CameraShake(impulseSource);

        SpawnDamageParticles();

        //ContactDamage();
        //StartCoroutine(FlashCoroutine());

        health -= damage;
        healthBar.UpdateHealthBar(health,maxHealth);
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
        {
            Instantiate(XpForPlayer, transform.position, Quaternion.identity);
            // Destroy(gameObject);
            // gameObject.SetActive(false);
            EnemyPoolManager.Instance.ReturnEnemyToPool(gameObject);
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
            if(knockBackCounter >= knockBackDuration)
            {
                enemyRb.velocity = Vector3.zero;
                isTakingDamage = false;
                knockBackCounter = 0;
                


                Vector2 dashDirection = (playerRb.transform.position - transform.position).normalized;
                float angleInRadians = Mathf.Atan2(dashDirection.y, dashDirection.x);
                float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
                rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f + angleInDegrees + 180 : 180f + angleInDegrees);
                Instantiate(dashExplosionParticles,transform.position, rotation);
                enemyRb.AddForce(dashDirection * dashDistance, ForceMode2D.Impulse);
                
            }
        }
    }

    private void SpawnDamageParticles()
    {
        int direction = playerRb.transform.rotation.y == 0 ? 1 : -1;
        Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 180f : 0f);  
        damageParticlesInstance = Instantiate(damageParticles,transform.position, rotation);
        Instantiate(impactParticles,transform.position, rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Vérifie si la collision est suffisamment forte pour causer des dégâts supplémentaires
            MonsterHealth otherEnemy = collision.gameObject.GetComponent<MonsterHealth>();
            if (enemyRb != null && enemyRb.velocity.magnitude > 20f && otherEnemy != null)
            {               
                float impactForce = collision.relativeVelocity.magnitude;
                float additionalDamage = impactForce * additionalDamageMultiplier;
                otherEnemy.TakeDamage(normalDamage + additionalDamage);    
                StartCoroutine(DestroyComicBoomEffect());                           
            }
        } else if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Roofs"))
        {
            if(enemyRb.velocity.magnitude > 20f)
            {
                float impactForce = collision.relativeVelocity.magnitude;
                float additionalDamage = impactForce * additionalDamageMultiplier;
                TakeDamage(normalDamage + additionalDamage);
                StartCoroutine(DestroyComicBoomEffect());

            }
        }
    }

    public IEnumerator DestroyComicBoomEffect()
    {
        GameObject comicBoom = Instantiate(ComicBoomEffect,new Vector2(transform.position.x,transform.position.y + 2f), Quaternion.identity);
        Light light = comicBoom.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(comicBoom);
        Destroy(light);
        
        yield return null;
    }
    public void ContactDamage()
    {

        if(enemyRb.velocity.magnitude > 30f && !IsGrounded())
        {
            StartCoroutine(DamageGrounded());
        }
    }

    public IEnumerator DamageGrounded()
    {
        float impactForce = enemyRb.velocity.magnitude;
        float additionalDamage =  impactForce * additionalDamageMultiplier;
        yield return new WaitUntil(() => IsGrounded());
        TakeDamage(normalDamage + additionalDamage);
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
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayerMask);
         // Dessiner le raycast dans la vue de scène pour le débogage
        Debug.DrawRay(transform.position, Vector2.down * rayDistance, Color.red);
        return hit.collider != null;
    }

}
