using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveableNPC : MonoBehaviour
{
    public float saveTimeRequired = 5f;  // Temps que le joueur doit passer dans la zone
    private bool playerInZone = false;
    public float currentSaveTime = 0f;
    
    public Slider saveIndicator; // Pour indiquer visuellement le processus de sauvegarde
    public bool pnjSaved = false;

    void Start()
    {
        if (saveIndicator != null)
        {
            saveIndicator.maxValue = saveTimeRequired;  // Définit la valeur max du slider
            saveIndicator.value = 0f;  // Réinitialise le slider
        }
    }

    void Update()
    {
        if (playerInZone)
        {
            currentSaveTime += Time.deltaTime;

            // Mettre à jour un indicateur visuel de progression (facultatif)
            if (saveIndicator != null)
            {
                saveIndicator.value = currentSaveTime;
            }

            if (currentSaveTime >= saveTimeRequired)
            {
                SaveNPC();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("Player is here");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            currentSaveTime = 0f;  // Réinitialise le temps de sauvegarde si le joueur quitte la zone
            if (saveIndicator != null)
            {
                saveIndicator.value = 0f; // Réinitialise la barre de progression
            }
        }
    }

    void SaveNPC()
    {
        // Actions à exécuter quand le PNJ est sauvé
        Debug.Log("PNJ Sauvé!");
        pnjSaved = true;
        // Désactive ou détruit le PNJ
        Destroy(gameObject); 
    }
}

