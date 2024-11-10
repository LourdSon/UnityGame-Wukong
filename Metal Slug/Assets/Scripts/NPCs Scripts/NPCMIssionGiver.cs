using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMIssionGiver : MonoBehaviour
{
    public BoxCollider2D boxCollider2DTrigger;
    public Text text;
    public GameObject gameObjectParent;
    public Vector3 offset;
    public PlayerMovement playerMovement;
    public bool isListening;
    public GameObject wavenumber;
    public GameObject enemynumber;



    
    
    // Start is called before the first frame update
    void Start()
    {
        wavenumber.SetActive(false);
        enemynumber.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        text.transform.position = gameObjectParent.transform.position + offset;
        if (isListening)
        {
            wantToFightVerify();
        }
        UIWantToFight();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other == other.gameObject.CompareTag("Player") && !isListening && playerMovement.wantToFight == false)
        {
            text.text = "Do you want to fight ? Press 'Punch'";
            isListening = true;
        }else if (other == other.gameObject.CompareTag("Player") && !isListening && playerMovement.wantToFight == true)
        {
            text.text = "You're doing well... Do you want to Stop ? Press 'SuperShot'";
            isListening = true;
        }
    }
    
    public void OnTriggerExit2D()
    {
        isListening = false;
        if(playerMovement.wantToFight == false)
        {
            text.text = "No ? Maybe later then...";
        }
        if(playerMovement.wantToFight == true)
        {
            text.text = "Good luck then !";
        }
    }
    public void wantToFightVerify()
    {
        if(PlayerController.instance.playerInputActions.Player.Attack.triggered && playerMovement.wantToFight == false)
        {
            playerMovement.wantToFight = true;
            text.text = "Very well...";
        }
        if(PlayerController.instance.playerInputActions.Player.SuperShot.triggered && playerMovement.wantToFight == true)
        {
            playerMovement.wantToFight = false;
            text.text = "Ok, I'll do the rest myself...";
        }
    }

    public void UIWantToFight()
    {
        if(playerMovement.wantToFight == false)
        {
            wavenumber.SetActive(false);
            enemynumber.SetActive(false);
        } else
        {
            wavenumber.SetActive(true);
            enemynumber.SetActive(true);
        }
    }
}
