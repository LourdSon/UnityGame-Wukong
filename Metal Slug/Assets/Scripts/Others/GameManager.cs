using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Burst.Intrinsics;
public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Text gameOverText;
    public Button restartButton;

    private PlayerHealth playerHealth;

    private Vector2 defaultGravityR; // Déplacez ici la définition de defaultGravity
    


    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.OnDeath += HandleGameOver;
        gameOverScreen.SetActive(false);
        // Sauvegarder la gravité par défaut
        defaultGravityR = Physics2D.gravity;
        // Ajoute un écouteur d'événements de clic sur le bouton de redémarrage
        restartButton.onClick.AddListener(RestartGame);
        
        

    }

    void HandleGameOver()
    {
        gameOverText.text = "Game Over";
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
        
    }

    void Update()
    {
       
    }

    void RestartGame()
    {
        ResetPlayer(defaultGravityR);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }


    //Lors du reset, chose à ignorer
    void ResetPlayer(Vector2 defaultGravity)
    {
        // Réinitialiser la gravité du joueur à sa valeur initiale
        Physics2D.gravity = defaultGravity;
    }


    
}
