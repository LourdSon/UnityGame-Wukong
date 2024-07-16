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
    private SpriteRenderer spriteRenderer;

    public float timeBtwAttacks = 3f;
    public float attackTimeCounter;
    private Animator animator;


    //public int damage = 10;
    //public MonsterHealth monsterHealth;
    public Vector3 offset = new Vector3(2.5f, 0f, 0f);


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackTimeCounter = 0f;
        animator = GetComponent<Animator>();
        
    }

    // Update est appelée une fois par frame
    void Update()
    {
        Shooting();

    }

    public void Shooting()
    {
        // Obtient les valeurs des entrées horizontales et verticales
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool mouseRight = Input.GetButtonDown("Shooting");

        PlayerMovement playerKi = GetComponent<PlayerMovement>();
        float cKi = playerKi.currentKi;
        float mkI = playerKi.maxKi;
        float scaleMultiplier = 1 + (cKi / mkI);

        // Vérifie si l'entrée n'est pas nulle (si le joueur appuie sur les touches de direction)
        if (horizontalInput != 0 && mouseRight && attackTimeCounter <= 0f || verticalInput != 0 && mouseRight && attackTimeCounter <= 0f)
        {
            animator.SetTrigger("SimpleShootingTrigger");
            // Calcule la direction de tir en fonction des entrées horizontales et verticales
            Vector2 shootDirection = new Vector2(horizontalInput, verticalInput).normalized;
            // Crée une instance de la boule d'énergie
            GameObject energyBall = Instantiate(energyBallPrefab, transform.position, Quaternion.identity);
            energyBall.transform.localScale *= scaleMultiplier;
            // Obtient le composant Rigidbody2D de la boule d'énergie
            Rigidbody2D rb = energyBall.GetComponent<Rigidbody2D>();
            // Applique une force à la boule d'énergie dans la direction de tir
            rb.velocity = shootDirection * energyBallSpeed;
            attackTimeCounter = timeBtwAttacks;

        } else if(rb.velocity.x == 0 && mouseRight && attackTimeCounter <= 0f)
        {
            animator.SetTrigger("SimpleShootingTrigger");
            // Obtenir la direction actuelle du sprite du joueur
            int direction = spriteRenderer.flipX ? -1 : 1;
            // Crée une instance de la boule d'énergie
            GameObject energyBall = Instantiate(energyBallPrefab, new Vector3(transform.position.x + (offset.x*direction), transform.position.y + offset.y, transform.position.z), Quaternion.identity);
            energyBall.transform.localScale *= scaleMultiplier;
            // Obtient le composant Rigidbody2D de la boule d'énergie
            Rigidbody2D rb = energyBall.GetComponent<Rigidbody2D>();
            // Applique une force à la boule d'énergie dans la direction de tir sur l'axe x
            rb.velocity = direction * Vector2.right * energyBallSpeed;
            attackTimeCounter = timeBtwAttacks;

        }

        if (attackTimeCounter >= 0f)
        {
            attackTimeCounter -= Time.deltaTime;
        }
    }


    
}
#endif