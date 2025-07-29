using System.Collections;
using System.Security.Cryptography;
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
    public GameObject damageGruntPrefab;
    public Vector3 monsterScale;
    public float percentageScale;


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
        monsterScale = transform.localScale;
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreLayerCollision(11,14,true);
        Physics2D.IgnoreLayerCollision(12,23,true);
        knockBackTest();
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        moreLevelMoreHealth();
        
    }

    public void moreLevelMoreHealth()
    {
        if(playerLevel.isLevelingUp && !damageIncreased)
        {
            maxHealth *= percentage;
            monsterScale *= percentageScale;
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
        // SpawnDamageParticles();
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
            if(gameObject.activeSelf)
            {
                StartCoroutine(timer.bonusTimer(bonusSeconds));
                StartCoroutine(DeadBodyFall(gameObject));
            }
            playerScore.enemiesKilled += 1;
            Instantiate(XpForPlayer, myTransform.position, Quaternion.identity);
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
        if (gameObject.activeInHierarchy) // Vérifie si le GameObject est actif
        {
            StartCoroutine(damageGruntAudio());
        }
        
    }

    public void TakeDamage2(float damage)
    {
        knockBackCounter = 0;
        
        // SpawnDamageParticles();
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
            if(gameObject.activeSelf)
            {
                StartCoroutine(timer.bonusTimer(bonusSeconds));
                StartCoroutine(DeadBodyFall(gameObject));
            }
            playerScore.enemiesKilled += 1;
            Instantiate(XpForPlayer, myTransform.position, Quaternion.identity);
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
        if (gameObject.activeInHierarchy) // Vérifie si le GameObject est actif
        {
            StartCoroutine(damageGruntAudio());
        }
    }
   public IEnumerator DeadBodyFall(GameObject enemy)
    {
        if(!enemy.activeSelf)
        {
            yield break; // Si le GameObject n'est pas actif, ne rien faire
        }
        else
        {
            Vector2 enemyVelocity = enemy.GetComponent<Rigidbody2D>().velocity;
            GameObject clone = Instantiate(enemy, enemy.transform.position, enemy.transform.rotation);
            EnemyPoolManager.Instance.ReturnEnemyToPool(enemy);
            clone.tag = "DeadBody"; // Changer le tag du corps
            if (clone != null)
            {
                Animator animator = clone.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = false; // Désactiver l'Animator
                }
                MonoBehaviour[] components = clone.GetComponents<MonoBehaviour>();            
                foreach (MonoBehaviour component in components)
                {
                    component.enabled = false; // Désactiver tous les MonoBehaviours
                }

                Rigidbody2D enemyRb = clone.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemyRb.velocity = enemyVelocity; // Appliquer la vitesse du corps
                    enemyRb.constraints = RigidbodyConstraints2D.None; // Enlève les contraintes
                    enemyRb.gravityScale = 1f; // Applique la gravité
                    // enemyRb.velocity = new Vector2(0, -5f); // Applique une force vers le bas
                }
                Canvas canvas = clone.GetComponentInChildren<Canvas>();
                if (canvas != null)
                {
                    canvas.gameObject.SetActive(false); // Désactiver le Canvas
                }
                SpriteRenderer spriteRenderer = clone.GetComponentInParent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.black; // Changer la couleur du sprite
                }
            
                Transform[] allChildren = clone.GetComponentsInChildren<Transform>(true);
                foreach (Transform child in allChildren)
                {
                    if (child.gameObject != clone)
                    {
                        // child.tag = "DeadBody"; // Changer le tag de tous les enfants
                        // child.layer = LayerMask.NameToLayer("DeadBody"); // Changer le layer de tous les enfants
                        // child.SetActive(false); // Désactiver tous les enfants
                        Destroy(child.gameObject); // Détruire tous les
                    }
                }

                clone.tag = "DeadBody";
                clone.layer = LayerMask.NameToLayer("DeadBody"); // Changer le layer du corps
                clone.transform.rotation = Quaternion.Euler(clone.transform.rotation.eulerAngles.x, clone.transform.rotation.eulerAngles.y, clone.transform.rotation.eulerAngles.z + 90f); // Réinitialiser la rotation
                yield return new WaitUntil(() => enemyRb.velocity.magnitude < 0.1f); // Attendre que le corps soit immobile
                // clone.transform.rotation = Quaternion.Euler(0f, 0f, 90f); // Réinitialiser la rotation
                clone.GetComponent<MonsterHealth>().enabled = false; // Désactiver le script de santé du corps
            }
            clone.GetComponent<MonsterHealth>().enabled = false; // Désactiver le script de santé du corps
            yield return null;
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
                knockBackCounter = 0;
                dashDirection = (playerRb.transform.position - myTransform.position).normalized;
                angleInRadians = Mathf.Atan2(dashDirection.y, dashDirection.x);
                angleInDegrees = angleInRadians * Mathf.Rad2Deg;
                rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f + angleInDegrees + 180 : 180f + angleInDegrees);
                Instantiate(dashExplosionParticles, myTransform.position, rotation);
                enemyRb.AddForce(dashDirection * dashDistance, ForceMode2D.Impulse);
                isTakingDamage = false;
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
        ContactDamage(collision);
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
        if (gameObject.CompareTag("Enemy") && (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Roofs")))
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

    private IEnumerator damageGruntAudio()
    {
        if(damageGruntPrefab != null)
        {
            GameObject damageGruntGameObject = Instantiate(damageGruntPrefab, transform.position, Quaternion.identity);
            AudioSource damageGruntAudioSource = damageGruntGameObject.GetComponent<AudioSource>();
            if(damageGruntAudioSource != null)
            {
                float newPitch = UnityEngine.Random.Range(0.9f,1f);
                damageGruntAudioSource.pitch = newPitch;
                damageGruntAudioSource.Play();
                Destroy(damageGruntGameObject, damageGruntAudioSource.clip.length);
            }
            yield return null;
        }
    }
}
