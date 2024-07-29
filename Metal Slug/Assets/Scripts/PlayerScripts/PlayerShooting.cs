#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Référence au prefab de la boule d'énergie
    public GameObject energyBallPrefab;

    // Vitesse de la boule d'énergie
    public float energyBallSpeed = 5f;
    private Rigidbody2D rb;
    private Rigidbody2D energyrb;
    private SpriteRenderer spriteRenderer;

    public float timeBtwAttacks = 3f;
    public float attackTimeCounter;
    private Animator animator;
    private PlayerMovement playerKi;
    private bool isShooting;
    private bool isShooting2;
    private Vector2 shootDirection;
    private int direction;
    private float scaleMultiplier;
    private GameObject energyBall;


    //public int damage = 10;
    //public MonsterHealth monsterHealth;
    public Vector3 offset = new Vector3(2.5f, 0f, 0f);


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackTimeCounter = 0f;
        animator = GetComponent<Animator>();
        playerKi = GetComponent<PlayerMovement>();

        // Obtenir la direction actuelle du sprite du joueur
    }

    // Update est appelée une fois par frame
    void Update()
    {
        ReadInputShooting();

    }
    void FixedUpdate()
    {
        Shooting();
    }

    public void ReadInputShooting()
    {
        // Obtient les valeurs des entrées horizontales et verticales
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool mouseRight = Input.GetButtonDown("Shooting");
        direction = spriteRenderer.flipX ? -1 : 1;

        
        float cKi = playerKi.currentKi;
        float mkI = playerKi.maxKi;
        scaleMultiplier = 1 + (cKi / mkI);
        // Calcule la direction de tir en fonction des entrées horizontales et verticales
        shootDirection = new Vector2(horizontalInput, verticalInput).normalized;


        // Vérifie si l'entrée n'est pas nulle (si le joueur appuie sur les touches de direction)
        if (horizontalInput != 0 && mouseRight && attackTimeCounter <= 0f || verticalInput != 0 && mouseRight && attackTimeCounter <= 0f)
        {
            energyBall = Instantiate(energyBallPrefab, transform.position, Quaternion.identity);
            energyrb = energyBall.GetComponent<Rigidbody2D>();
            energyBall.transform.localScale *= scaleMultiplier;
            animator.SetTrigger("SimpleShootingTrigger");
            isShooting = true;
            attackTimeCounter = timeBtwAttacks;

        } else if(horizontalInput == 0 && verticalInput == 0 && mouseRight && attackTimeCounter <= 0f)
        {
            energyBall = Instantiate(energyBallPrefab, new Vector2(transform.position.x + (offset.x*direction), transform.position.y + offset.y), Quaternion.identity);
            energyrb = energyBall.GetComponent<Rigidbody2D>();
            energyBall.transform.localScale *= scaleMultiplier;
            animator.SetTrigger("SimpleShootingTrigger");
            attackTimeCounter = timeBtwAttacks;
            isShooting2 = true;

        }

        if (attackTimeCounter >= 0f)
        {
            attackTimeCounter -= Time.deltaTime;
        }
    }
    private void Shooting()
    {     
        if (isShooting)
        {
            StartCoroutine(Shooting1());
        } else if(isShooting2)
        {
            
            StartCoroutine(Shooting2());
        }
    }

    private IEnumerator Shooting1()
    {
        energyrb.velocity = shootDirection * energyBallSpeed;
        isShooting = false;
        yield return null;
    }
    private IEnumerator Shooting2()
    {
        energyrb.velocity = direction * Vector2.right * energyBallSpeed;
        isShooting2 = false;
        yield return null;
    }
    
}
#endif