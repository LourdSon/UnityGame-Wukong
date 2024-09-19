using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusScript : MonoBehaviour
{
    
    public void TryAgain()
    {
        Physics2D.gravity = new Vector2(0,-9.81f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
