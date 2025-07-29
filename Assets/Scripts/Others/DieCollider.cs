

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCollider : MonoBehaviour
{



    public int damage = 10;
    


    private PlayerHealth playerHealth;
    private Vector3 respawnPosition;
    

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
        if(collision.CompareTag("EnergyBall3") || collision.CompareTag("EnergyBallEnemy"))
        {
            collision.gameObject.SetActive(false);
        }
    }




    // Fonction pour réapparition du joueur
    private void RespawnPlayer(GameObject player)
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damage);
        respawnPosition = new Vector3(20f, 2f, 0f);
        player.transform.position = respawnPosition;
    }
}

