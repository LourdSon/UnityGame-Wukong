using System;
using System.Collections;
using Cinemachine;
using UnityEngine;


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
    public float volumeSoundEffect = 0.55f;
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
        playerLevel = GetComponent<PlayerLevel>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if(!playerHealth.isHealing)
        {
            Attackp();
            HoldingSelectF();
            moreLevelMoreDamage();
        }
    }

    void FixedUpdate()
    {
        if(!playerHealth.isHealing)
        {
            Attack1234();
        }
    }

    public void moreLevelMoreDamage()
    {
        if(playerLevel.isLevelingUp && !damageIncreased)
        {
            damage += damage/10;
            forceMagnitudeUpward += forceMagnitudeUpward/100;
            forceMagnitudeForward += forceMagnitudeForward/100;
            forceMagnitudeDownward += forceMagnitudeDownward/100;


            damageIncreased = true;
        }else if (!playerLevel.isLevelingUp)
        {
            damageIncreased = false;
        }
    }
    private void Attackp()
    {
        Vector2 moveInput = PlayerController.instance.playerInputActions.Player.Move.ReadValue<Vector2>();
        upwardAttackKey = moveInput.y;
        horizontalInput = moveInput.x;
        int direction = playerRb.transform.rotation.y == 0 ? 1 : -1;
        detectionPosition = (Vector2)transform.position + Vector2.right * direction * detectionOffset; 
        detectionPositionDown = (Vector2)transform.position + Vector2.down * detectionOffsetAir.y + Vector2.right * direction * detectionOffsetAir.x;
        detectionPositionAttract = (Vector2)transform.position + Vector2.right * direction * detectionOffsetAttract; 
        
        rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
        

        if (PlayerController.instance.playerInputActions.Player.Attack.triggered && upwardAttackKey == 1 && attackTimeCounterUpward <= 0f && !isHoldingSelect)
        {
            // Obtenir la direction actuelle du sprite du joueur
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterUpward = timeBtwAttacksUpward;
            attack1 = true;
            
        } else if (PlayerController.instance.playerInputActions.Player.Attack.triggered  && upwardAttackKey == 0 && attackTimeCounter <= 0f && !isHoldingSelect)
        {
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounter = timeBtwAttacks;
            attack2 = true;
        } else if (PlayerController.instance.playerInputActions.Player.Attack.triggered && upwardAttackKey == -1 && attackTimeCounterDownward <= 0f && !isHoldingSelect)
        {
            animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterDownward = timeBtwAttacksDownward;
            attack3 = true;  
        } /*else if (Input.GetButtonDown("Fire2")  && attackTimeCounterSlam <= 0f)
        {
            //animator.SetTrigger("SimpleAttackTrigger");
            attackTimeCounterSlam = timeBtwAttacksSlam;
            attack4 = true;  
        }*/ else if(PlayerController.instance.playerInputActions.Player.Attraction.triggered && attackTimeCounterAttract <= 0f && !isHoldingSelect)
        {
            attackTimeCounterAttract = timeBtwAttacksAttract;
            attack5 = true;
        } else if(holdingTime >= requiredHoldingTime && PlayerController.instance.playerInputActions.Player.SuperShot.triggered && attackTimeCounterPique <= 0f && playerKi.currentKi >= piqueCost)
        {
            playerKi.currentKi -= piqueCost;
            playerKi.UpdateKiBar();
            attackTimeCounterPique = timeBtwAttacksPique;
            attack6 = true;
        } else if(holdingTime>= requiredHoldingTime && PlayerController.instance.playerInputActions.Player.Attack.triggered && attackTimeCounterSamourai <= 0f && playerKi.currentKi >= samouraiCost)
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
        StartCoroutine(SpawnPunchParticles(1));
        StartCoroutine(particlesForceFieldAttack(1));
        if (colliders.Length >= 1)
        {
            float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
            audioSource.pitch = newPitch;
            Instantiate(hitEffect, detectionPosition, Quaternion.identity);
            playerRb.AddForce(Vector2.up * forceMagnitudeUpward, ForceMode2D.Impulse);
            audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
            CameraShakeManager.instance.CameraShake(impulseSource);
            // StartCoroutine(DestroyDestructionGroundEffect());
        }
        // Appliquer une force pour projeter les ennemis vers le haut
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(Vector2.up * forceMagnitudeUpward, ForceMode2D.Impulse);
                MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                if (monsterHealth != null)
                monsterHealth.TakeDamage(damage);
            }
        }
        attack1 = false;
        yield return null;
    }

    private IEnumerator Attack2Co()
    {
        int direction = playerRb.transform.rotation.y == 0 ? 1 : -1;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(detectionPosition, new Vector2(detectionRadius+5,detectionRadius+4), 0,enemyLayerMask);
        StartCoroutine(SpawnPunchParticles(2));
        StartCoroutine(particlesForceFieldAttack(2));
        if (colliders.Length >= 1)
            {
                
                float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
                audioSource.pitch = newPitch;
                Instantiate(hitEffect, detectionPosition, Quaternion.identity);
                
                //playerRb.velocity = new Vector2(diagonal.x * selfForceMagnitudeForward,playerRb.velocity.y);
                playerRb.AddForce(Vector2.right * selfForceMagnitudeForward * -direction, ForceMode2D.Impulse);
                audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
                CameraShakeManager.instance.CameraShake(impulseSource);
                // StartCoroutine(DestroyDestructionGroundEffect());

  
            }
            // Appliquer une force pour projeter les ennemis vers l'avant
            foreach (Collider2D collider in colliders)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                    enemyRb.AddForce(directionVector * forceMagnitudeForward, ForceMode2D.Impulse);
                    //playerRb.AddForce(Vector2.right * -selfForceMagnitudeForward, ForceMode2D.Impulse);
                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                    if (monsterHealth != null)
                    monsterHealth.TakeDamage(damage);
                }

                
            }
            attack2 = false;
            yield return null;
    }

    private IEnumerator Attack3Co()
    {
        Collider2D[] collidersDown = Physics2D.OverlapCircleAll(detectionPositionDown, detectionRadius, enemyLayerMask);
        StartCoroutine(SpawnPunchParticles(3));
        StartCoroutine(particlesForceFieldAttack(3));
        if (collidersDown.Length >= 1)
            {
                float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
                audioSource.pitch = newPitch;
                Instantiate(hitEffect, detectionPosition, Quaternion.identity);
                playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
                audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
                CameraShakeManager.instance.CameraShake(impulseSource);
                // StartCoroutine(DestroyDestructionGroundEffect());
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
        
        int direction = playerRb.transform.rotation.y == 0 ? 1 : -1;
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
            Instantiate(ComicBoomEffect, transform.position, Quaternion.Euler(0f,0f,0f));
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
        int direction = playerRb.transform.rotation.y == 0 ? 1 : -1;
        if (colliders.Length >= 1)
            {
                /* Instantiate(hitEffect, detectionPosition, Quaternion.identity); */
                //playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
                float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
                audioSource.pitch = newPitch;
                audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
                
                CameraShakeManager.instance.CameraShake(impulseSource);
                Instantiate(ComicBoomEffect, transform.position, Quaternion.Euler(0f,0f,0f));
            }
            foreach (Collider2D collider in colliders)
            {
                Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                    enemyRb.AddForce(directionVector * -forceMagnitudeForward , ForceMode2D.Impulse);

                    MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
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
        Physics2D.IgnoreLayerCollision(9,11,true);
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionPositionAttract, detectionRadiusAttract, enemyLayerMask);
        int direction = playerRb.transform.rotation.y == 0 ? 1 : -1;
        playerRb.velocity = Vector2.zero;
        positionTemp = playerRb.transform.position; 
        playerRb.transform.position = new Vector3(playerRb.transform.position.x + detectionRadiusAttract*3 * direction, playerRb.transform.position.y, playerRb.transform.position.z);
        attackCenter = new Vector2(playerRb.transform.position.x + detectionRadiusAttract*1.5f * -direction, playerRb.transform.position.y );
        sizeAttack = playerRb.transform.position + positionTemp;
        playerRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        
        playerRb.velocity = Vector2.zero;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackCenter, new Vector2(detectionRadiusAttract*3f,detectionRadiusAttract), 0, enemyLayerMask);
        if (colliders.Length >= 1)
        {
            Instantiate(hitEffect, detectionPosition, Quaternion.identity);
            //playerRb.AddForce(Vector2.up * selfForceMagnitudeForward/1.5f, ForceMode2D.Impulse);
            float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
            audioSource.pitch = newPitch;
            audioSource.PlayOneShot(punchSoundEffect, volumeSoundEffect);
            
            CameraShakeManager.instance.CameraShake(impulseSource);
            Instantiate(ComicBoomEffect, transform.position, Quaternion.Euler(0f,0f,0f));
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

                MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                Instantiate(hitEffect, collider.GetComponent<Rigidbody2D>().transform.position, Quaternion.identity);
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
        GameObject comicBoom = Instantiate(ComicBoomEffect,new Vector2(transform.position.x,transform.position.y + 2f), Quaternion.identity);
        Light light = comicBoom.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(comicBoom);
        Destroy(light);
        
        yield return null;
    }

    public IEnumerator DestroyDestructionGroundEffect()
    {
        GameObject destructionGroundObject = Instantiate(DestructionGroundEffect,new Vector2(transform.position.x,transform.position.y), Quaternion.Euler(0f,0f,0f));
        yield return new WaitForSeconds(1f);
        Destroy(destructionGroundObject);
        yield return null;
    }

    private IEnumerator SpawnPunchParticles(int attacknumber)
    {
        int direction = playerRb.transform.rotation.y == 0 ? 1 : -1;
        //Regular attack
        if (attacknumber == 2)
        {
            Vector3 offsetParticles = new Vector3(offsetParticlesX,0,0);
            Vector3 offsetParticles2 = new Vector3(offsetParticlesX*2.5f,0,0);
            Vector3 offsetParticles3 = new Vector3(offsetParticlesX*5f,0,0);

            ParticleSystem punchPart = Instantiate(punchParticles,transform.position + offsetParticles * direction, Quaternion.Euler(0f, 0f, 180f));
            punchPart.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            ParticleSystem punchPart2 = Instantiate(punchParticles,transform.position + offsetParticles2 * direction, Quaternion.Euler(0f, 0f, 180f));
            punchPart2.transform.localScale = new Vector3(2, 2, 1);
            yield return new WaitForSeconds(0.05f);
            ParticleSystem punchPart3 = Instantiate(punchParticles,transform.position + offsetParticles3 * direction, Quaternion.Euler(0f, 0f, 180f));
            punchPart3.transform.localScale = new Vector3(3, 3, 1);
            yield return null;
        }
        //Down attack
        if(attacknumber == 3)
        {
            Vector3 offsetParticles = new Vector3(detectionOffsetAir.x * direction,-offsetParticlesX,0);
            Vector3 offsetParticles2 = new Vector3(detectionOffsetAir.x * direction,-offsetParticlesX*2.5f,0);
            
            ParticleSystem punchPart = Instantiate(punchParticles,transform.position + offsetParticles, Quaternion.Euler(0f, 0f, 90f));
            punchPart.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            ParticleSystem punchPart2 = Instantiate(punchParticles,transform.position + offsetParticles2, Quaternion.Euler(0f, 0f, 90f));
            punchPart2.transform.localScale = new Vector3(2, 2, 1);
            yield return null;
        }
        // Up attack
        if(attacknumber == 1)
        {
            Vector3 offsetParticles = new Vector3(detectionOffset * direction,offsetParticlesX,0);
            Vector3 offsetParticles2 = new Vector3(detectionOffset * direction,offsetParticlesX*2.5f,0);
            
            ParticleSystem punchPart = Instantiate(punchParticles,transform.position + offsetParticles, Quaternion.Euler(0f, 0f, 90f));
            punchPart.transform.localScale = new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            ParticleSystem punchPart2 = Instantiate(punchParticles,transform.position + offsetParticles2, Quaternion.Euler(0f, 0f, 90f));
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
}
