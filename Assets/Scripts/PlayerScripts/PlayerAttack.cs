using System;
using System.Collections;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


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

    public AudioSource audioSource;
    public AudioClip punchSoundEffect;
    
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
    public GameObject ComicBoomEffect;
    public GameObject DestructionGroundEffect;
    public PlayerLevel playerLevel;
    private bool damageIncreased = false;
    public float offsetParticlesX,offsetParticlesY;
    public ParticleSystem punchParticles;
    public GameObject ForceFieldParticles;
    public GameObject ForceFieldParticlesDown;
    public GameObject ForceFieldParticlesUp;
    private PlayerHealth playerHealth;
    public GameObject SlashEffect;
    public GameObject PowEffect;
    



    private Vector2 moveInput;
    private int direction;
    private RaycastHit2D hit;
    private Collider2D[] colliders;
    private float newPitch;
    private Rigidbody2D enemyRb;
    private MonsterHealth monsterHealth;
    private Vector2 directionVector;
    private Collider2D[] collidersDown;
    private GameObject comicBoom;
    private Light lighter;
    private GameObject destructionGroundObject;
    private Vector3 offsetParticles, offsetParticles2, offsetParticles3;
    private ParticleSystem punchPart, punchPart2, punchPart3;
    private Transform myTransform;
    private Volume volume;
    public GameObject postProcessing;
    private ColorAdjustments colorAdjustments;
    private ChromaticAberration chromaticAberration;
    public PlayerShooting playerShooting;
    public Vector3 offsetTest, offsetTestUp, offsetTestDown;
    public int isInPique;
    public GameObject punchGruntPrefab;
    public float waitStopTime;

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
        playerKi = GetComponent<PlayerMovement>();

        attack1 = false;
        attack2 = false;
        attack3 = false;
        attack4 = false;
        attack5 = false;
        attack6 = false;
        attack7 = false;
        playerLevel = GetComponent<PlayerLevel>();
        playerHealth = GetComponent<PlayerHealth>();
        myTransform = transform;
        volume = postProcessing.GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out chromaticAberration);
        isInPique = 3;
    }

    void Update()
    {
        if(!playerHealth.isHealing && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            Attackp();
            HoldingSelectF();
            moreLevelMoreDamage();
        }
    }

    void FixedUpdate()
    {
        if(!playerHealth.isHealing && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            Attack1234();
        }
    }

    public void moreLevelMoreDamage()
    {
        if(playerLevel.isLevelingUp && !damageIncreased)
        {
            damage += damage/15;
            forceMagnitudeUpward += forceMagnitudeUpward/125;
            forceMagnitudeForward += forceMagnitudeForward/125;
            forceMagnitudeDownward += forceMagnitudeDownward/125;
            playerShooting.bonusScale += 0.15f;
            damageIncreased = true;
        }else if (!playerLevel.isLevelingUp)
        {
            damageIncreased = false;
        }
    }
    private void Attackp()
    {
        moveInput = PlayerController.instance.playerInputActions.Player.Move.ReadValue<Vector2>();
        upwardAttackKey = moveInput.y;
        horizontalInput = moveInput.x;
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        detectionPosition = (Vector2)myTransform.position + Vector2.right * direction * detectionOffset; 
        detectionPositionDown = (Vector2)myTransform.position + Vector2.down * detectionOffsetAir.y + Vector2.right * direction * detectionOffsetAir.x;
        detectionPositionAttract = (Vector2)myTransform.position + Vector2.right * direction * detectionOffsetAttract; 
        
        rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
        

        if (PlayerController.instance.playerInputActions.Player.Attack.triggered && upwardAttackKey == 1 && attackTimeCounterUpward <= 0f && !isHoldingSelect)
        {
            // Obtenir la direction actuelle du sprite du joueur
            StartCoroutine(punchGruntAudio());
            animator.SetTrigger("SimpleAttackTriggerUp");
            attackTimeCounterUpward = timeBtwAttacksUpward;
            attack1 = true;
            
        } else if (PlayerController.instance.playerInputActions.Player.Attack.triggered  && upwardAttackKey == 0 && attackTimeCounter <= 0f && !isHoldingSelect)
        {
            StartCoroutine(punchGruntAudio());
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounter = timeBtwAttacks;
            attack2 = true;
        } else if (PlayerController.instance.playerInputActions.Player.Attack.triggered && upwardAttackKey == -1 && attackTimeCounterDownward <= 0f && !isHoldingSelect)
        {
            StartCoroutine(punchGruntAudio());
            animator.SetTrigger("SimpleAttackTriggerDown");
            attackTimeCounterDownward = timeBtwAttacksDownward;
            attack3 = true;  
        } /*else if (Input.GetButtonDown("Fire2")  && attackTimeCounterSlam <= 0f)
        {
            //animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterSlam = timeBtwAttacksSlam;
            attack4 = true;  
        }*/ else if(PlayerController.instance.playerInputActions.Player.Attraction.triggered && attackTimeCounterAttract <= 0f && !isHoldingSelect)
        {
            StartCoroutine(punchGruntAudio());
            StartCoroutine(playerKi.chargeGruntAudio());
            attackTimeCounterAttract = timeBtwAttacksAttract;
            attack5 = true;
        } else if(holdingTime >= requiredHoldingTime && PlayerController.instance.playerInputActions.Player.SuperShot.triggered && attackTimeCounterPique <= 0f && playerKi.currentKi >= piqueCost)
        {
            StartCoroutine(playerKi.chargeGruntAudio());
            playerKi.currentKi -= piqueCost;
            playerKi.UpdateKiBar();
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterPique = timeBtwAttacksPique;
            attack6 = true;
            if(holdingTime >= requiredHoldingTime && PlayerController.instance.playerInputActions.Player.SuperShot.triggered && isInPique > 0)
            {
                StartCoroutine(playerKi.chargeGruntAudio());
                playerKi.currentKi -= piqueCost;
                playerKi.UpdateKiBar();
                animator.SetTrigger("SimpleAttackTrigger");
                attackTimeCounterPique = 0;
                attack6 = true;
            }
        } else if(holdingTime>= requiredHoldingTime && PlayerController.instance.playerInputActions.Player.Attack.triggered && attackTimeCounterSamourai <= 0f && playerKi.currentKi >= samouraiCost)
        {
            StartCoroutine(punchGruntAudio());
            StartCoroutine(playerKi.chargeGruntAudio());
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
        if(PlayerController.instance.playerInputActions.Player.Hold.triggered)
        {   
            isHoldingSelect = true;
        }
        if(PlayerController.instance.playerInputActions.Player.Hold.ReadValue<float>() == 0f)
        {
            isHoldingSelect = false;
            holdingTime = 0f;
        }
        if(isHoldingSelect)
        {
            holdingTime += Time.deltaTime;
            
        }
    }

    /* private void HoldingSelectF()
    {
        // Vérifie si la touche de soin a été activée (première activation)
        if (PlayerController.instance.playerInputActions.Player.Heal.ReadValue<float>() > 0f)
        {
            isHoldingSelect = true;
            holdingTime = 0f; // Réinitialiser le temps de maintien lorsqu'on commence à maintenir
        }

        // Vérifie si la touche est toujours maintenue
        if (PlayerController.instance.playerInputActions.Player.Heal.ReadValue<float>() > 0f)
        {
            if (isHoldingSelect)
            {
                holdingTime += Time.deltaTime; // Mise à jour du temps de maintien
            }
        }
        else
        {
            // La touche n'est plus maintenue
            if (isHoldingSelect)
            {
                isHoldingSelect = false;
            }
        }
    } */

    private float TimerDecrement(float timeCounter)
    {
        if (timeCounter >= 0f)
            timeCounter -= Time.deltaTime;

        return timeCounter;
    }



    public bool IsGrounded()
    {

        hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayerMask);
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
        colliders = Physics2D.OverlapCircleAll(detectionPosition, detectionRadius, enemyLayerMask);
        StartCoroutine(SpawnPunchParticles(1));
        StartCoroutine(particlesForceFieldAttack(1));
        if (colliders.Length >= 1)
        {
            newPitch = UnityEngine.Random.Range(0.8f, 0.9f);
            audioSource.pitch = newPitch;
            Instantiate(hitEffect, detectionPosition, Quaternion.identity);
            playerRb.AddForce(Vector2.up * forceMagnitudeUpward, ForceMode2D.Impulse);
            audioSource.PlayOneShot(punchSoundEffect);
            CameraShakeManager.instance.CameraShake(impulseSource);
            // StartCoroutine(stopTime(waitStopTime));
            // StartCoroutine(DestroyDestructionGroundEffect());
        }
        // Appliquer une force pour projeter les ennemis vers le haut
        foreach (Collider2D collider in colliders)
        {
            enemyRb = collider.GetComponent<Rigidbody2D>();
            monsterHealth = collider.GetComponent<MonsterHealth>();
            if (enemyRb != null)
            {
                playerRb.transform.position = enemyRb.transform.position - offsetTest * direction;
                enemyRb.AddForce(Vector2.up * forceMagnitudeUpward, ForceMode2D.Impulse);
                if (monsterHealth != null)
                monsterHealth.TakeDamage(damage);
            }
        }
        attack1 = false;
        yield return null;
    }

    private IEnumerator Attack2Co()
    {
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        colliders = Physics2D.OverlapBoxAll(detectionPosition, new Vector2(detectionRadius+5,detectionRadius+4), 0,enemyLayerMask);
        // StartCoroutine(SpawnPunchParticles(2));
        StartCoroutine(particlesForceFieldAttack(2));
        if (colliders.Length >= 1)
            {
                
                newPitch = UnityEngine.Random.Range(0.8f, 0.9f);
                audioSource.pitch = newPitch;
                Instantiate(hitEffect, detectionPosition, Quaternion.identity);
                
                //playerRb.velocity = new Vector2(diagonal.x * selfForceMagnitudeForward,playerRb.velocity.y);
                playerRb.AddForce(Vector2.right * selfForceMagnitudeForward * -direction, ForceMode2D.Impulse);
                audioSource.PlayOneShot(punchSoundEffect);
                CameraShakeManager.instance.CameraShake(impulseSource);
                // StartCoroutine(stopTime(waitStopTime));
                // StartCoroutine(DestroyDestructionGroundEffect());

  
            }
            // Appliquer une force pour projeter les ennemis vers l'avant
            foreach (Collider2D collider in colliders)
            {
                enemyRb = collider.GetComponent<Rigidbody2D>();
                monsterHealth = collider.GetComponent<MonsterHealth>();
                
                if (enemyRb != null)
                {
                    
                    directionVector = ((Vector2)enemyRb.transform.position - (Vector2)myTransform.position).normalized;
                    playerRb.transform.position = enemyRb.transform.position - offsetTest * direction;
                    enemyRb.AddForce(directionVector * forceMagnitudeForward, ForceMode2D.Impulse);
                    //playerRb.AddForce(Vector2.right * -selfForceMagnitudeForward, ForceMode2D.Impulse);
                    if (monsterHealth != null)
                    monsterHealth.TakeDamage(damage);
                }

                
            }
            attack2 = false;
            yield return null;
    }

    private IEnumerator Attack3Co()
    {
        collidersDown = Physics2D.OverlapCircleAll(detectionPositionDown, detectionRadius, enemyLayerMask);
        StartCoroutine(SpawnPunchParticles(3));
        StartCoroutine(particlesForceFieldAttack(3));
        if (collidersDown.Length >= 1)
            {
                newPitch = UnityEngine.Random.Range(0.8f, 0.9f);
                audioSource.pitch = newPitch;
                Instantiate(hitEffect, detectionPosition, Quaternion.identity);
                playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
                audioSource.PlayOneShot(punchSoundEffect);
                CameraShakeManager.instance.CameraShake(impulseSource);
                // StartCoroutine(stopTime(waitStopTime));
                // StartCoroutine(DestroyDestructionGroundEffect());
            }
            foreach (Collider2D collider in collidersDown)
            {
                enemyRb = collider.GetComponent<Rigidbody2D>();
                //Rigidbody2D bossRb = collider.GetComponentInChildren<Rigidbody2D>();
                if (enemyRb != null)
                {
                    playerRb.transform.position = enemyRb.transform.position - offsetTest * direction;
                    enemyRb.AddForce(Vector2.down * forceMagnitudeDownward, ForceMode2D.Impulse);
                    //bossRb.AddForce(Vector2.down * forceMagnitudeDownward, ForceMode2D.Impulse);

                    monsterHealth = enemyRb.GetComponent<MonsterHealth>();
                    if (monsterHealth != null)
                    monsterHealth.TakeDamage(damage);
                    //monsterHealth.ContactDamage();
                }
            
            }
            attack3 = false;
            yield return null;
    }
    private IEnumerator WaitForLanding()
    {
        if(isInPique <= 0)
        {
            isInPique = 2;
            yield return null;
        } else
        {
            isInPique -= 1;
        }
        // Appliquer une force descendante aux ennemis
        if(moveInput == Vector2.zero)
        {
            playerRb.AddForce(Vector2.right * direction * slamForce + Vector2.down * slamForce, ForceMode2D.Impulse);
        }
        else
        {
            playerRb.AddForce(moveInput * slamForce, ForceMode2D.Impulse);
        }
        CameraShakeManager.instance.CameraShake(impulseSource);
        // Physics2D.IgnoreLayerCollision(9,11,true);
        StartCoroutine(particlesForceFieldAttack(2));
        yield return new WaitForSeconds(distancePique);
        
        playerRb.velocity = Vector2.zero;
        // Physics2D.IgnoreLayerCollision(9,11,false);
        // yield return new WaitForSeconds(doubleChocTimer);
        Instantiate(slamParticles, myTransform.position,rotation);
        CameraShakeManager.instance.CameraShake(impulseSource);
        colliders = Physics2D.OverlapCircleAll(myTransform.position, detectionRadiusPique, enemyLayerMask);
        if (colliders.Length >= 1)
        {
            //playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
            Instantiate(hitEffect, detectionPosition, Quaternion.identity);
            newPitch = UnityEngine.Random.Range(0.8f, 0.9f);
            audioSource.pitch = newPitch;
            audioSource.PlayOneShot(punchSoundEffect);
            CameraShakeManager.instance.CameraShake(impulseSource);
            Instantiate(PowEffect, myTransform.position, Quaternion.Euler(0f,0f,0f));
            StartCoroutine(stopTime(waitStopTime));
        }
        foreach (Collider2D collider in colliders)
        {
            enemyRb = collider.GetComponent<Rigidbody2D>();
            monsterHealth = collider.GetComponent<MonsterHealth>();
            if (enemyRb != null)
            {
                directionVector = ((Vector2)enemyRb.transform.position - (Vector2)myTransform.position).normalized;
                enemyRb.AddForce(directionVector * forceMagnitudeForward, ForceMode2D.Impulse);
                // Infliger des dégâts aux ennemis
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
        colliders = Physics2D.OverlapCircleAll(detectionPositionAttract, detectionRadiusAttract, enemyLayerMask);
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        if (colliders.Length >= 1)
            {
                /* Instantiate(hitEffect, detectionPosition, Quaternion.identity); */
                //playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
                newPitch = UnityEngine.Random.Range(0.8f, 0.9f);
                audioSource.pitch = newPitch;
                audioSource.PlayOneShot(punchSoundEffect);
                
                CameraShakeManager.instance.CameraShake(impulseSource);
                Instantiate(ComicBoomEffect, myTransform.position, Quaternion.Euler(0f,0f,0f));
                StartCoroutine(stopTime(waitStopTime));
            }
            foreach (Collider2D collider in colliders)
            {
                enemyRb = collider.GetComponent<Rigidbody2D>();
                monsterHealth = collider.GetComponent<MonsterHealth>();
                if (enemyRb != null)
                {
                    directionVector = ((Vector2)enemyRb.transform.position - (Vector2)myTransform.position).normalized;
                    enemyRb.AddForce(directionVector * -forceMagnitudeForward , ForceMode2D.Impulse);

                    Instantiate(hitEffect, enemyRb.transform.position, Quaternion.identity);
                    if (monsterHealth != null)
                    monsterHealth.TakeDamage(damage/2 * piqueRatio);
                    //monsterHealth.ContactDamage();
                }

            }
            attack5 = false;
            yield return null;
    }
    private IEnumerator SamouraiCo()
    {
        Time.timeScale = 0.77f;
        StartCoroutine(EffectBlackAndWhite());
        Physics2D.IgnoreLayerCollision(9,11,true);
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPositionAttract, detectionRadiusAttract, enemyLayerMask);
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        playerRb.velocity = Vector2.zero;
        positionTemp = myTransform.position; 
        myTransform.position = new Vector3(myTransform.position.x + detectionRadiusAttract*3 * direction, myTransform.position.y, myTransform.position.z);
        attackCenter = new Vector2(myTransform.position.x + detectionRadiusAttract*1.5f * -direction, myTransform.position.y );
        sizeAttack = myTransform.position + positionTemp;
        playerRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        
        playerRb.velocity = Vector2.zero;
        colliders = Physics2D.OverlapBoxAll(attackCenter, new Vector2(detectionRadiusAttract*3f,detectionRadiusAttract), 0, enemyLayerMask);
        if (colliders.Length >= 1)
        {
            Instantiate(hitEffect, detectionPosition, Quaternion.identity);
            //playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
            newPitch = UnityEngine.Random.Range(0.8f, 0.9f);
            audioSource.pitch = newPitch;
            audioSource.PlayOneShot(punchSoundEffect);
            
            CameraShakeManager.instance.CameraShake(impulseSource);
            // StartCoroutine(stopTime(waitStopTime));
            
        }

        foreach (Collider2D collider in colliders)
        {
            if (collider == null || collider.gameObject == null || collider.GetComponent<MonsterHealth>() == null)
            {
                continue;
            }
            else
            {
                //Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                //enemyRb.AddForce(directionVector * -forceMagnitudeForward , ForceMode2D.Impulse);

                monsterHealth = collider.GetComponent<MonsterHealth>();
                Instantiate(hitEffect, collider.GetComponent<Rigidbody2D>().transform.position, Quaternion.identity);
                Instantiate(SlashEffect, collider.GetComponent<Rigidbody2D>().transform.position, Quaternion.identity);
                if (monsterHealth != null)
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
        //Gizmos.DrawWireCube(detectionPosition, new Vector2(detectionRadius+5,detectionRadius+4));
        // Gizmos.DrawWireSphere(detectionPositionDown, detectionRadius);
        Gizmos.DrawWireSphere(detectionPosition, detectionRadius);
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

    public IEnumerator DestroyDestructionGroundEffect()
    {
        destructionGroundObject = Instantiate(DestructionGroundEffect,new Vector2(myTransform.position.x, myTransform.position.y), Quaternion.Euler(0f,0f,0f));
        yield return new WaitForSeconds(1f);
        Destroy(destructionGroundObject);
        yield return null;
    }

    private IEnumerator EffectBlackAndWhite()
    {
        colorAdjustments.hueShift.value = -180f;
        chromaticAberration.intensity.value = 1f;
        yield return new WaitForSeconds(0.5f);
        colorAdjustments.hueShift.value = 0f;
        chromaticAberration.intensity.value = 0f;
        yield return null;
        
    }

    private IEnumerator SpawnPunchParticles(int attacknumber)
    {
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        //Regular attack
        if (attacknumber == 2)
        {
            // offsetParticles = new Vector3(offsetParticlesX,0,0);
            offsetParticles2 = new Vector3(offsetParticlesX*2.5f,0,0);
            offsetParticles3 = new Vector3(offsetParticlesX*4f,0,0);

            // punchPart = Instantiate(punchParticles, myTransform.position + offsetParticles3 * direction, Quaternion.Euler(0f, 0f, 180f));
            // punchPart.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            punchPart2 = Instantiate(punchParticles, myTransform.position + offsetParticles2 * direction, Quaternion.Euler(0f, 0f, 180f));
            punchPart2.transform.localScale = new Vector3(2, 2, 1);
            yield return new WaitForSeconds(0.05f);
            punchPart3 = Instantiate(punchParticles, myTransform.position + offsetParticles * direction, Quaternion.Euler(0f, 0f, 180f));
            punchPart3.transform.localScale = new Vector3(3, 3, 1);
            yield return null;
        }
        //Down attack
        if(attacknumber == 3)
        {
            offsetParticles = new Vector3(detectionOffsetAir.x * direction,-offsetParticlesX,0);
            offsetParticles2 = new Vector3(detectionOffsetAir.x * direction,-offsetParticlesX*2.5f,0);
            
            punchPart = Instantiate(punchParticles, myTransform.position + offsetParticles2, Quaternion.Euler(0f, 0f, 90f));
            punchPart.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            punchPart2 = Instantiate(punchParticles, myTransform.position + offsetParticles, Quaternion.Euler(0f, 0f, 90f));
            punchPart2.transform.localScale = new Vector3(2, 2, 1);
            yield return null;
        }
        // Up attack
        if(attacknumber == 1)
        {
            offsetParticles = new Vector3(detectionOffset * direction,offsetParticlesX,0);
            offsetParticles2 = new Vector3(detectionOffset * direction,offsetParticlesX*2.5f,0);
            
            punchPart = Instantiate(punchParticles, myTransform.position + offsetParticles2, Quaternion.Euler(0f, 0f, 90f));
            punchPart.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            punchPart2 = Instantiate(punchParticles, myTransform.position + offsetParticles, Quaternion.Euler(0f, 0f, 90f));
            punchPart2.transform.localScale = new Vector3(2, 2, 1);
            yield return null;
        }
        //trueRotation = Quaternion.Euler() 
        //Instantiate(dashParticles,new Vector2(transform.position.x, transform.position.y -2), rotation);
    }

    private IEnumerator particlesForceFieldAttack(int number)
    {
        if(number == 2)
        {
            ForceFieldParticles.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            ForceFieldParticles.SetActive(false);
        } 
        if(number == 1)
        {
            ForceFieldParticlesUp.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            ForceFieldParticlesUp.SetActive(false);
        }
        if(number == 3)
        {
            ForceFieldParticlesDown.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            ForceFieldParticlesDown.SetActive(false);
        }
    }

    public IEnumerator DestroySlashEffectEffect()
    {
        comicBoom = Instantiate(SlashEffect,new Vector2(myTransform.position.x, myTransform.position.y + 2f), Quaternion.identity);
        lighter = comicBoom.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(comicBoom);
        Destroy(lighter);
        
        yield return null;
    }

    private IEnumerator punchGruntAudio()
    {
        GameObject punchGruntGameObject = Instantiate(punchGruntPrefab, transform.position, Quaternion.identity);
        AudioSource punchGruntAudioSource = punchGruntGameObject.GetComponent<AudioSource>();
        if(punchGruntAudioSource != null)
        {
            float newPitch = UnityEngine.Random.Range(0.9f,1f);
            punchGruntAudioSource.pitch = newPitch;
            punchGruntAudioSource.Play();
            Destroy(punchGruntGameObject, punchGruntAudioSource.clip.length);
        }
        yield return null;
    }

    public IEnumerator stopTime(float waitTime)
    {
        Time.timeScale = 0.33f;
        yield return new WaitForSeconds(waitTime);
        Time.timeScale = 1;
        yield return null;
    }
}
