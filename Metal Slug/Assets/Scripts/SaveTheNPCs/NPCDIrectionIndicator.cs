using UnityEngine;
using UnityEngine.UI;

public class NPCDirectionIndicator : MonoBehaviour
{
    public Transform player;         // Référence au joueur
    public Transform npc;            // Référence au PNJ à sauver
    public GameObject npcfirst;
    public Image arrowUI;            // Image de la flèche dans l'UI
    public float detectionRadius = 10f;  // Rayon de détection autour du joueur
    private float distanceToNPC;
    public PlayerMovement playerMovement;
    public GameObject npcBattler;

    void Update()
    {
        npcfirst = GameObject.FindGameObjectWithTag("NPCs");
        if(npcfirst != null && player != null && playerMovement.wantToFight == true)
        {
            npc = npcfirst.GetComponent<Transform>();
            distanceToNPC = Vector2.Distance(player.position, npc.position);
        } 
        if(playerMovement.wantToFight == false && npcBattler != null)
        {
            distanceToNPC = Vector2.Distance(player.position, npcBattler.transform.position);
        }
        // Calcul de la distance entre le joueur et le PNJ
        

        if(playerMovement.wantToFight == true)
        {
            if (distanceToNPC > detectionRadius && npcfirst != null && player != null)
            {
                arrowUI.enabled = true;  // Affiche la flèche

                // Calcul de la direction du PNJ par rapport au joueur
                Vector2 direction = (npc.position - player.position).normalized;

                // Calcul de l'angle de rotation pour la flèche
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Applique la rotation à la flèche
                arrowUI.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle );
            }else
            {
                // Si le PNJ est dans le rayon de détection, désactive la flèche
                arrowUI.enabled = false;
            }
        }
        // Si le PNJ est hors du rayon de détection, affiche la flèche
        if(playerMovement.wantToFight == false)
        {
            if (distanceToNPC > detectionRadius && npcBattler != null && player != null )
            {
                arrowUI.enabled = true;  // Affiche la flèche

                // Calcul de la direction du PNJ par rapport au joueur
                Vector2 direction = (npcBattler.transform.position - player.position).normalized;

                // Calcul de l'angle de rotation pour la flèche
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Applique la rotation à la flèche
                arrowUI.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle );
            }else
            {
                // Si le PNJ est dans le rayon de détection, désactive la flèche
                arrowUI.enabled = false;
            }
        }
        

    }
}
