using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    

    public int firstTime = 0;
    public GameObject eventSystem;
    public GameObject eventSystem2;
    public GameObject tutorialMenu;
    
    public void PlayTutorial()
    {
        Physics2D.gravity = new Vector2(0,-9.81f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
    public void PlayGame()
    {
        if(!PlayerPrefs.HasKey("firstTime"))
        {
            Physics2D.gravity = new Vector2(0,-9.81f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            firstTime = 1;
            PlayerPrefs.SetInt("firstTime", firstTime);
        } else if(PlayerPrefs.HasKey("firstTime"))
        {
            Physics2D.gravity = new Vector2(0,-9.81f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            firstTime = 1;
            PlayerPrefs.SetInt("firstTime", firstTime);
        }
        
    }

    void Update()
    {
        if(tutorialMenu.activeSelf)
        {
            eventSystem.SetActive(false);
            eventSystem2.SetActive(true);
        } else
        {
            eventSystem2.SetActive(false);
            eventSystem.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
