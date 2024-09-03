

using System;
using System.Collections;

using Cinemachine;

//using System.Numerics;
using UnityEngine;

//using Unity.Mathematics;


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

    public float timeBtwAttacksAttract = 2f;
    public float attackTimeCounterAttract = 0f;
    public float attractForce = 50f;
    public Vector2 detectionPositionAttract;
    public float detectionOffsetAttract = 2f;
    public float detectionRadiusAttract = 5f;


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
    public bool attack5;

    public ParticleSystem slamParticles;
    public Quaternion rotation;

    private AudioSource audioSource;
    public AudioClip punchSoundEffect;
    public float volumeSoundEffect = 0.25f;
    public ParticleSystem hitEffect;
    public float horizontalInput;
    public float upwardAttackKey;

    [Header("HoldingSelect")]
    public bool isHoldingSelect = false;
    public bool isHoldingSelect2 = false;
    public float holdingTime = 0f;
    public float requiredHoldingTime = 0.5f;
    public bool attack6 = false;
    public float distancePique = 0.5f;
    public float attackTimeCounterPique = 0f;
    public float timeBtwAttacksPique = 0.5f;
    public float detectionRadiusPique = 10f;
    public float piqueRatio = 1.35f;
    public float attackTimeCounterSamourai = 0f;
    public float timeBtwAttacksSamourai = 0.5f;
    public bool attack7;
    public float samouraiRatio = 1.5f;
    public Vector3 positionTemp;
    public Vector2 sizeAttack;
    public Vector2 attackCenter;
    private PlayerMovement playerKi;
    public float piqueCost = 15f;
    public float samouraiCost = 15f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Récupérer le composant SpriteRenderer
        animator = GetComponent<Animator>();
        attackTimeCounter = 0f;
        attackTimeCounterUpward = 0f;
        attackTimeCounterDownward = 0f;
        attackTimeCounterSlam = 0f;
        attackTimeCounterAttract = 0f;
        attackTimeCounterPique = 0f;
        attackTimeCounterSamourai = 0f;

        playerRb = GetComponent<Rigidbody2D>();
        tileDestroyer = GetComponentInChildren<TileDestroyer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GetComponent<AudioSource>();
        playerKi = GetComponent<PlayerMovement>();

        attack1 = false;
        attack2 = false;
        attack3 = false;
        attack4 = false;
        attack5 = false;
        attack6 = false;
        attack7 = false;

    }

    void Update()
    {
        Attackp();
        HoldingSelectF();
    }

    void FixedUpdate()
    {
        Attack1234();
    }

    private void Attackp()
    {
        upwardAttackKey = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        int direction = spriteRenderer.flipX ? -1 : 1;
        detectionPosition = (Vector2)transform.position + Vector2.right * direction * detectionOffset; 
        detectionPositionDown = (Vector2)transform.position + Vector2.down * detectionOffsetAir.y + Vector2.right * direction * detectionOffsetAir.x;
        detectionPositionAttract = (Vector2)transform.position + Vector2.right * direction * detectionOffsetAttract; 
        
        rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
        

        if (Input.GetButtonDown("Fire1") && upwardAttackKey == 1 && attackTimeCounterUpward <= 0f && !isHoldingSelect)
        {
            // Obtenir la direction actuelle du sprite du joueur
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterUpward = timeBtwAttacksUpward;
            attack1 = true;
            
        } else if (Input.GetButtonDown("Fire1")  && upwardAttackKey == 0 && attackTimeCounter <= 0f && !isHoldingSelect)
        {
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounter = timeBtwAttacks;
            attack2 = true;
        } else if (Input.GetButtonDown("Fire1") && upwardAttackKey == -1 && attackTimeCounterDownward <= 0f && !isHoldingSelect)
        {
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterDownward = timeBtwAttacksDownward;
            attack3 = true;  
        } /*else if (Input.GetButtonDown("Fire2")  && attackTimeCounterSlam <= 0f)
        {
            //animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterSlam = timeBtwAttacksSlam;
            attack4 = true;  
        }*/ else if(Input.GetButtonDown("Boomerang") && attackTimeCounterAttract <= 0f && !isHoldingSelect)
        {
            attackTimeCounterAttract = timeBtwAttacksAttract;
            attack5 = true;
        } else if(holdingTime >= requiredHoldingTime && Input.GetButtonDown("Fire2") && attackTimeCounterPique <= 0f && playerKi.currentKi >= piqueCost)
        {
            playerKi.currentKi -= piqueCost;
            playerKi.UpdateKiBar();
            attackTimeCounterPique = timeBtwAttacksPique;
            attack6 = true;
        } else if(holdingTime>= requiredHoldingTime && Input.GetButtonDown("Fire1") && attackTimeCounterSamourai <= 0f && playerKi.currentKi >= samouraiCost)
        {
            playerKi.currentKi -= samouraiCost;
            playerKi.UpdateKiBar();
            attackTimeCounterSamourai = timeBtwAttacksSamourai;
            attack7 = true;
            
        }


        attackTimeCounter = TimerDecrement(attackTimeCounter);
        attackTimeCounterSlam = TimerDecrement(attackTimeCounterSlam);
        attackTimeCounterDownward = TimerDecrement(attackTimeCounterDownward);
        attackTimeCounterUpward = TimerDecrement(attackTimeCounterUpward);
        attackTimeCounterAttract = TimerDecrement(attackTimeCounterAttract);
        attackTimeCounterPique = TimerDecrement(attackTimeCounterPique);
        attackTimeCounterSamourai = TimerDecrement(attackTimeCounterSamourai);
    }
        

    private void HoldingSelectF()
    {
        if(Input.GetButtonDown("Return Boomerang"))
        {   
            isHoldingSelect = true;
        }
        if(Input.GetButtonUp("Return Boomerang"))
        {
            isHoldingSelect = false;
            holdingTime = 0f;
        }
        if(isHoldingSelect)
        {
            holdingTime += Time.deltaTime;
            
        }
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
        /*else if (attack4 == true)
        {
            StartCoroutine(WaitForLanding());
            attack4 = false;
        } */else if (attack5 == true)
        {
            StartCoroutine(Attack5Co());
            attack5 = false;
        } else if (attack6 == true)
        {
            StartCoroutine(WaitForLanding());
            attack6 = false;
        } else if (attack7 == true)
        {
            StartCoroutine(SamouraiCo());
            attack7 = false;
        }
    }

    private IEnumerator Attack1Co()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, enemyLayerMask);
        if (colliders.Length >= 1)
        {
            float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
            audioSource.pitch = newPitch;
            Instantiate(hitEffect, detectionPosition, Quaternion.identity);
            playerRb.AddForce(Vector2.up * forceMagnitudeUpward, ForceMode2D.Impulse);
            audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
            CameraShakeManager.instance.CameraShake(impulseSource);
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
        Collider2D[] colliders = Physics2D.OverlapBoxAll(detectionPosition, new Vector2(detectionRadius+5,detectionRadius+4), 0,enemyLayerMask);
        if (colliders.Length >= 1)
            {
                Debug.Log(colliders[0]);
                float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
                audioSource.pitch = newPitch;
                Instantiate(hitEffect, detectionPosition, Quaternion.identity);
                int direction = spriteRenderer.flipX ? -1 : 1;
                //playerRb.velocity = new Vector2(diagonal.x * selfForceMagnitudeForward,playerRb.velocity.y);
                playerRb.AddForce(Vector2.right * selfForceMagnitudeForward * -direction, ForceMode2D.Impulse);
                audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
                CameraShakeManager.instance.CameraShake(impulseSource);
            }
            // Appliquer une force pour projeter les ennemis vers l'avant
            foreach (Collider2D collider in colliders)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                
                if (enemyRb != null && enemyRb.tag == "Enemy")
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
                float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
                audioSource.pitch = newPitch;
                Instantiate(hitEffect, detectionPosition, Quaternion.identity);
                playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
                audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
                CameraShakeManager.instance.CameraShake(impulseSource);
            }
            foreach (Collider2D collider in collidersDown)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                //Rigidbody2D bossRb = collider.GetComponentInChildren<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemyRb.AddForce(Vector2.down * forceMagnitudeDownward, ForceMode2D.Impulse);
                    //bossRb.AddForce(Vector2.down * forceMagnitudeDownward, ForceMode2D.Impulse);

                    MonsterHealth monsterHealth = enemyRb.GetComponent<MonsterHealth>();

                    monsterHealth.TakeDamage(damage);
                    //monsterHealth.ContactDamage();
                }
            
            }
            attack3 = false;
            yield return null;
    }
    private IEnumerator WaitForLanding()
    {
        
        int direction = spriteRenderer.flipX ? -1 : 1;
        // Appliquer une force descendante aux ennemis
        playerRb.AddForce(Vector2.right * direction * slamForce + Vector2.down * slamForce, ForceMode2D.Impulse);
        CameraShakeManager.instance.CameraShake(impulseSource);
        Physics2D.IgnoreLayerCollision(9,11,true);
        yield return new WaitForSeconds(distancePique);
        
        playerRb.velocity = Vector2.zero;
        Physics2D.IgnoreLayerCollision(9,11,false);
        // yield return new WaitForSeconds(doubleChocTimer);
        Instantiate(slamParticles,transform.position,rotation);
        CameraShakeManager.instance.CameraShake(impulseSource);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadiusPique, enemyLayerMask);
        if (colliders.Length >= 1)
        {
            //playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
            Instantiate(hitEffect, detectionPosition, Quaternion.identity);
            float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
            audioSource.pitch = newPitch;
            audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
            CameraShakeManager.instance.CameraShake(impulseSource);
        }
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                enemyRb.AddForce(directionVector * forceMagnitudeForward, ForceMode2D.Impulse);
                // Infliger des dégâts aux ennemis
                MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                if (monsterHealth != null)
                {
                    monsterHealth.TakeDamage(damage);
                }
            }
        }
        attack6 = false;
        
        //tileDestroyer.OGDestructionMouse();
        yield return null;

    }

    private IEnumerator Attack5Co()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPositionAttract, detectionRadiusAttract, enemyLayerMask);
        int direction = spriteRenderer.flipX ? -1 : 1;
        if (colliders.Length >= 1)
            {
                Instantiate(hitEffect, detectionPosition, Quaternion.identity);
                //playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
                float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
                audioSource.pitch = newPitch;
                audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
                
                CameraShakeManager.instance.CameraShake(impulseSource);
            }
            foreach (Collider2D collider in colliders)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                    enemyRb.AddForce(directionVector * -forceMagnitudeForward , ForceMode2D.Impulse);

                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();

                    monsterHealth.TakeDamage(damage * piqueRatio);
                    //monsterHealth.ContactDamage();
                }

            }
            attack5 = false;
            yield return null;
    }
    private IEnumerator SamouraiCo()
    {
        Time.timeScale = 0.77f;
        Physics2D.IgnoreLayerCollision(9,11,true);
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPositionAttract, detectionRadiusAttract, enemyLayerMask);
        int direction = spriteRenderer.flipX ? -1 : 1;
        playerRb.velocity = Vector2.zero;
        positionTemp = playerRb.transform.position; 
        playerRb.transform.position = new Vector3(playerRb.transform.position.x + detectionRadiusAttract*3 * direction, playerRb.transform.position.y, playerRb.transform.position.z);
        attackCenter = new Vector2(playerRb.transform.position.x + detectionRadiusAttract*1.5f * -direction, playerRb.transform.position.y );
        sizeAttack = playerRb.transform.position + positionTemp;
        playerRb.velocity = Vector2.zero;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackCenter, new Vector2(detectionRadiusAttract*3f,detectionRadiusAttract), 0, enemyLayerMask);
        yield return new WaitForSeconds(0.5f);
        
        playerRb.velocity = Vector2.zero;
        if (colliders.Length >= 1)
            {
                Instantiate(hitEffect, detectionPosition, Quaternion.identity);
                //playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
                float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
                audioSource.pitch = newPitch;
                audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
                
                CameraShakeManager.instance.CameraShake(impulseSource);
            }
            foreach (Collider2D collider in colliders)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    //Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                    //enemyRb.AddForce(directionVector * -forceMagnitudeForward , ForceMode2D.Impulse);

                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();

                    monsterHealth.TakeDamage(damage * samouraiRatio);
                    //monsterHealth.ContactDamage();
                }

            }
            Time.timeScale = 1;
            Physics2D.IgnoreLayerCollision(9,11,false);
            attack7 = false;
            yield return null;
    }

    // Afficher le rayon de détection dans l'éditeur Unity
    private void OnDrawGizmosSelected()
    {
        // Dessiner le rayon de détection dans l'éditeur
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(detectionPosition, new Vector2(detectionRadius+5,detectionRadius+4));
    }
}
