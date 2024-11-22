using UnityEngine;

public class EnemyDamageShooting : MonoBehaviour
{
    public int damage = 10;
    public ParticleSystem impactParticles;
    public int force = 10;

    private PlayerHealth playerHealth;
    private EnergyBallDestroyEffectScript destroyEffect;
    private Vector2 directionVector;
    private Transform myTransform;
    private Rigidbody2D rb;

    void Start()
    {
        myTransform = transform;
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnergyBallEnemy"))
        {
            // Appliquer les dégâts au joueur
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Obtenir l’effet de destruction et l’activer
            destroyEffect = other.GetComponent<EnergyBallDestroyEffectScript>();
            if (destroyEffect != null)
            {
                destroyEffect.OnInstanceDestroyParticle(other.gameObject);
            }

            // Activer les particules d’impact
            Instantiate(impactParticles, other.transform.position, Quaternion.identity);

            // Désactiver la boule d'énergie
            other.gameObject.SetActive(false);

            // Calculer et appliquer la force de recul
            directionVector = (myTransform.position - other.transform.position).normalized;
            rb.AddForce(-directionVector * force, ForceMode2D.Impulse);
        }
    }
}
