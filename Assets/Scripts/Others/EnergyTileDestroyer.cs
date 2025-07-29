

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnergyTileDestroyer : MonoBehaviour
{
     //TilemapCollider2D tilemapCollider; // Référence au TilemapCollider2D
    private TilemapCollider2D tilemapCollider;
    private Vector3 hitPosition;
    private Vector3Int cellPosition;
    private 



    void OnTriggerEnter2D(Collider2D other)
    {

        tilemapCollider = GameObject.FindObjectOfType<TilemapCollider2D>();
        if (other.gameObject.CompareTag("Ground")) // Vérifie si la collision est avec un objet tagué "Ground"
        {

            hitPosition = other.transform.position; // Utilisez la position de l'autre collider (Ground) comme position de collision

            cellPosition = tilemapCollider.GetComponent<Tilemap>().WorldToCell(hitPosition); // Convertit la position du monde en position de cellule de Tilemap

            // Détruire une seule tuile à l'emplacement de la collision
            tilemapCollider.GetComponent<Tilemap>().SetTile(cellPosition, null);

            // Mettre à jour le TilemapCollider2D pour refléter les changements
            tilemapCollider.enabled = false;
            tilemapCollider.enabled = true;

            Destroy(gameObject); // Détruire la boule d'énergie après la collision
        }
    }
        
    
}
