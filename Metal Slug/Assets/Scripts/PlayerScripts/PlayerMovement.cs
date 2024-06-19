
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngineInternal;
using UnityEngine.UI;
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
    private ParticleSystem dashParticlesInstance;

    public float detectionBoxKi =15f;
    public LayerMask enemyLayerMask;
    public float forceMagnitudeForward = 10f;

    public TileDestroyer tileDestroyer;
    

    
    

    
    

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();


        Physics2D.gravity *= gravityModifier;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentKi = 0f;
        UpdateKiBar();

        impulseSource = GetComponent<CinemachineImpulseSource>();

        tileDestroyer = GetComponentInChildren<TileDestroyer>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerRb!= null)
        {
            
            MovePlayer();
            Jump();
            ResetPosition(); // P
            DashPress(); // Q
            ChargeKi(); // E
            InstantDash(); // V
            ThorTest();
        } 
        //PunchAttack();
            
    }



    private void MovePlayer()
    {
        
            //Get Axis from Unity
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            //Move the character
            //transform.Translate(Vector2.right * Time.deltaTime * moveSpeed * horizontalInput);
            //playerRb.AddForce(Vector2.right * horizontalInput * moveSpeed);

            float horizontalMove = horizontalInput * moveSpeed;
            Vector2 movement = new Vector2(horizontalMove, playerRb.velocity.y);
            playerRb.velocity = movement;
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


    private void PunchAttack()
    {
        if (Input.GetKeyDown(KeyCode.T) && isGrounded && Time.time > nextPunchAttackTime)
        {
            
            StartCoroutine(PunchCo());
            nextPunchAttackTime = Time.time + punchAttackCooldown;

        }
    }


    private IEnumerator PunchCo()
    {
        animator.SetBool("IsPunching", true);
        yield return new WaitForSeconds(punchAttackDuration);
        animator.SetBool("IsPunching", false);
    }


    private void Jump()
    {
        //Jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            CameraShakeManager.instance.CameraShake(impulseSource);
            Vector2 jumpVelocity = new Vector2(playerRb.velocity.x, jumpForce);
            playerRb.velocity = Vector2.Lerp(playerRb.velocity, jumpVelocity, jumpCurve.Evaluate(jumpTime));
            animator.SetBool("IsJumping", true);
            animator.SetTrigger("JumpingTrigger");
            //float time = Map(playerRb.velocity.y, jumpForce, -jumpForce, 0, 1, true);
            //animator.Play("Jumping", 0, time);
            //animator.speed = 0;   
                        
            jumpCounter -=1;         
            
            if (jumpCounter == 0)
            {   
                isGrounded = false;
                jumpCounter = 2;   
            }

        }
    }

    private float Map(float y, float jumpForce, float v1, int v2, int v3, bool v4)
    {
        throw new NotImplementedException();
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
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCounter = 2;
            animator.SetBool("IsJumping", false);
            
        }
    }


    
    private void DashPress()
    {
        // Si le joueur appuie sur le bouton de dash et le dash est prêt
        if (Input.GetAxis("Dash") == 1  && Time.time > nextDashTime)
        {
            isDashing = true;
            animator.SetBool("IsDashing", true);
            StartCoroutine(Dash());
            nextDashTime = Time.time + dashCooldown; // Mettre à jour le temps de recharge du dash
            
        }
    }

    private IEnumerator Dash()
    {


        CameraShakeManager.instance.CameraShake(impulseSource);
        SpawnDashParticles();
        // Déterminer la direction du dash en fonction des entrées du joueur
        Vector2 dashMovement = new Vector2(horizontalInput, verticalInput);
        float defaultSpeed = moveSpeed;
        moveSpeed += dashForce;

        playerRb.velocity = new Vector2(dashMovement.x * dashForce, dashMovement.y * dashForce/2);
        //playerRb.AddForce(dashMovement * dashForce,ForceMode2D.Impulse);
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        
        yield return new WaitForSeconds(dashDuration);
        boxCollider.enabled = true;

        // Arrêter le dash en réinitialisant la vélocité du joueur
        //playerRb.velocity = Vector2.zero;
        animator.SetBool("IsDashing", false);

        
        isDashing = false;
        jumpCounter = 1;
        //moveSpeed -= dashForce;
        
        StartCoroutine(ReduceSpeedGradually(defaultSpeed, speedReductionDuration));
        
    }

    private IEnumerator ReduceSpeedGradually(float targetSpeed, float duration)
    {
        float startSpeed = moveSpeed;
        float time = 0;

        while (time < duration)
        {
            moveSpeed = Mathf.Lerp(startSpeed, targetSpeed, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        moveSpeed = targetSpeed;
    }


    private void ChargeKi()
    {
        if(Input.GetAxisRaw("Charging") == 1  && Time.time > nextChargeTime && isGrounded)
        {
            animator.SetBool("IsCharging", true);
            animator.SetTrigger("ChargingTrigger");
            StartCoroutine(KiChargingCo());
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
            audioSource.PlayOneShot(soundEffect, 0.25f);
        }
        playerRb.velocity = new Vector2(0f,0f);
        EnemiesStepBackCharging();
        yield return new WaitForSeconds(chargeDuration);
        KiCharging.gameObject.SetActive(false);
        audioSource.Stop();
        
        animator.SetBool("IsCharging", false);
        if(currentKi >= maxKi)
        {
            audioSource.loop = true;
            audioSource.clip = KiAura;
            audioSource.volume = 0.25f;
            audioSource.Play();
            KiCharging.gameObject.SetActive(true);
            yield return new WaitForSeconds(100f);
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

    private void EnemiesStepBackCharging()
    {
        // Obtenir la direction actuelle du sprite du joueur
        

         // Détecter les ennemis dans la zone d'attaque
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionBoxKi, enemyLayerMask);
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
        Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);  
        dashParticlesInstance = Instantiate(dashParticles,transform.position, rotation);
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
