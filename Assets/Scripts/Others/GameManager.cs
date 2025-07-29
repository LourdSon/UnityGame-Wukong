using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Text gameOverText;
    public Button restartButton;

    private PlayerHealth playerHealth;

    private Vector2 defaultGravityR;

    public GameObject gameOverEventSystem;
    public GameObject EventSystem;

    public GameObject[] pauseScreen;

    public bool onPause;
    public Button pauseButton;
    public GameObject pauseEventSystem;
    public GameObject firstButton;

    void Start()
    {
        Time.timeScale = 1f;
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerHealth.OnDeath += HandleGameOver;
        gameOverScreen.SetActive(false);
        if(pauseScreen.Length != 0)
        {
            for (int i = 0; i < pauseScreen.Length; i++)
            {
                if(pauseScreen[i].activeSelf)
                    pauseScreen[i].SetActive(false);
            }
        }
        defaultGravityR = Physics2D.gravity;
        restartButton.onClick.AddListener(RestartGame);
        gameOverEventSystem.SetActive(false);
        EventSystem.SetActive(false);
        pauseEventSystem.SetActive(false);
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

    public void ResetPlayer(Vector2 defaultGravity)
    {
        Physics2D.gravity = defaultGravity;
    }

    void PauseGame()
    {
        if(pauseScreen.Length == 0)
            return;
            
        if(PlayerController.instance.playerInputActions.Player.PauseMenu.triggered && onPause == false)
        {
            Time.timeScale = 0f;
            onPause = true;
            pauseScreen[0].SetActive(true);
            EventSystem.SetActive(false);
            pauseEventSystem.SetActive(true);
            pauseEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(firstButton);
        } else if (PlayerController.instance.playerInputActions.Player.PauseMenu.triggered && onPause == true)
        {
            DepauseGame();
        }
    }

    void DepauseGame()
    {
        Time.timeScale = 1f;
        onPause = false;
        for (int i = 0; i < pauseScreen.Length; i++)
        {
            if(pauseScreen[i].activeSelf)
                pauseScreen[i].SetActive(false);
        }
        pauseEventSystem.SetActive(false);
        EventSystem.SetActive(true);
    }
}
