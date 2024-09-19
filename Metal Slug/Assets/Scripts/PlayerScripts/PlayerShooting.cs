
using System.Collections;

using UnityEngine;
using Cinemachine;


public class PlayerShooting : MonoBehaviour
{
    // Référence au prefab de la boule d'énergie
    public GameObject energyBallPrefab;
    // Vitesse de la boule d'énergie
    public float energyBallSpeed = 5f;
    private Rigidbody2D playerRb;
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
    private PlayerAttack playerAttack;
    public float costExplosion = 20f;
    public float attackTimeCounter3;
    public float timeBtwAttacks3 = 10f;
    public bool isShooting4;
    public float radius;
    public Rigidbody2D[] energyballrb3;
    private ObjectPool objectPool;
    public float delay = 0.25f;


    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackTimeCounter = 0f;
        attackTimeCounter2 = 0f;
        attackTimeCounter3 = 0f;
        animator = GetComponent<Animator>();
        playerKi = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        energyBall2 = GameObject.FindWithTag("EnergyBall2");
        energyBall2.SetActive(false);
        playerAttack = GetComponent<PlayerAttack>();
        objectPool = GetComponent<ObjectPool>();

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
        Vector2 moveInput = PlayerController.instance.playerInputActions.Player.Move.ReadValue<Vector2>();
        float horizontalInput = moveInput.x;
        float verticalInput = moveInput.y;
        bool mouseRight = PlayerController.instance.playerInputActions.Player.Shoot.triggered;
        direction = playerRb.transform.rotation.y == 0 ? 1 : -1;

        
        float cKi = playerKi.currentKi;
        float mkI = playerKi.maxKi;
        scaleMultiplier = 1 + (cKi / mkI);
        // Calcule la direction de tir en fonction des entrées horizontales et verticales
        shootDirection = new Vector2(horizontalInput, verticalInput).normalized;


