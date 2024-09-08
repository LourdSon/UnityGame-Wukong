
using System;
using System.Collections;

using UnityEngine;

using Cinemachine;



public class PlayerMovement : MonoBehaviour
{

    [Header("Horizontal movement")]
    //Move Variables
    public float moveSpeed = 10.0f;
    private float horizontalInput;
    private float verticalInput;

    


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
    public float chargeDuration = 1.3f; // La durée du dash
    public float chargeCooldown = 1f; // Le temps de recharge du dash
    private float nextChargeTime;
    public float maxKi = 100f;
    public float currentKi = 0f;
    public UnityEngine.UI.Image KiBarFill;


    [Header("Audio Source")]
    private AudioSource audioSource;
    public AudioClip soundEffect;
    public AudioClip KiAura;



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
    private bool isCharging;

    public LayerMask playerLayerMask;
    public float detectionRadius = 5f;
    Material material;

    public Quaternion rotation;
    public Quaternion trueRotation;
    public ParticleSystem slamParticles;

    private PlayerAttack playerAttack;
    

    
    

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

        //tileDestroyer = GetComponentInChildren<TileDestroyer>();
        
        
        
    }


    void Update()
    {
        if(!EndMission.isCutsceneon)  
        {  
            ReadInputMove();
            ReadInputJump();
            ReadDashPress();
            ReadChargeKi();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!EndMission.isCutsceneon)  
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

    private void ReadInputMove()
    {
        if(isDashing == false)
        {
            //Get Axis from Unity
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            float horizontalMove = horizontalInput * moveSpeed;
            movement = new Vector2(horizontalMove, verticalInput);
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            //Flip sprite
            if (horizontalInput != 0)
            {
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsIdling", false);
                if (horizontalInput < 0)
                {
                    spriteRenderer.flipX = true;
                } else if (horizontalInput > 0)
                {
                    spriteRenderer.flipX = false;
                }
            } else
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsIdling", true);
            }
        }
    }

    private void MovePlayer()
    {
        
            //Move the character
            transform.Translate(Vector2.right * Time.deltaTime * moveSpeed * horizontalInput);
            //playerRb.AddForce(Vector2.right * movement.x);
            //playerRb.velocity = new Vector2(movement.x, playerRb.velocity.y);       
            //playerRb.MovePosition((Vector2) transform.position + movement * Time.deltaTime);
        
    
    }


    private void Jump()
    {
        if (jumpBool)
        {
            Physics2D.IgnoreLayerCollision(9,14,true);
            playerRb.velocity = Vector2.Lerp(playerRb.velocity, jumpVelocity, jumpCurve.Evaluate(jumpTime));
            jumpBool = false;
            
        }
        
    }
    private void ReadInputJump()
    {
        //Jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpVelocity = new Vector2(playerRb.velocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
            animator.SetTrigger("JumpingTrigger");     
            jumpBool = true;                    
            jumpCounter -=1;                    
            if (jumpCounter == 0)
            {   
                isGrounded = false;
                jumpCounter = 2;
            }
        }
    }




    private void ResetPosition()
    {
        //Reset Player position
        if(Input.GetKeyDown(KeyCode.P))
        {
            transform.position = new Vector2(20,2);
        }
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Roofs"))
        {
            isGrounded = true;
            jumpCounter = 2;
            animator.SetBool("IsJumping", false);
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
    private void ReadDashPress()
    {
        // Si le joueur appuie sur le bouton de dash et le dash est prêt
        if (Input.GetAxis("Dash") == 1  && Time.time > nextDashTime)
        {
            SpawnDashParticles();
            isGrounded=true;
            jumpCounter = 1;
            isDashing = true;
            animator.SetBool("IsDashing", true);
            nextDashTime = Time.time + dashCooldown; // Mettre à jour le temps de recharge du dash
        }
    }

    private IEnumerator Dash()
    {
        
        
        
        // Déterminer la direction du dash en fonction des entrées du joueur
        Vector2 dashMovement = new Vector2(horizontalInput, verticalInput).normalized;
        SpriteRenderer playerSpriteRenderer = GetComponent<SpriteRenderer>();
        //GameObject playerGameObject = GetComponent<GameObject>();
        float defaultSpeed = moveSpeed;
        moveSpeed += dashForce;
        int direction = playerSpriteRenderer.flipX ? 1 : -1;
        float angleInRadians = Mathf.Atan2(dashMovement.y, dashMovement.x);
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        
        // 2. Calcul de l'angle en fonction du vecteur de mouvement
        if(angleInDegrees > 45 && angleInDegrees < 115)
        {
            
            
            //playerRb.velocity = new Vector2(dashMovement.x * dashForce, dashMovement.y * dashForce);
            rotation = Quaternion.Euler(0f, 0f, angleInDegrees -90);
            playerRb.transform.rotation = rotation;
        }
        playerRb.AddForce(dashMovement * dashForce,ForceMode2D.Impulse);       
        Physics2D.IgnoreLayerCollision(9,11,true);
        Physics2D.IgnoreLayerCollision(9,14,true);      
        //playerRb.gravityScale = 0;
        
        yield return new WaitForSeconds(dashDuration);
        //boxCollider.enabled = true,
        //playerRb.gravityScale = 1;
        

        Physics2D.IgnoreLayerCollision(9,11,false);
        Physics2D.IgnoreLayerCollision(9,14,false);
        // Arrêter le dash en réinitialisant la vélocité du joueur
        //playerRb.velocity = Vector2.zero;
        animator.SetBool("IsDashing", false);       
        isDashing = false;
        rotation = Quaternion.Euler(0f, 0f, 0f);
        playerRb.transform.rotation = rotation;
        //jumpCounter = 1;
        //moveSpeed -= dashForce;
        StartCoroutine(ReduceSpeedGradually(defaultSpeed, speedReductionDuration));
        
    }

    private IEnumerator ReduceSpeedGradually(float targetSpeed, float duration)
    {
        float startSpeed = moveSpeed;
        //float time = 1;

        if (/*time < duration &&*/ moveSpeed > targetSpeed)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, /*time /*/ duration);
            //time += Time.deltaTime;
            yield return null;
        }

        //moveSpeed = targetSpeed;
    }

    private void ChargeKi()
    {
        if(isCharging)
        {
            StartCoroutine(KiChargingCo());
            isCharging = false;
        }
    }
    private void ReadChargeKi()
    {
        if(Input.GetAxisRaw("Charging") == 1  && Time.time > nextChargeTime)
        {
            animator.SetBool("IsCharging", true);
            animator.SetTrigger("ChargingTrigger");
            isCharging = true;
            nextChargeTime = Time.time + chargeCooldown;
            
        }
    }

    private IEnumerator KiChargingCo()
    {
        CameraShakeManager.instance.CameraShake(impulseSource);
        currentKi += 20;
        currentKi = Mathf.Clamp(currentKi, 0, maxKi);
        UpdateKiBar();
        KiCharging.gameObject.SetActive(true);
        //tileDestroyer.DestructionMouse();
        if (KiCharging.gameObject.activeSelf)
        {
            float newPitch = UnityEngine.Random.Range(0.9f,1.1f);
            audioSource.pitch = newPitch;
            audioSource.PlayOneShot(soundEffect, 0.1f);
            Instantiate(slamParticles,transform.position,Quaternion.identity);
        }
        playerRb.velocity = new Vector2(0f,0f);
        EnemiesStepBackCharging();
        yield return new WaitForSeconds(chargeDuration);
        KiCharging.gameObject.SetActive(false);
        audioSource.Stop();
        
        animator.SetBool("IsCharging", false);
        if(currentKi >= maxKi)
        {
            float newPitch = UnityEngine.Random.Range(0.9f,1.1f);
            audioSource.pitch = newPitch;
            audioSource.loop = true;
            audioSource.clip = KiAura;
            audioSource.volume = 0.05f;
            audioSource.Play();
            KiCharging.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            KiCharging.gameObject.SetActive(false);
            audioSource.Stop();
            audioSource.loop = false;
        }
          
    }

    public void UpdateKiBar()
    {
        float targetKiFillAmount = currentKi / maxKi;
        KiBarFill.fillAmount = targetKiFillAmount;
    }

    public void EnemiesStepBackCharging()
    {
        
         // Détecter les ennemis dans la zone d'attaque
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionBoxKi, enemyLayerMask);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)transform.position).normalized;
                enemyRb.AddForce(directionVector * forceMagnitudeForward/0.75f, ForceMode2D.Impulse);
                // Infliger des dégâts aux ennemis
                MonsterHealth monsterHealth = collider.GetComponent<MonsterHealth>();
                if (monsterHealth != null)
                {
                    monsterHealth.TakeDamage(0f);
                }
            }
        }
    }

    



    private void SpawnDashParticles()
    {
        GameObject player = GameObject.FindWithTag("Player");
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        int direction = playerSpriteRenderer.flipX ? 1 : -1;
        Vector2 inputPlayer = new Vector2(horizontalInput, verticalInput).normalized;
        
        // 2. Calcul de l'angle en fonction du vecteur de mouvement
        
        float angleInRadians = Mathf.Atan2(inputPlayer.y, inputPlayer.x);
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f + angleInDegrees + 180 : 180f + angleInDegrees);
        //trueRotation = Quaternion.Euler() 
        //Instantiate(dashParticles,new Vector2(transform.position.x, transform.position.y -2), rotation);
        Instantiate(dashExplosionParticles,transform.position, rotation);

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
        Vector2 instantDashDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // Appliquer une force pour le dash
        Vector2 newPosition = (Vector2)transform.position + instantDashDirection * instantDashForce;
        transform.position = newPosition;
        // Attendre la durée du dash
        yield return new WaitForSeconds(instantDashDuration);

        // Arrêter le dash en réinitialisant la vélocité du joueur
        playerRb.velocity = Vector2.zero;
        animator.SetBool("IsInstantDashing", false);
        jumpCounter = 1;
    }

    public void ThorTest()
    {
        ThorHammer thorHammer = GetComponent<ThorHammer>();
        if (Input.GetKeyDown(KeyCode.R))
                {
                    thorHammer.ReturnObject();
                }
    }

}
