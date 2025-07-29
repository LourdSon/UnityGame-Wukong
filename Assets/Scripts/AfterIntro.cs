using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterIntro : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("CityLevel", LoadSceneMode.Single);
    }   
}
