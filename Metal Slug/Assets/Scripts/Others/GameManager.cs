


using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Text gameOverText;
    public Button restartButton;

    private PlayerHealth playerHealth;

    private Vector2 defaultGravityR; // Déplacez ici la définition de defaultGravity

    public GameObject gameOverEventSystem;
    public GameObject EventSystem;

    public GameObject pauseScreen;
    public bool onPause;
    public Button pauseButton;
    public GameObject pauseEventSystem;
    
    
    


    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.OnDeath += HandleGameOver;
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        // Sauvegarder la gravité par défaut
        defaultGravityR = Physics2D.gravity;
        // Ajoute un écouteur d'événements de clic sur le bouton de redémarrage
        restartButton.onClick.AddListener(RestartGame);
        gameOverEventSystem.SetActive(false);
        pauseEventSystem.SetActive(false);
        EventSystem.SetActive(true);
        pauseButton.onClick.AddListener(DepauseGame);
        onPause = false;
        

    }

    void HandleGameOver()
    {
        gameOverText.text = "Game Over";
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
        EventSystem.SetActive(false);
        gameOverEventSystem.SetActive(true);
        
    }

    void Update()
    {
       PauseGame();
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


    void PauseGame()
    {
        if(Input.GetButtonDown("Start"))
        {
            Time.timeScale = 0f;
            onPause = true;
            pauseScreen.SetActive(true);
            EventSystem.SetActive(false);
            pauseEventSystem.SetActive(true);
        }
    }
    
    void DepauseGame()
    {
        Time.timeScale = 1f;
        onPause = false;
        pauseScreen.SetActive(false);
        pauseEventSystem.SetActive(false);
        EventSystem.SetActive(true);
    }
}
