using UnityEngine;
using UnityEngine.UI;

public class DirectionIndicator : MonoBehaviour
{
    public Transform player;             // Référence au joueur
    public Image arrowUI;                // Image de la flèche dans l'UI
    public float detectionRadius = 10f;  // Rayon de détection autour du joueur
    public string ufoIdentifier = "Nest"; // Identifiant des UFOs (nom ou autre critère)
    public PlayerMovement playerMovement;

    private Transform target;            // Référence à la cible actuelle (PNJ ou UFO)
    private GameObject currentTarget;    // Object courant (PNJ ou UFO)

    void Update()
    {
        // Récupérer la cible active en fonction du contexte
        UpdateTarget();

        if (currentTarget != null && player != null)
        {
            float distanceToTarget = Vector2.Distance(player.position, currentTarget.transform.position);

            // Affiche la flèche si la cible est hors du rayon de détection
            if (distanceToTarget > detectionRadius)
            {
                arrowUI.enabled = true;

                // Calcul de la direction et rotation de la flèche
                Vector2 direction = (currentTarget.transform.position - player.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                arrowUI.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                // Désactiver la flèche si la cible est proche
                arrowUI.enabled = false;
            }
        }
        else
        {
            // Désactiver la flèche si aucune cible n'est trouvée
            arrowUI.enabled = false;
        }
    }

    private void UpdateTarget()
    {
        // Priorité 1 : Trouver un PNJ
        currentTarget = GameObject.FindGameObjectWithTag("NPCs");

        // Si aucun PNJ n'est actif, chercher un UFO parmi les ennemis
        if (currentTarget == null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            currentTarget = FindSpecificEnemy(enemies, ufoIdentifier);
        }
    }

    private GameObject FindSpecificEnemy(GameObject[] enemies, string identifier)
    {
        foreach (GameObject enemy in enemies)
        {
            // Vérifie si le nom contient l'identifiant "UFO"
            if (enemy.name.Contains(identifier))
            {
                return enemy; // Retourne le premier match
            }

            // OU vérifie un composant spécifique pour les UFOs
            // Exemple : `UFOComponent` est un script attaché uniquement aux UFOs
            // if (enemy.GetComponent<UFOComponent>() != null)
            // {
            //     return enemy;
            // }
        }
        return null; // Aucun UFO trouvé
    }
}
