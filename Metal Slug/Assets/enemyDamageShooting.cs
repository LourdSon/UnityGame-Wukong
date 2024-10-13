


using UnityEngine;

public class EnemyDamageShooting : MonoBehaviour
{

    public int damage = 10;
    public ParticleSystem impactParticles;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "EnergyBallEnemy")
        {
            
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            int direction = transform.rotation.y == 0f ? 1 : -1;
            Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0 ? 0f : 180f);
            EnergyBallDestroyEffectScript destroyEffect = other.GetComponent<EnergyBallDestroyEffectScript>();
            destroyEffect.OnInstanceDestroy(other.gameObject);
            Instantiate(impactParticles, other.transform.position, Quaternion.identity);
            // Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        } 
    }
}

