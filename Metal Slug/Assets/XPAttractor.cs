using UnityEngine;

public class XPAttractor : MonoBehaviour
{
    public float attractionSpeed = 5f; // Vitesse à laquelle l'XP est attirée

    void OnTriggerStay2D(Collider2D other)
    {
        // Vérifie si l'objet dans la zone est un objet d'XP
        if (other.CompareTag("SStoneObject"))
        {

            // Calcule la direction vers laquelle attirer l'XP
            Vector3 direction = (transform.position - other.transform.position).normalized;

            // Déplace l'XP vers le joueur
            other.transform.position += direction * attractionSpeed * Time.deltaTime;
        }
    }
}
