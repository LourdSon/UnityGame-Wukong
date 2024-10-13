using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveableNPC : MonoBehaviour
{
    public float saveTimeRequired = 5f;  // Temps que le joueur doit passer dans la zone
    public bool playerInZone = false;
    public float currentSaveTime = 0f;
    
    public Slider saveIndicator; // Pour indiquer visuellement le processus de sauvegarde
    public bool pnjSaved = false;

    public float currentLimitTime = 0f;
    public float timeRequired = 50f;
    public GameObject XpForPlayer;
    public Text text;

    void Start()
    {
        if (saveIndicator != null)
        {
            saveIndicator.maxValue = saveTimeRequired;  // Définit la valeur max du slider
            saveIndicator.value = 0f;  // Réinitialise le slider
        }
        currentLimitTime = timeRequired;
    }

    void Update()
    {
        currentLimitTime -= Time.deltaTime;
        UpdateTimerText();
        if (currentLimitTime <= 0f)
        {
            Destroy(gameObject);
            Debug.Log("You failed !");
        }
        if (playerInZone)
        {
            currentSaveTime += Time.deltaTime;

            // Mettre à jour un indicateur visuel de progression (facultatif)
            if (saveIndicator != null)
            {
                saveIndicator.value = currentSaveTime;
            }

            if (currentSaveTime >= saveTimeRequired && currentLimitTime > 0)
            {
                pnjSaved = true;
                SaveNPC();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
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
        Instantiate(XpForPlayer, transform.position, Quaternion.identity);
        Instantiate(XpForPlayer, transform.position, Quaternion.identity);
        Instantiate(XpForPlayer, transform.position, Quaternion.identity);
        Instantiate(XpForPlayer, transform.position, Quaternion.identity);
        Destroy(gameObject); 
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentLimitTime / 60F);
        int seconds = Mathf.FloorToInt(currentLimitTime % 60F);
        int milliseconds = Mathf.FloorToInt((currentLimitTime * 1000F) % 1000F);
        text.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}

