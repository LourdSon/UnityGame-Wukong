


using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour
{
    public Tilemap tilemap; // Référence à votre Tilemap
    public Camera mainCamera; // Référence à la caméra principale
    public TilemapCollider2D tilemapCollider2D;


    private PlayerHealth playerHealth;
    private Vector3 worldPoint;
    private Vector3Int cellPosition;
    private Vector3Int currentCell;
    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
    }

    public void UpdateTilemapCollider()
    {
        tilemapCollider2D.enabled = false;
        tilemapCollider2D.enabled = true;
    }

    public void DestructionMouse()
    {
        playerHealth = GetComponent<PlayerHealth>();
        worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Convertit la position de la souris en position dans le monde
        cellPosition = tilemap.WorldToCell(myTransform.position); // Convertit la position du monde en position de cellule de Tilemap
        // Détruire une zone de 3x3 tuiless
        for (int y = -3; y <= -2; y++)
        {
            
            for(int x = -7; x <= -2; x++)
            {
                
                currentCell = new Vector3Int((int)myTransform.position.x + x, (int)myTransform.position.y + y, cellPosition.z);
                tilemap.SetTile(currentCell, null); 
            }

            for(int c = 2;c <= 7; c++)
            {
                
                currentCell = new Vector3Int((int)myTransform.position.x + c, (int)myTransform.position.y + y, cellPosition.z);
                tilemap.SetTile(currentCell, null); 
            }
        }
        
        tilemapCollider2D.ProcessTilemapChanges(); // Met à jour le TilemapCollider2D pour refléter les changements
        UpdateTilemapCollider();
        
        
    }

    public void OGDestructionMouse()
        {
            playerHealth = GetComponent<PlayerHealth>();
            worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition); // Convertit la position de la souris en position dans le monde
            cellPosition = tilemap.WorldToCell(myTransform.position); // Convertit la position du monde en position de cellule de Tilemap
            // Détruire une zone de 3x3 tuiless
            for (int y = -3; y <= -2; y++)
            {
                
                for(int x = -2; x <= 2; x++)
                {
                    
                    currentCell = new Vector3Int((int)myTransform.position.x + x, (int)myTransform.position.y + y, cellPosition.z);
                    tilemap.SetTile(currentCell, null); 
                }

                 
                
            }
            
            tilemapCollider2D.ProcessTilemapChanges(); // Met à jour le TilemapCollider2D pour refléter les changements
            UpdateTilemapCollider();
            
            
        }

}

