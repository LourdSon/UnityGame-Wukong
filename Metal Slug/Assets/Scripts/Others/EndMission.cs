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
    public GameObject[] enemies;
    public GameObject[] bosses;
    public GameObject boss;

    public Transform playerTransform;
    public Vector3 offset = new Vector3(2f,0f,0f);

    public Animator camAnimBoss;
    public static bool isCutsceneon;
    public bool playedOnce;



    // Start is called before the first frame update
    void Start()
    {
        winScreen.SetActive(false);
        // Sauvegarder la gravité par défaut
        defaultGravityR = Physics2D.gravity;
        // Ajoute un écouteur d'événements de clic sur le bouton de redémarrage
        restartButton.onClick.AddListener(RestartGame);
        bosses = GameObject.FindGameObjectsWithTag("Boss");
        boss.SetActive(false);
        isCutsceneon = false;
        playedOnce = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnemyNumber();
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
            YouWinScreen();
        }
    }
    public void EnemyNumber()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bosses = GameObject.FindGameObjectsWithTag("Boss");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        int count = enemies.Length;
        int countB = bosses.Length;
        
        if(!playedOnce && count == 0)
        {
            boss.SetActive(true);
            if(countB == 1)
            {
                isCutsceneon = true;
                camAnimBoss.SetBool("cutscene1", true);
                Invoke(nameof(StopCutscene), 7.30f);
                playedOnce = true;
            } 
        }else if (countB == 0 && playedOnce)
        {
            YouWinScreen();
        }
    }
    public void StopCutscene()
    {
        isCutsceneon = false;
        camAnimBoss.SetBool("cutscene1", false);
    }
    public void YouWinScreen()
    {
        // Arrêter le temps du jeu
        Time.timeScale = 0f;
        winText.text = "Mission Complete";
        winScreen.SetActive(true);
    }

}
