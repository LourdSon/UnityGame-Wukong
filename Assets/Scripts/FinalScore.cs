using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{

    public PlayerScore playerScore;
    public Text enemiesKilled;
    public Text playerLevelMax;
    public Text healed;
    public Text roundsMax;
    public GameObject gameOverScreen;
    public GameObject enemiesKilledBestScore;
    public GameObject playerLevelMaxBestScore;
    public GameObject healedBestScore;
    public GameObject roundsMaxBestScore;

    public void BestScore()
    {
        if(PlayerPrefs.HasKey("enemiesKilled"))
        {
            LoadEnemiesKilled();
        } else
        {
            SetEnemiesKilled();
        }
        
        if(PlayerPrefs.HasKey("playerLevelMax"))
        {
            LoadplayerLevelMax();
        } else
        {
            SetplayerLevelMax();
        }

        if(PlayerPrefs.HasKey("healed"))
        {
            Loadhealed();
        } else
        {
            Sethealed();
        }
        
        if(PlayerPrefs.HasKey("roundsMax"))
        {
            LoadroundsMax();
        } else
        {
            SetroundsMax();
        }
    }

    public void SetEnemiesKilled()
    {
        enemiesKilled.text = playerScore.enemiesKilled.ToString();
        PlayerPrefs.SetInt("enemiesKilled", playerScore.enemiesKilled);
    }

    private void LoadEnemiesKilled()
    {
        if (playerScore.enemiesKilled > PlayerPrefs.GetInt("enemiesKilled"))
        {
            enemiesKilledBestScore.SetActive(true);
            SetEnemiesKilled();
        }
        else
        {
            enemiesKilledBestScore.SetActive(false);
            SetEnemiesKilled();
        }

    }

    public void SetplayerLevelMax()
    {
        playerLevelMax.text = playerScore.playerLevelMax.ToString();
        PlayerPrefs.SetInt("playerLevelMax", playerScore.playerLevelMax);
    }

    private void LoadplayerLevelMax()
    {
        if (playerScore.playerLevelMax > PlayerPrefs.GetInt("playerLevelMax"))
        {
            playerLevelMaxBestScore.SetActive(true);
            SetplayerLevelMax();
        }
        else
        {
            playerLevelMaxBestScore.SetActive(false);
            SetplayerLevelMax();
        }
    }

    public void Sethealed()
    {
        healed.text = playerScore.healed.ToString();
        PlayerPrefs.SetInt("healed", playerScore.healed);
    }

    private void Loadhealed()
    {
        if (playerScore.healed > PlayerPrefs.GetInt("healed"))
        {
            healedBestScore.SetActive(true);
            Sethealed();
        }
        else
        {
            healedBestScore.SetActive(false);
            Sethealed();
        }
    }

    public void SetroundsMax()
    {
        roundsMax.text = playerScore.roundsMax.ToString();
        PlayerPrefs.SetInt("roundsMax", playerScore.roundsMax);
    }

    private void LoadroundsMax()
    {
        if (playerScore.roundsMax > PlayerPrefs.GetInt("roundsMax"))
        {
            roundsMaxBestScore.SetActive(true);
            SetroundsMax();
        }
        else
        {
            roundsMaxBestScore.SetActive(false);
            SetroundsMax();
        }
    }
}