        // Vérifie si l'entrée n'est pas nulle (si le joueur appuie sur les touches de direction)
        if (horizontalInput != 0 && mouseRight && attackTimeCounter <= 0f && cKi >= costEnergy && !playerAttack.isHoldingSelect|| verticalInput != 0 && mouseRight && attackTimeCounter <= 0f && cKi >= costEnergy && !playerAttack.isHoldingSelect)
        {
            energyBall = Instantiate(energyBallPrefab, transform.position, Quaternion.identity);
            energyrb = energyBall.GetComponent<Rigidbody2D>();
            energyBall.transform.localScale *= scaleMultiplier;
            animator.SetTrigger("SimpleShootingTrigger");
            playerKi.currentKi -= costEnergy;
            playerKi.UpdateKiBar();
            isShooting = true;
            attackTimeCounter = timeBtwAttacks;

        } else if(horizontalInput == 0 && verticalInput == 0 && mouseRight && attackTimeCounter <= 0f && cKi >= costEnergy && !playerAttack.isHoldingSelect)
        {
            energyBall = Instantiate(energyBallPrefab, new Vector2(transform.position.x + (offset.x*direction), transform.position.y + offset.y), Quaternion.identity);
            energyrb = energyBall.GetComponent<Rigidbody2D>();
            energyBall.transform.localScale *= scaleMultiplier;
            animator.SetTrigger("SimpleShootingTrigger");
            playerKi.currentKi -= costEnergy;
            playerKi.UpdateKiBar();
            attackTimeCounter = timeBtwAttacks;
            isShooting2 = true;

        } else if(horizontalInput != 0 && PlayerController.instance.playerInputActions.Player.SuperShot.triggered && attackTimeCounter2 <= 0f  && cKi >= costUltimate || verticalInput != 0 && PlayerController.instance.playerInputActions.Player.SuperShot.triggered && attackTimeCounter2 <= 0f && cKi >= costUltimate)
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

        } else if(playerAttack.holdingTime >= playerAttack.requiredHoldingTime && mouseRight && attackTimeCounter3 <= 0f && cKi >= costExplosion)
        {
            int numberOfBalls = 10; // Nombre de EnergyBalls à instancier
            int setsOfBalls = 5;
            
            
            StartCoroutine(SpawnAndShootEnergyBalls(numberOfBalls,setsOfBalls,delay));
            
            animator.SetTrigger("SimpleShootingTrigger");
            playerKi.currentKi -= costExplosion;
            playerKi.UpdateKiBar();
            attackTimeCounter3 = timeBtwAttacks3;
            isShooting4 = true;
        }

        if (attackTimeCounter >= 0f)
        {
            attackTimeCounter -= Time.deltaTime;
        }
        if (attackTimeCounter2 >= 0f)
        {
            attackTimeCounter2 -= Time.deltaTime;
        }
        if (attackTimeCounter3 >= 0f)
        {
            attackTimeCounter3 -= Time.deltaTime;
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
        float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(energySoundEffect, volumeSoundEffect);
        energyrb.velocity = shootDirection * energyBallSpeed;
        isShooting = false;
        yield return new WaitForSeconds(2f);
        //Destroy(energyBall.gameObject);
        yield return null;
    }
    private IEnumerator Shooting2()
    {
        float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
        audioSource.pitch = newPitch;
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
            float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
            audioSource.pitch = newPitch;
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
            float newPitch = UnityEngine.Random.Range(0.8f,1.2f);
            audioSource.pitch = newPitch;
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

    
    private IEnumerator SpawnAndShootEnergyBalls(int numberOfBalls, int setsOfBalls, float delay)
    {
        float angleStep = 360f / numberOfBalls; // L'angle entre chaque EnergyBall
        int totalBalls = numberOfBalls * setsOfBalls;
        energyballrb3 = new Rigidbody2D[totalBalls]; // Initialiser le tableau avec la bonne taille
        int index = 0;
        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        for (int j = 0; j < setsOfBalls; j++)
        {
            
            CameraShakeManager.instance.CameraShake(impulseSource);
            playerKi.EnemiesStepBackCharging();
            playerRb.velocity = Vector2.zero;
            // Générer un ensemble de boules d'énergie
            for (int i = 0; i < numberOfBalls; i++)
            {
                float angle = i * angleStep;
                Vector3 position = GetPositionAroundHero(angle);
                GameObject pooledEnergyBall = objectPool.GetPooledObject(); // Obtenir un objet du pool
                pooledEnergyBall.transform.position = position;
                pooledEnergyBall.transform.rotation = Quaternion.identity;
                pooledEnergyBall.transform.localScale = Vector3.one * scaleMultiplier;
                pooledEnergyBall.SetActive(true); // Activer l'objet
                energyballrb3[index] = pooledEnergyBall.GetComponent<Rigidbody2D>(); // Stocker chaque Rigidbody2D dans le tableau
                index++;
            }

            // Projeter l'ensemble de boules d'énergie
            for (int i = j * numberOfBalls; i < (j + 1) * numberOfBalls; i++)
            {
                Vector2 directionVector = ((Vector2)energyballrb3[i].transform.position - (Vector2)transform.position).normalized;
                energyballrb3[i].velocity = directionVector * energyBallSpeed;
            }

            // Attendre avant de passer à l'ensemble suivant
            yield return new WaitForSeconds(delay);
        }

        isShooting4 = false; // Réinitialiser l'état de tir
    }
    public Vector3 GetPositionAroundHero(float angle)
    {
        // Convertir l'angle en radians
        float angleRad = angle * Mathf.Deg2Rad;

        // Calculer la position autour du héros
        float x = transform.position.x + Mathf.Cos(angleRad) * radius;
        float y = transform.position.y + Mathf.Sin(angleRad) * radius;

        // Retourner la nouvelle position avec la même hauteur que le héros
        return new Vector3(x, y, transform.position.z);
    }
    
}
