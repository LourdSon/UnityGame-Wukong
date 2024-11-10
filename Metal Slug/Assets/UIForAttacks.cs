using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIForAttacks : MonoBehaviour
{
    public GameObject imageAttack1, imageAttack2, imageAttack3;
    public PlayerAttack playerAttack;
    public PlayerShooting playerShooting;
    private Color tempColor, currentColor, newColor;
    public float transparence;
    private float timer1, timer2, timer3;
    

    void Start()
    {
        timer1 = playerAttack.timeBtwAttacksSamourai;
        timer2 = playerShooting.timeBtwAttacks3;
        timer3 = playerAttack.timeBtwAttacksPique;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Gamepad.current != null)
        {
            imageAttack1.SetActive(true);
            imageAttack2.SetActive(true);
            imageAttack3.SetActive(true);
        } else 
        {
            imageAttack1.SetActive(false);
            imageAttack2.SetActive(false);
            imageAttack3.SetActive(false);
        }

        if(imageAttack3.activeSelf /* && playerAttack.attack6 == true */ && playerAttack.attackTimeCounterPique > 0)
        {
            StartCoroutine(UIBlinking(imageAttack3, timer3));
        }
        else if(imageAttack1.activeSelf /* && playerAttack.attack7 == true */ && playerAttack.attackTimeCounterSamourai > 0)
        {
            StartCoroutine(UIBlinking(imageAttack1, timer1));
        }
        else if(imageAttack2.activeSelf /* && playerShooting.isShooting4 == true */ && playerShooting.attackTimeCounter3 > 0)
        {
            StartCoroutine(UIBlinking(imageAttack2, timer2));
        }
    }

    private IEnumerator UIBlinking(GameObject image, float timer)
    {
        image.SetActive(false);
        yield return new WaitForSeconds(timer);
        image.SetActive(true);
    }
}
