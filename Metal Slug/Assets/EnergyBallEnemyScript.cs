using UnityEngine;

public class EnergyBallEnemyScript : MonoBehaviour
{
    public float speed = 20f;
    private Vector2 direction;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;  // Normalise la direction pour éviter des vitesses différentes
    }

    void FixedUpdate()
    {
        // Déplacer la boule d'énergie dans la direction définie
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("EnergyBall") || other.gameObject.CompareTag("EnergyBall3"))
        {
            EnergyBallDestroyEffectScript destroyEffect = other.GetComponent<EnergyBallDestroyEffectScript>();
            EnergyBallDestroyEffectScript destroyEffect2 = GetComponent<EnergyBallDestroyEffectScript>();
            destroyEffect2.OnInstanceDestroy(gameObject);
            destroyEffect.OnInstanceDestroy(other.gameObject);

            gameObject.SetActive(false);
            other.gameObject.SetActive(false); // Désactive aussi l'autre boule
        } else if(other.gameObject.CompareTag("EnergyBall2"))
        {
            EnergyBallDestroyEffectScript destroyEffect = other.GetComponent<EnergyBallDestroyEffectScript>();
            EnergyBallDestroyEffectScript destroyEffect2 = GetComponent<EnergyBallDestroyEffectScript>();
            destroyEffect2.OnInstanceDestroy(gameObject);
            destroyEffect.OnInstanceDestroy(other.gameObject);
            gameObject.SetActive(false);
        } else if(other.gameObject.tag == "Ground")
        {
            EnergyBallDestroyEffectScript destroyEffect2 = GetComponent<EnergyBallDestroyEffectScript>();
            destroyEffect2.OnInstanceDestroy(gameObject);
            gameObject.SetActive(false);
        }
    }


}
