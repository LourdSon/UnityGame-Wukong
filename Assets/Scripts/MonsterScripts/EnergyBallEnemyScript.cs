using UnityEngine;

public class EnergyBallEnemyScript : MonoBehaviour
{
    public float speed = 20f;
    private Vector2 direction;
    private EnergyBallDestroyEffectScript destroyEffect;
    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
        destroyEffect = GetComponent<EnergyBallDestroyEffectScript>();
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;  // Normalise la direction
    }

    void FixedUpdate()
    {
        // Déplace la boule d'énergie dans la direction définie
        myTransform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si la collision est avec une autre "EnergyBall" ou le "Ground"
        if (other.CompareTag("EnergyBall") || other.CompareTag("EnergyBall2") || other.CompareTag("EnergyBall3"))
        {
            // Récupère et active l'effet de destruction pour les deux objets
            EnergyBallDestroyEffectScript otherDestroyEffect = other.GetComponent<EnergyBallDestroyEffectScript>();
            if (otherDestroyEffect != null)
            {
                otherDestroyEffect.OnInstanceDestroyParticle(other.gameObject);
            }

            if (destroyEffect != null)
            {
                destroyEffect.OnInstanceDestroyParticle(gameObject);
            }
            if (other.CompareTag("EnergyBall") || other.CompareTag("EnergyBall3"))
            {
                if (other.gameObject != null) // Vérifie si l'objet n'est pas null avant de le désactiver
                {
                    other.gameObject.SetActive(false);
                }
            }
            // Désactive les objets de la collision
            if (gameObject != null) // Vérifie si l'objet n'est pas null avant de le désactiver
            {
                gameObject.SetActive(false);
            }
        }
        else if (other.CompareTag("Ground"))
        {
            // Détruit seulement cette boule d'énergie si elle touche le sol
            if (destroyEffect != null)
            {
                destroyEffect.OnInstanceDestroyParticle(gameObject);
            }
            if (gameObject != null) // Vérifie si l'objet n'est pas null avant de le désactiver
            {
                gameObject.SetActive(false);
            }
        }
    }
}
