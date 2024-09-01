


using UnityEngine;

public class PlayerDamage : MonoBehaviour
{

    public int damage = 30;
    public int multipUltim = 3;
    public ParticleSystem impactParticles;
    public float forceMagnitudeUpward;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "EnergyBall")
        {
            SpriteRenderer enemySr = GetComponentInChildren<SpriteRenderer>();
            MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
            monsterHealth.TakeDamage(damage);
            int direction = enemySr.flipX ? 1 : -1;
            Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            Instantiate(impactParticles, other.transform.position, rotation);
            Destroy(other.gameObject);

            
        } if(other.gameObject.tag == "EnergyBall2")
        {
            SpriteRenderer enemySr = GetComponentInChildren<SpriteRenderer>();
            Rigidbody2D enemyRb = GetComponentInChildren<Rigidbody2D>();
            MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
            monsterHealth.TakeDamage(damage*multipUltim);
            int direction = enemySr.flipX ? 1 : -1;
            Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            int upOrDown = Random.Range(-1,1);
            if (upOrDown == 0)
            { 
                upOrDown = -1;
            }
            enemyRb.AddForce(Vector2.up * forceMagnitudeUpward * upOrDown, ForceMode2D.Impulse);
            Instantiate(impactParticles, other.transform.position, rotation);

        }
        if(other.gameObject.tag == "EnergyBall3")
        {
            SpriteRenderer enemySr = GetComponentInChildren<SpriteRenderer>();
            MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
            monsterHealth.TakeDamage(damage);
            int direction = enemySr.flipX ? 1 : -1;
            Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            Instantiate(impactParticles, other.transform.position, rotation);
            other.gameObject.SetActive(false);
            //Destroy(other.gameObject);

            
        }
    }
}

