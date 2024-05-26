using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndMission : MonoBehaviour
{


    public GameObject winScreen;
    public Text winText;
    public Button restartButton;
    private Vector2 defaultGravityR;


    // Start is called before the first frame update
    void Start()
    {
        winScreen.SetActive(false);
        // Sauvegarder la gravité par défaut
        defaultGravityR = Physics2D.gravity;
        // Ajoute un écouteur d'événements de clic sur le bouton de redémarrage
        restartButton.onClick.AddListener(RestartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RestartGame()
    {
        ResetPlayer(defaultGravityR);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    void ResetPlayer(Vector2 defaultGravity)
    {
        // Réinitialiser la gravité du joueur à sa valeur initiale
        Physics2D.gravity = defaultGravity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si le collider entrant est celui du joueur
        if (collision.CompareTag("Player"))
        {
            // Arrêter le temps du jeu
            Time.timeScale = 0f;
            winText.text = "Mission Complete";
            winScreen.SetActive(true);
        }
    }
}
