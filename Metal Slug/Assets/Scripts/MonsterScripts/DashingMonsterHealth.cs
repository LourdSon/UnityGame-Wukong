
using System.Collections;

using Cinemachine;

using UnityEngine;

public class DashingMonsterHealth : MonoBehaviour
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
    private DashingMonster dashingMonster;
    private GameObject player;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSpriteRenderer;
    public float dashDistance = 50f;


    private Vector2 dashDirection;
    private int direction;
    private Quaternion rotation;
    private float impactForce;
    private float additionalDamage;
    private RaycastHit2D hit;
    private Transform myTransform;
    private Transform playerTransform;



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
        dashingMonster = GetComponent<DashingMonster>();
        player = GameObject.FindWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        myTransform = transform;
        playerTransform = playerRb.transform;
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
        //CameraShakeManager.instance.CameraShake(impulseSource);

        SpawnDamageParticles();

        //ContactDamage();
        //StartCoroutine(FlashCoroutine());

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
                enemyRb.velocity = Vector3.zero;
                dashDirection = (playerTransform.position - myTransform.position).normalized;
                enemyRb.AddForce(dashDirection * dashDistance, ForceMode2D.Impulse);
            }
        }
    }

    private void SpawnDamageParticles()
    {
        direction = playerTransform.rotation.y == 0 ? 1 : -1;
        rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 180f : 0f);  
        damageParticlesInstance = Instantiate(damageParticles,myTransform.position, rotation);
        Instantiate(impactParticles,myTransform.position, rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Roofs"))
        {
            if(enemyRb.velocity.magnitude > 15f)
            {
                impactForce = collision.relativeVelocity.magnitude;
                additionalDamage = impactForce * additionalDamageMultiplier;
                TakeDamage(normalDamage + additionalDamage);
            }
        }
    }

    public void ContactDamage()
    {

        if(enemyRb.velocity.magnitude > 15f && !IsGrounded())
        {
            // StartCoroutine(DamageGrounded());
        }
    }

    public IEnumerator DamageGrounded()
    {
        impactForce = enemyRb.velocity.magnitude;
        additionalDamage =  impactForce * additionalDamageMultiplier;
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
        
        hit = Physics2D.Raycast(myTransform.position, Vector2.down, rayDistance, groundLayerMask);
         // Dessiner le raycast dans la vue de scène pour le débogage
        Debug.DrawRay(myTransform.position, Vector2.down * rayDistance, Color.red);
        return hit.collider != null;
    }
}
