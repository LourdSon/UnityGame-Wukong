

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCollider : MonoBehaviour
{



    public int damage = 10;
    




    // Fonction appelée lorsqu'un autre collider entre en contact avec ce collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.CompareTag("Player"))
        {
            RespawnPlayer(collision.gameObject);
        }*/
        /*else*/ 
        if (collision.CompareTag("Enemy") || collision.CompareTag("EnergyBall"))
        {
            Destroy(collision.gameObject);
        } 
        if(collision.CompareTag("EnergyBall3"))
        {
            collision.gameObject.SetActive(false);
        }
    }




    // Fonction pour réapparition du joueur
    private void RespawnPlayer(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damage);
        Vector3 respawnPosition = new Vector3(20f, 2f, 0f);
        player.transform.position = respawnPosition;
    }
}

