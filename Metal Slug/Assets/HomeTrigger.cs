using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeTrigger : MonoBehaviour
{
    public bool isAtHome;
    public bool isInFrontOfHome;

    void Start()
    {
        isAtHome = false;
        isInFrontOfHome = false;  
    }
    void Update()
    {
        if(PlayerController.instance.playerInputActions.Player.Attack.triggered && isInFrontOfHome == true)
        {
            EnterHome();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInFrontOfHome = true;
            Debug.Log("Do you want to enter your home ?");
        }
    }
    public void OnTriggerExit2D()
    {
        isInFrontOfHome = false;
        isAtHome = false;
    }
    public void EnterHome()
    {
        isAtHome = true;
        
    }
}
