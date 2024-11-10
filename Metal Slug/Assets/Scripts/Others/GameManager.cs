


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
    public GameObject optionsEventSystem;
    public GameObject optionsScreen;
    
    
    


    void Start()
    {
        Time.timeScale = 1f;
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.OnDeath += HandleGameOver;
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        if(optionsScreen != null)
        optionsScreen.SetActive(false);
        // Sauvegarder la gravité par défaut
        defaultGravityR = Physics2D.gravity;
        // Ajoute un écouteur d'événements de clic sur le bouton de redémarrage
        restartButton.onClick.AddListener(RestartGame);
        gameOverEventSystem.SetActive(false);
        EventSystem.SetActive(false);
        pauseEventSystem.SetActive(false);
        if(optionsEventSystem != null)
        optionsEventSystem.SetActive(false);
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
       PauseOnOptions();
    }

    void RestartGame()
    {
        ResetPlayer(defaultGravityR);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }


    public void ResetPlayer(Vector2 defaultGravity)
    {
        // Réinitialiser la gravité du joueur à sa valeur initiale
        Physics2D.gravity = defaultGravity;
    }


    void PauseGame()
    {
        if(PlayerController.instance.playerInputActions.Player.PauseMenu.triggered && onPause == false)
        {
            Time.timeScale = 0f;
            onPause = true;
            pauseScreen.SetActive(true);
            EventSystem.SetActive(false);
            pauseEventSystem.SetActive(true);
        } else if (PlayerController.instance.playerInputActions.Player.PauseMenu.triggered && onPause == true)
        {
            DepauseGame();
        }

        if(optionsEventSystem != null && pauseScreen.activeSelf)
        {
            optionsEventSystem.SetActive(false);
            pauseEventSystem.SetActive(true);

        }
    }

    void PauseOnOptions()
    {
        if(optionsEventSystem != null && optionsScreen.activeSelf)
        {
            Time.timeScale = 0f;
            pauseEventSystem.SetActive(false);
            EventSystem.SetActive(false);
            optionsEventSystem.SetActive(true);
        } else if (optionsEventSystem != null && !optionsScreen.activeSelf)
        {
            optionsEventSystem.SetActive(false);
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
