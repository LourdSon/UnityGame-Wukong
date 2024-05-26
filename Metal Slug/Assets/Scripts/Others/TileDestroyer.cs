using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour
{
    public Tilemap tilemap; // Référence à votre Tilemap
    public Camera mainCamera; // Référence à la caméra principale
    public TilemapCollider2D tilemapCollider2D;

    void Update()
    {
         
        
    }

    public void UpdateTilemapCollider()
    {
        tilemapCollider2D.enabled = false;
        tilemapCollider2D.enabled = true;
    }

    public void DestructionMouse()
    {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Convertit la position de la souris en position dans le monde
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position); // Convertit la position du monde en position de cellule de Tilemap
        // Détruire une zone de 3x3 tuiless
        for (int y = -3; y <= -2; y++)
        {
            
            for(int x = -7; x <= -2; x++)
            {
                
                Vector3Int currentCell = new Vector3Int((int)transform.position.x + x, (int)transform.position.y + y, cellPosition.z);
                tilemap.SetTile(currentCell, null); 
            }

            for(int c = 2;c <= 7; c++)
            {
                
                Vector3Int currentCell = new Vector3Int((int)transform.position.x + c, (int)transform.position.y + y, cellPosition.z);
                tilemap.SetTile(currentCell, null); 
            }
        }
        
        tilemapCollider2D.ProcessTilemapChanges(); // Met à jour le TilemapCollider2D pour refléter les changements
        UpdateTilemapCollider();
        
        
    }

    public void OGDestructionMouse()
        {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Convertit la position de la souris en position dans le monde
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position); // Convertit la position du monde en position de cellule de Tilemap
            // Détruire une zone de 3x3 tuiless
            for (int y = -3; y <= -2; y++)
            {
                
                for(int x = -2; x <= 2; x++)
                {
                    
                    Vector3Int currentCell = new Vector3Int((int)transform.position.x + x, (int)transform.position.y + y, cellPosition.z);
                    tilemap.SetTile(currentCell, null); 
                }

                 
                
            }
            
            tilemapCollider2D.ProcessTilemapChanges(); // Met à jour le TilemapCollider2D pour refléter les changements
            UpdateTilemapCollider();
            
            
        }

}
