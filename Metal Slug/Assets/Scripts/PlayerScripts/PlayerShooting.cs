
using System.Collections;

using UnityEngine;
using Cinemachine;


public class PlayerShooting : MonoBehaviour
{
    // Référence au prefab de la boule d'énergie
    public GameObject energyBallPrefab;
    // Vitesse de la boule d'énergie
    public float energyBallSpeed = 5f;
    private Rigidbody2D rb;
    private Rigidbody2D energyrb;
    private Rigidbody2D energyrb2;
    private SpriteRenderer spriteRenderer;

    public float timeBtwAttacks = 3f;
    public float attackTimeCounter;
    private Animator animator;
    private PlayerMovement playerKi;
    private bool isShooting;
    private bool isShooting2;
    private bool isShooting3;
    private Vector2 shootDirection;
    private int direction;
    private float scaleMultiplier;
    private GameObject energyBall;
    private GameObject energyBall2;
    public GameObject energyBallPrefab2;
    public float energyBallSpeed2 = 5f;
    public float selfForceMagnitudeForward = 25f;
    public Vector3 offsetGenki;


    //public int damage = 10;
    //public MonsterHealth monsterHealth;
    public Vector3 offset = new Vector3(2.5f, 0f, 0f);

    private AudioSource audioSource;
    public AudioClip energySoundEffect;
    public float volumeSoundEffect = 0.25f;
    private CinemachineImpulseSource impulseSource;
    public float costUltimate = 20f;
    public float costEnergy = 5f;
    public float attackTimeCounter2;
    public float timeBtwAttacks2 = 10f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackTimeCounter = 0f;
        attackTimeCounter2 = 0f;
        animator = GetComponent<Animator>();
        playerKi = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        energyBall2 = GameObject.FindWithTag("EnergyBall2");
        energyBall2.SetActive(false);

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
        if (horizontalInput != 0 && mouseRight && attackTimeCounter <= 0f && cKi >= costEnergy || verticalInput != 0 && mouseRight && attackTimeCounter <= 0f && cKi >= costEnergy)
        {
            energyBall = Instantiate(energyBallPrefab, transform.position, Quaternion.identity);
            energyrb = energyBall.GetComponent<Rigidbody2D>();
            energyBall.transform.localScale *= scaleMultiplier;
            animator.SetTrigger("SimpleShootingTrigger");
            playerKi.currentKi -= costEnergy;
            playerKi.UpdateKiBar();
            isShooting = true;
            attackTimeCounter = timeBtwAttacks;

        } else if(horizontalInput == 0 && verticalInput == 0 && mouseRight && attackTimeCounter <= 0f && cKi >= costEnergy)
        {
            energyBall = Instantiate(energyBallPrefab, new Vector2(transform.position.x + (offset.x*direction), transform.position.y + offset.y), Quaternion.identity);
            energyrb = energyBall.GetComponent<Rigidbody2D>();
            energyBall.transform.localScale *= scaleMultiplier;
            animator.SetTrigger("SimpleShootingTrigger");
            playerKi.currentKi -= costEnergy;
            playerKi.UpdateKiBar();
            attackTimeCounter = timeBtwAttacks;
            isShooting2 = true;

        } else if(horizontalInput != 0 && Input.GetButtonDown("Fire2") && attackTimeCounter2 <= 0f  && cKi >= costUltimate || verticalInput != 0 && Input.GetButtonDown("Fire2") && attackTimeCounter2 <= 0f && cKi >= costUltimate)
        {
            //energyBall2 = Instantiate(energyBallPrefab2, transform.position + offsetGenki, Quaternion.identity);
            energyBall2.transform.position = new Vector3(transform.position.x + offsetGenki.x * horizontalInput, transform.position.y + offsetGenki.y * verticalInput, transform.position.z);
            energyBall2.SetActive(true);
            energyrb2 = energyBall2.GetComponent<Rigidbody2D>();
            energyBall2.transform.localScale = new Vector3(3f,3f,3f);
            energyBall2.transform.localScale *= scaleMultiplier;
            animator.SetTrigger("SimpleShootingTrigger");
            isShooting3 = true;
            attackTimeCounter2 = timeBtwAttacks2;
            playerKi.currentKi -= costUltimate;
            playerKi.UpdateKiBar();

        }

        if (attackTimeCounter >= 0f)
        {
            attackTimeCounter -= Time.deltaTime;
        }
        if (attackTimeCounter2 >= 0f)
        {
            attackTimeCounter2 -= Time.deltaTime;
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
        }else if(isShooting3)
        {
            
            StartCoroutine(Shooting3());
        }
    }

    private IEnumerator Shooting1()
    {
        audioSource.PlayOneShot(energySoundEffect, volumeSoundEffect);
        energyrb.velocity = shootDirection * energyBallSpeed;
        isShooting = false;
        yield return new WaitForSeconds(2f);
        //Destroy(energyBall.gameObject);
        yield return null;
    }
    private IEnumerator Shooting2()
    {
        audioSource.PlayOneShot(energySoundEffect, volumeSoundEffect);
        energyrb.velocity = direction * Vector2.right * energyBallSpeed;
        isShooting2 = false;
        yield return new WaitForSeconds(2f);
        //Destroy(energyBall.gameObject);
        yield return null;
    }
    private IEnumerator Shooting3()
    {
        CameraShakeManager.instance.CameraShake(impulseSource);
        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        playerRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);
        if(shootDirection!= null)
        {
            playerRb.AddForce(Vector2.right * selfForceMagnitudeForward/5 * -direction, ForceMode2D.Impulse);
            audioSource.PlayOneShot(energySoundEffect, volumeSoundEffect);
            energyrb2.AddForce(shootDirection * energyBallSpeed2, ForceMode2D.Impulse);
            CameraShakeManager.instance.CameraShake(impulseSource);
            isShooting3 = false;
            //energyBall2.transform.localScale = new Vector3(3,3,3);
            yield return new WaitForSeconds(1f);
            energyBall2.SetActive(false);

        }else if(shootDirection == null)
        {
            playerRb.AddForce(Vector2.right * selfForceMagnitudeForward/5 * -direction, ForceMode2D.Impulse);
            energyrb2.AddForce(direction * Vector2.right * energyBallSpeed2, ForceMode2D.Impulse);
            audioSource.PlayOneShot(energySoundEffect, volumeSoundEffect);
            CameraShakeManager.instance.CameraShake(impulseSource);
            isShooting3 = false;
            //energyBall2.transform.localScale = new Vector3(3,3,3);
            yield return new WaitForSeconds(1f);
            energyBall2.SetActive(false);

        }
        yield return null;
    }
    
}
