


using UnityEngine;

public class PlayerDamage : MonoBehaviour
{

    public int damage = 30;
    public int multipUltim = 3;
    public ParticleSystem impactParticles;
    public float forceMagnitudeUpward;
    public int multipBeerus = 3;
    public GameObject comicBoomEffect;
    public PlayerLevel playerLevel;
    public bool damageIncreased;




    private SpriteRenderer enemySr;
    private MonsterHealth monsterHealth;
    private int direction;
    private Quaternion rotation;
    private EnergyBallDestroyEffectScript destroyEffect;
    private Rigidbody2D enemyRb;
    private Vector2 directionVector;
    private Transform myTransform;
    
    


    // Start is called before the first frame update
    void Start()
    {
        enemySr = GetComponentInChildren<SpriteRenderer>();
        monsterHealth = GetComponent<MonsterHealth>();
        enemyRb = GetComponentInChildren<Rigidbody2D>();
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerLevel != null)
        {
            moreLevelMoreDamage();
        }
    }
    public void moreLevelMoreDamage()
    {
        
        if(playerLevel.isLevelingUp && !damageIncreased)
        {
            damage += damage/5;
            damageIncreased = true;
        }else if (!playerLevel.isLevelingUp)
        {
            damageIncreased = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "EnergyBall" && tag == "Enemy")
        {
            monsterHealth.TakeDamage2(damage);
            direction = myTransform.rotation.y == 0f ? 1 : -1;
            rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            destroyEffect = other.GetComponent<EnergyBallDestroyEffectScript>();
            destroyEffect.OnInstanceDestroyParticle(other.gameObject);
            Instantiate(impactParticles, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);

            
        } if(other.gameObject.tag == "EnergyBall2" && tag == "Enemy")
        {
            monsterHealth.TakeDamage(damage*multipUltim);
            direction = myTransform.rotation.y == 0f ? 1 : -1;
            rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            directionVector = ((Vector2)enemyRb.transform.position - (Vector2)other.transform.position).normalized;
            enemyRb.AddForce(directionVector * forceMagnitudeUpward, ForceMode2D.Impulse);
            Instantiate(impactParticles, other.transform.position, rotation);
            Instantiate(comicBoomEffect, other.transform.position, Quaternion.identity);

        }
        if(other.gameObject.tag == "EnergyBall3" && tag == "Enemy")
        {
            monsterHealth.TakeDamage(damage*multipBeerus);
            direction = myTransform.rotation.y == 0f ? 1 : -1;
            rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            Instantiate(impactParticles, other.transform.position, rotation);
            destroyEffect = other.GetComponent<EnergyBallDestroyEffectScript>();
            destroyEffect.OnInstanceDestroyParticle(other.gameObject);
            other.gameObject.SetActive(false);
            Instantiate(comicBoomEffect, other.transform.position, Quaternion.identity);
            //Destroy(other.gameObject);
        }
    }
}

