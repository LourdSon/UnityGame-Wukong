using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SkipCutscene : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetButtonDown("Jump"))
        {
            Physics2D.gravity = new Vector2(0,-9.81f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
