using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public bool isInTutorialText;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            playerMovement.wantToFight = false;
           
        }
    }
}
