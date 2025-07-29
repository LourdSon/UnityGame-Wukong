
using System;
using System.Collections;

using UnityEngine;

using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{

    
    [Header("Horizontal movement")]
    //Move Variables
    public float moveSpeed = 10.0f;
    private float horizontalInput;
    private float verticalInput;
    public int direction;
    


    //Jump Variables
    private Rigidbody2D playerRb;
    private SpriteRenderer spriteRenderer;
    public Animator animator;

    
    [Header("Jump movement")]
    public float jumpForce = 15.0f;
    public float gravityModifier = 2;
    public bool isGrounded = true;
    private float jumpCounter = 2;

    public AnimationCurve jumpCurve; // Une courbe d'interpolation pour ajuster le saut
    private float jumpTime; // Le temps écoulé depuis le début du saut




    [Header("Dash movement")]
    public float dashForce = 10f; // La force du dash
    public float dashDuration = 0.2f; // La durée du dash
    public float dashCooldown = 1f; // Le temps de recharge du dash
    private float nextDashTime; // Temps auquel le prochain dash sera autorisé
    public bool isDashing = false;
    public float speedReductionDuration = 0.5f;

    [Header("Instant Dash movement")]
    public float instantDashForce = 10f; // La force du dash
    public float instantDashDuration = 0.2f; // La durée du dash
    public float instantDashCooldown = 0.5f; // Le temps de recharge du dash
    private float nextInstantDashTime; // Temps auquel le prochain dash sera autorisé

    [Header("Punching")]
    public float punchAttackDuration = 0.2f;
    public float punchAttackCooldown = 1f;
    private float nextPunchAttackTime;
    public float enemyStepBackDistance = 1f;

    [Header("Charging Ki")]
    public GameObject KiCharging;
    public GameObject KiCharging2;
    public float chargeDuration = 1.3f; // La durée du dash
    public float chargeCooldown = 1f; // Le temps de recharge du dash
    private float nextChargeTime;
    public float maxKi = 100f;
    public float currentKi = 0f;
    public UnityEngine.UI.Image KiBarFill;


    [Header("Audio Source")]
    private AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioClip soundEffect;
    public AudioClip KiAura;
    public AudioClip walkSound;



    [Header("Attack Movement")]
    public bool attack = false;
    public float timeBetweenAttack;
    public float timeSinceAttack;


    private CinemachineImpulseSource impulseSource;
    public ParticleSystem dashParticles;
    public ParticleSystem dashExplosionParticles;

    public float detectionBoxKi =15f;
    public LayerMask enemyLayerMask;
    public float forceMagnitudeForward = 10f;

    public TileDestroyer tileDestroyer;
    

    private Vector2 movement;
    private Vector2 jumpVelocity;
    private bool jumpBool;
    public bool isCharging;

    public LayerMask playerLayerMask;
    public float detectionRadius = 5f;
    Material material;

    public Quaternion rotation;
    public Quaternion trueRotation;
    public ParticleSystem slamParticles;

    private PlayerAttack playerAttack;

    public float rayDistance;
    public LayerMask groundLayerMask;
    public bool wantToFight;
    public bool isJumping = false;
    public GameObject dashEffect;
    public GameObject DestructionGroundEffect;
    public Light2D auraLight;
    public float footstepTimer = 0f;
    public float footstepDelay = 0.5f;
    public bool isGrounder;
    public PlayerLevel playerLevel;
    private bool forceIncreased;
    public GameObject ForceFieldParticles;
    private PlayerHealth playerHealth;
    public float volumeScaleFootstep;
    public GameObject ChargeUpEffect;
    public GameObject LevelUpEffect;
    
    
    private Vector2 moveInput;
    private float horizontalMove;
    private float newPitch;
    private Vector2 dashMovement;
    private float defaultSpeed;
    private float angleInRadians;
    private float angleInDegrees;
    private float targetKiFillAmount;
    private Collider2D[] colliders;
    private Rigidbody2D enemyRb;
    private Vector2 directionVector;
    private MonsterHealth monsterHealth;
    private Vector2 instantDashDirection;
    private Vector2 inputPlayer;
    private Vector2 newPosition;
    private ThorHammer thorHammer;
    private RaycastHit2D hit;
    private GameObject destructionGroundObject;
    private GameObject comicBoom;
    private Light lighter;
    private Transform myTransform;
    public GameObject chargeMaxEffect;
    private GameObject chargeMax;
    public TutorialScript tutorialScript;
    public AudioSource dashAudioSource;
    public GameObject chargeGruntPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();


        Physics2D.gravity *= gravityModifier;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentKi = maxKi;
        UpdateKiBar();

        impulseSource = GetComponent<CinemachineImpulseSource>();
        playerAttack = GetComponent<PlayerAttack>();
        if(SceneManager.GetActiveScene().buildIndex == 2)
            wantToFight = true;
        //tileDestroyer = GetComponentInChildren<TileDestroyer>();
        Physics2D.IgnoreLayerCollision(9,16,true);

        playerLevel = GetComponent<PlayerLevel>();
        ForceFieldParticles.SetActive(false);
        playerHealth = GetComponent<PlayerHealth>();
        myTransform = transform;
        
    }


    void Update()
    {
        if(/* !EndMission.isCutsceneon || */ !tutorialScript.isInTutorialText && !playerHealth.isHealing && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)  
        {  
            ReadInputMove();
            ReadInputJump();
            ReadDashPress();
            ReadChargeKi();
            IsGrounded();
            moreLevelMoreForce();
            RoofJump();
        }

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdling", true);
            isGrounded = true;
            animator.SetBool("IsGrounded()", true);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(/* !EndMission.isCutsceneon || */ !tutorialScript.isInTutorialText && !playerHealth.isHealing && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)  
        {  
               
            MovePlayer();
            Jump();
            ResetPosition(); // P
            DashPress(); // Q
            ChargeKi(); // E
            InstantDash(); // V
            ThorTest();
            //PunchAttack();
        }
    }

    public void moreLevelMoreForce()
    {
        if(playerLevel.isLevelingUp && !forceIncreased)
        {
            dashForce += dashForce/100;
            jumpForce += jumpForce/100;
            // playerHealth.health = playerHealth.maxHealth; 
            forceIncreased = true;
            StartCoroutine(DestroyLevelUpEffect());
        }else if (!playerLevel.isLevelingUp)
        {
            forceIncreased = false;
        }
    }

    private void ReadInputMove()
    {
        if(isDashing == false)
        {
            //Get Axis from Unity
            moveInput = PlayerController.instance.playerInputActions.Player.Move.ReadValue<Vector2>();
            horizontalInput = moveInput.x;
            verticalInput = moveInput.y;
            horizontalMove = horizontalInput * moveSpeed;
            movement = new Vector2(horizontalMove, verticalInput);
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y,0f);

            //Flip sprite
            if (horizontalInput != 0)
            {
                PlayFootsteps();
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsIdling", false);
                if (horizontalInput < 0)
                {
                    myTransform.rotation = Quaternion.Euler(myTransform.rotation.x, 180f, myTransform.rotation.z);
                    direction = -1;
                } else if (horizontalInput > 0)
                {
                    myTransform.rotation = Quaternion.Euler(myTransform.rotation.x, 0f,myTransform.rotation.z);
                    direction = 1;
                }
            } else
            {
                audioSource2.Stop();
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsIdling", true);
            }
        }
    }
    void PlayFootsteps()
    {
        // Gère le timer pour jouer les pas à intervalles réguliers
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= footstepDelay && isGrounded)
        {
            newPitch = UnityEngine.Random.Range(0.9f,1.1f);
            audioSource2.pitch = newPitch;
            audioSource2.PlayOneShot(walkSound, volumeScaleFootstep); 
            footstepTimer = 0f;                      
        }
    }

    private void MovePlayer()
    {
        
            //Move the character
            myTransform.Translate(Vector2.right * Time.deltaTime * moveSpeed * direction * horizontalInput);
            //playerRb.AddForce(Vector2.right * movement.x);
            //playerRb.velocity = new Vector2(movement.x, playerRb.velocity.y);       
            //playerRb.MovePosition((Vector2) transform.position + movement * Time.deltaTime);
    }
    private void Jump()
    {
        if (jumpBool)
        {
            isJumping = true;
            SpawnJumpParticles();
            playerRb.velocity = Vector2.Lerp(playerRb.velocity, jumpVelocity, jumpCurve.Evaluate(jumpTime));
            jumpBool = false;
        }
    }

    public void RoofJump()
    {
        if (isJumping)
        {
            // Si le joueur monte (vitesse verticale positive)
            if (playerRb.velocity.y >= 0)
            {
                // Ignore les collisions entre le joueur et la couche de plateforme pendant la montée
                Physics2D.IgnoreLayerCollision(9, 14, true);
            }
            // Si le joueur descend (vitesse verticale négative)
            else if (playerRb.velocity.y < 0)
            {
                // Réactive les collisions entre le joueur et la couche de plateforme pendant la descente
                Physics2D.IgnoreLayerCollision(9, 14, false);
            }
            if (playerRb.velocity.y == 0)
            {
                isJumping = false; // Le saut est terminé
                Physics2D.IgnoreLayerCollision(9, 14, false); // Réactivation des collisions
            }
        }
    }

    private void ReadInputJump()
    {
        //Jump
        if(PlayerController.instance.playerInputActions.Player.Jump.triggered && isGrounder)
        {
            jumpVelocity = new Vector2(playerRb.velocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
            animator.SetTrigger("JumpingTrigger");     
            jumpBool = true;                    
            jumpCounter -=1;
                               
            if (jumpCounter == 0)
            {   
                isGrounder = false;
                jumpCounter = 2;
            }
            
        }
    }




    private void ResetPosition()
    {
        //Reset Player position
        if(PlayerController.instance.playerInputActions.Player.ResetPosition.triggered)
        {
            myTransform.position = new Vector2(70,2);
        }
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Roofs"))
        {
            isGrounder = true;
            jumpCounter = 2;
            animator.SetBool("IsJumping", false);
        }
    }


    private void ReadDashPress()
    {
        // Si le joueur appuie sur le bouton de dash et le dash est prêt
        if (PlayerController.instance.playerInputActions.Player.Dash.ReadValue<float>() == 1  && Time.time > nextDashTime)
        {
            SpawnDashParticles();
            
            isGrounder=true;
            jumpCounter = 1;
            isDashing = true;
            animator.SetBool("IsDashing", true);
            animator.SetBool("IsIdling", false);
            nextDashTime = Time.time + dashCooldown; // Mettre à jour le temps de recharge du dash
        }
    }
    private void DashPress()
    {
        if(isDashing)
        {
            
            StartCoroutine(Dash());
            isDashing = false;
        }
    }

    private IEnumerator Dash()
    {
        newPitch = UnityEngine.Random.Range(0.9f,1f);
        dashAudioSource.pitch = newPitch;
        dashAudioSource.Play();
        // Déterminer la direction du dash en fonction des entrées du joueur
        dashMovement = new Vector2(horizontalInput, verticalInput).normalized;
        //GameObject playerGameObject = GetComponent<GameObject>();
        defaultSpeed = moveSpeed;
        moveSpeed += dashForce;
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        angleInRadians = Mathf.Atan2(dashMovement.y, dashMovement.x);
        angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        // 2. Calcul de l'angle en fonction du vecteur de mouvement
        if(angleInDegrees > 45 && angleInDegrees < 115)
        {
            if(angleInDegrees == 90)
            {
                if(direction == 1)
                {
                    rotation = Quaternion.Euler(0f, myTransform.rotation.y, angleInDegrees-90);
                }
                if(direction == -1)
                {
                    rotation = Quaternion.Euler(0f, myTransform.rotation.y + 180, angleInDegrees-90);
                }
                // myTransform.rotation = Quaternion.Lerp(myTransform.rotation, rotation, 0.3f);
                myTransform.rotation = rotation;
            } else
            {
                //playerRb.velocity = new Vector2(dashMovement.x * dashForce, dashMovement.y * dashForce);
                rotation = Quaternion.Euler(0f, 0f, angleInDegrees -90);
                myTransform.rotation = Quaternion.Lerp(myTransform.rotation, rotation, 0.3f);
            }
        } 
        if(angleInDegrees == -90)
        {
            if(direction == 1)
            {
                rotation = Quaternion.Euler(0f, myTransform.rotation.y, angleInDegrees+90);
            }
            if(direction == -1)
            {
                rotation = Quaternion.Euler(0f, myTransform.rotation.y + 180, angleInDegrees+90);
            }
            // myTransform.rotation = Quaternion.Lerp(myTransform.rotation, rotation, 0.3f);
            myTransform.rotation = rotation;

        }
        playerRb.AddForce(new Vector2(dashMovement.x * dashForce, dashMovement.y * dashForce * 1.33f),ForceMode2D.Impulse);       
        Physics2D.IgnoreLayerCollision(9,11,true);
        Physics2D.IgnoreLayerCollision(9,14,true);      
        //playerRb.gravityScale = 0;
        yield return new WaitForSeconds(dashDuration);
        //boxCollider.enabled = true,
        //playerRb.gravityScale = 1;
        

        Physics2D.IgnoreLayerCollision(9,11,false);
        // Arrêter le dash en réinitialisant la vélocité du joueur
        //playerRb.velocity = Vector2.zero;
        animator.SetBool("IsDashing", false);       
        isDashing = false;
        moveSpeed = defaultSpeed;
        

        if(direction == 1)
        {
            rotation = Quaternion.Euler(0f, myTransform.rotation.y, 0f);
        }
        if(direction == -1)
        {
            rotation = Quaternion.Euler(0f, myTransform.rotation.y + 180, 0f);
        }
        // myTransform.rotation = Quaternion.Lerp(myTransform.rotation, rotation, 0.3f);
        //jumpCounter = 1;
        //moveSpeed -= dashForce;
        StartCoroutine(ReduceSpeedGradually(defaultSpeed, speedReductionDuration));
        Physics2D.IgnoreLayerCollision(9,14,false);
    }

    private IEnumerator ReduceSpeedGradually(float targetSpeed, float duration)
    {
        if ( moveSpeed > targetSpeed)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed,duration);
            yield return null;
        }

    }

    private IEnumerator particlesForceField()
    {
        ForceFieldParticles.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        ForceFieldParticles.SetActive(false);
    }
    
    private void ChargeKi()
    {
        if(isCharging)
        {
            StartCoroutine(particlesForceField());
            StartCoroutine(KiChargingCo());
            
            isCharging = false;
        }
    }
    private void ReadChargeKi()
    {
        if(PlayerController.instance.playerInputActions.Player.KiExplosion.triggered  && Time.time > nextChargeTime)
        {
            StartCoroutine(chargeGruntAudio());
            animator.SetBool("IsCharging", true);
            animator.SetTrigger("ChargingTrigger");
            isCharging = true;
            nextChargeTime = Time.time + chargeCooldown;
            // StartCoroutine(DestroyDestructionGroundEffect());
            
        }
    }

    private IEnumerator KiChargingCo()
    {

        CameraShakeManager.instance.CameraShake(impulseSource);
        currentKi += 20;
        currentKi = Mathf.Clamp(currentKi, 0, maxKi);
        UpdateKiBar();
        // KiCharging.gameObject.SetActive(true);
        KiCharging2.gameObject.SetActive(true);
        //tileDestroyer.DestructionMouse();
        if (KiCharging2.gameObject.activeSelf)
        {
            newPitch = UnityEngine.Random.Range(0.9f,1f);
            audioSource.pitch = newPitch;
            audioSource.PlayOneShot(soundEffect, 0.1f);
            Instantiate(slamParticles, myTransform.position,Quaternion.identity);
        }
        if(currentKi < maxKi)
            StartCoroutine(DestroyChargeUpEffect());
        if(currentKi >= maxKi)
            StartCoroutine(DestroyChargeMaxEffect());
        playerRb.velocity = new Vector2(0f,0f);
        EnemiesStepBackCharging();
        yield return new WaitForSeconds(chargeDuration);
        
        // KiCharging.gameObject.SetActive(false);
        KiCharging2.gameObject.SetActive(false);
        audioSource.Stop();
        
        animator.SetBool("IsCharging", false);
        if(currentKi >= maxKi)
        {
            
            newPitch = 0.9f;
            audioSource.pitch = newPitch;
            audioSource.loop = true;
            audioSource.clip = KiAura;
            audioSource.volume = 0.05f;
            audioSource.Play();
            // KiCharging.gameObject.SetActive(true);
            KiCharging2.gameObject.SetActive(true);
            yield return new WaitForSeconds(10f);
            // KiCharging.gameObject.SetActive(false);
            KiCharging2.gameObject.SetActive(false);
            audioSource.Stop();
            audioSource.loop = false;
        }
          
    }

    public void UpdateKiBar()
    {
        targetKiFillAmount = currentKi / maxKi;
        KiBarFill.fillAmount = targetKiFillAmount;
    }

    public void EnemiesStepBackCharging()
    {
        
         // Détecter les ennemis dans la zone d'attaque
        colliders = Physics2D.OverlapCircleAll(myTransform.position, detectionBoxKi, enemyLayerMask);
        foreach (Collider2D collider in colliders)
        {
            enemyRb = collider.GetComponent<Rigidbody2D>();
            monsterHealth = collider.GetComponent<MonsterHealth>();
            if (enemyRb != null)
            {
                directionVector = ((Vector2)enemyRb.transform.position - (Vector2)myTransform.position).normalized;
                enemyRb.AddForce(directionVector * forceMagnitudeForward/0.75f, ForceMode2D.Impulse);
                // Infliger des dégâts aux ennemis
                if (monsterHealth != null)
                {
                    monsterHealth.TakeDamage(0f);
                }
            }
        }
    }

    



    private void SpawnDashParticles()
    {
        direction = myTransform.rotation.y == 0 ? 1 : -1;
        inputPlayer = new Vector2(horizontalInput, verticalInput).normalized;
        
        // 2. Calcul de l'angle en fonction du vecteur de mouvement
        
        angleInRadians = Mathf.Atan2(inputPlayer.y, inputPlayer.x);
        angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f + angleInDegrees + 180 : -180f + angleInDegrees);
        //trueRotation = Quaternion.Euler() 
        //Instantiate(dashParticles,new Vector2(transform.position.x, transform.position.y -2), rotation);
        Instantiate(dashExplosionParticles,myTransform.position, rotation);

    }
    

    private void SpawnJumpParticles()
    {
        rotation = Quaternion.Euler(0f, 0f, 90f);
        //trueRotation = Quaternion.Euler() 
        //Instantiate(dashParticles,new Vector2(transform.position.x, transform.position.y -2), rotation);
        Instantiate(dashExplosionParticles,myTransform.position, rotation);

    }











    private void InstantDash()
    {
         // Si le joueur appuie sur le bouton de dash et le dash est prêt
        if (Input.GetKeyDown(KeyCode.V) && Time.time > nextInstantDashTime)
        {
            animator.SetBool("IsInstantDashing", true);
            StartCoroutine(InstantDashCo());
            nextInstantDashTime = Time.time + instantDashCooldown; // Mettre à jour le temps de recharge du dash
            
        }
    }

    private IEnumerator InstantDashCo()
    {
        

        // Déterminer la direction du dash en fonction des entrées du joueur
        instantDashDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // Appliquer une force pour le dash
        newPosition = (Vector2)myTransform.position + instantDashDirection * instantDashForce;
        myTransform.position = newPosition;
        // Attendre la durée du dash
        yield return new WaitForSeconds(instantDashDuration);

        // Arrêter le dash en réinitialisant la vélocité du joueur
        playerRb.velocity = Vector2.zero;
        animator.SetBool("IsInstantDashing", false);
        jumpCounter = 1;
    }

    public void ThorTest()
    {
        thorHammer = GetComponent<ThorHammer>();
        if (Input.GetKeyDown(KeyCode.R))
                {
                    thorHammer.ReturnObject();
                }
    }


    public bool IsGrounded()
    {
        
        hit = Physics2D.Raycast(myTransform.position, Vector2.down, rayDistance, groundLayerMask);
         // Dessiner le raycast dans la vue de scène pour le débogage
        Debug.DrawRay(myTransform.position, Vector2.down * rayDistance, Color.red);
        if(hit.collider == null)
        {
            animator.SetBool("IsGrounded()", false);
            isGrounded = false;
            return hit.collider == null; 
        } else 
        {
            animator.SetBool("IsGrounded()", true);
            
            if(direction == 1)
            {
                rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            if(direction == -1)
            {
                rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            isGrounded = true;
            return hit.collider != null;   
        }
        
    }
    public IEnumerator DestroyDestructionGroundEffect()
    {
        
        destructionGroundObject = Instantiate(DestructionGroundEffect,new Vector2(myTransform.position.x, myTransform.position.y), Quaternion.Euler(0f,0f,0f));
        yield return new WaitForSeconds(1f);
        Destroy(destructionGroundObject);
        yield return null;
    }

    public IEnumerator DestroyChargeUpEffect()
    {
        comicBoom = Instantiate(ChargeUpEffect,new Vector2(myTransform.position.x, myTransform.position.y + 10f), Quaternion.identity);
        lighter = comicBoom.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(comicBoom);
        Destroy(lighter);
        
        yield return null;
    }
    public IEnumerator DestroyChargeMaxEffect()
    {
        chargeMax = Instantiate(chargeMaxEffect,new Vector2(myTransform.position.x, myTransform.position.y + 2f), Quaternion.identity);
        lighter = chargeMax.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(chargeMax);
        Destroy(lighter);
        
        yield return null;
    }

    public IEnumerator DestroyLevelUpEffect()
    {
        comicBoom = Instantiate(LevelUpEffect,new Vector2(myTransform.position.x, myTransform.position.y + 10f), Quaternion.identity);
        lighter = comicBoom.GetComponentInChildren<Light>();
        yield return new WaitForSeconds(0.3f);
        Destroy(comicBoom);
        Destroy(lighter);
        
        yield return null;
    }

    public IEnumerator chargeGruntAudio()
    {
        GameObject chargeGruntGameObject = Instantiate(chargeGruntPrefab, transform.position, Quaternion.identity);
        AudioSource chargeGruntAudioSource = chargeGruntGameObject.GetComponent<AudioSource>();
        if(chargeGruntAudioSource != null)
        {
            float newPitch = UnityEngine.Random.Range(0.9f, 1f);
            chargeGruntAudioSource.pitch = newPitch;
            chargeGruntAudioSource.Play();
            Destroy(chargeGruntGameObject, chargeGruntAudioSource.clip.length);
        }
        yield return null;
    }
}
