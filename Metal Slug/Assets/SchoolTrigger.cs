using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SchoolTrigger : MonoBehaviour
{
    public bool isAtSchool;
    public bool isInFrontOfSchool;

    void Start()
    {
        isAtSchool = false;
        isInFrontOfSchool = false;  
    }
    void Update()
    {
        if(PlayerController.instance.playerInputActions.Player.Attack.triggered && isInFrontOfSchool == true)
        {
            EnterSchool();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInFrontOfSchool = true;
            Debug.Log("Do you want to go to school ?");
        }
    }
    public void OnTriggerExit2D()
    {
        isInFrontOfSchool = false;
        isAtSchool = false;
    }
    public void EnterSchool()
    {
        isAtSchool = true;
        Debug.Log("IsAtSchool");
    }
    
}
