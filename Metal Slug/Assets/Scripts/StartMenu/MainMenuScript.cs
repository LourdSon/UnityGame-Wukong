using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    

    public int firstTime = 0;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            firstTime = 1;
            PlayerPrefs.SetInt("firstTime", firstTime);
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
