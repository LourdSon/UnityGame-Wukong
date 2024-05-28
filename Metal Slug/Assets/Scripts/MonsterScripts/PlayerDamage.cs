using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{

    public int damage = 30;
    public ParticleSystem impactParticles;
    
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
            SpriteRenderer enemyRb = GetComponent<SpriteRenderer>();
            MonsterHealth monsterHealth = GetComponent<MonsterHealth>();
            monsterHealth.TakeDamage(damage);
            int direction = enemyRb.flipX ? 1 : -1;
            Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            Instantiate(impactParticles,other.transform.position, rotation);
            Destroy(other.gameObject);

            
        }
    }
}
