using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCollider : MonoBehaviour
{



    public int damage = 10;
    




    // Fonction appelée lorsqu'un autre collider entre en contact avec ce collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si le collider entrant est celui du joueur
        if (collision.CompareTag("Player"))
        {
            // Réapparition du joueur à une position plus haute
            RespawnPlayer(collision.gameObject);
        }
        else if (collision.CompareTag("Enemy") || collision.CompareTag("EnergyBall"))
        {
            // Destruction des ennemis ou des boules d'énergie
            Destroy(collision.gameObject);
        }
    }




    // Fonction pour réapparition du joueur
    private void RespawnPlayer(GameObject player)
    {
        // Vous devez définir la position à laquelle le joueur réapparaîtra
        // Ici, nous supposons que le joueur réapparaîtra à la position (0, 5)
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damage);
        Vector3 respawnPosition = new Vector3(20f, 2f, 0f);
        // Déplacez le joueur à la position de réapparition
        player.transform.position = respawnPosition;
    }
}
