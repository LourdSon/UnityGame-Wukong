


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


    // Start is called before the first frame update
    void Start()
    {
       
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
            damage += damage/10;
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
            SpriteRenderer enemySr = GetComponentInChildren<SpriteRenderer>();
            MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
            monsterHealth.TakeDamage2(damage);
            int direction = transform.rotation.y == 0f ? 1 : -1;
            Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            EnergyBallDestroyEffectScript destroyEffect = other.GetComponent<EnergyBallDestroyEffectScript>();
            destroyEffect.OnInstanceDestroy(other.gameObject);
            Instantiate(impactParticles, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);

            
        } if(other.gameObject.tag == "EnergyBall2" && tag == "Enemy")
        {
            SpriteRenderer enemySr = GetComponentInChildren<SpriteRenderer>();
            Rigidbody2D enemyRb = GetComponentInChildren<Rigidbody2D>();
            MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
            monsterHealth.TakeDamage(damage*multipUltim);
            int direction = transform.rotation.y == 0f ? 1 : -1;
            Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            
            Vector2 directionVector = ((Vector2)enemyRb.transform.position - (Vector2)other.transform.position).normalized;
            enemyRb.AddForce(directionVector * forceMagnitudeUpward, ForceMode2D.Impulse);
            Instantiate(impactParticles, other.transform.position, rotation);
            Instantiate(comicBoomEffect, other.transform.position, Quaternion.identity);

        }
        if(other.gameObject.tag == "EnergyBall3" && tag == "Enemy")
        {
            SpriteRenderer enemySr = GetComponentInChildren<SpriteRenderer>();
            MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
            monsterHealth.TakeDamage(damage*multipBeerus);
            int direction = transform.rotation.y == 0f ? 1 : -1;
            Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            Instantiate(impactParticles, other.transform.position, rotation);
            EnergyBallDestroyEffectScript destroyEffect = other.GetComponent<EnergyBallDestroyEffectScript>();
            destroyEffect.OnInstanceDestroy(other.gameObject);
            other.gameObject.SetActive(false);
            Instantiate(comicBoomEffect, other.transform.position, Quaternion.identity);
            //Destroy(other.gameObject);

            
        }
    }
}

