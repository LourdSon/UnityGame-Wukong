using UnityEngine;

public class GiveExpToPlayer : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLevel playerLevel = other.GetComponent<PlayerLevel>();
            if (playerLevel != null)
            {
                playerLevel.ExpBar();
            }
            // gameObject.SetActive(false);
            Destroy(gameObject); // Utilise cette ligne si tu veux réellement détruire l'objet
        }
    }
}
