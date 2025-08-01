using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public float maxExp = 100f;
    public float currentExp	 = 0f;
    public float expFloat = 20f;
    public int currentLvl = 1;
    public UnityEngine.UI.Image ExpBarFill;
    public Text anouncementText;
    public Text UIlvlInBar;
    public bool isLevelingUp;


    private float targetKiFillAmount;
    public PlayerScore playerScore;
    
    

    public void ExpBar()
    {
        if(currentExp < maxExp)
        {
            isLevelingUp = false;
            currentExp += expFloat;
            currentExp = Mathf.Round(currentExp);
            currentExp = Mathf.Clamp(currentExp, 0, maxExp);
            UpdateExpBar();
        } else if(currentExp >= maxExp && !isLevelingUp)
        {
            playerScore.playerLevelMax += 1;
            isLevelingUp = true;
            anouncementText.text = "Level " + currentLvl + " !";
            UIlvlInBar.text = "Lvl." + currentLvl;

            currentLvl += 1;
             // Calcul du surplus d'exp restant après le niveau up
            currentExp = currentExp - maxExp;
            maxExp += maxExp/10;
            maxExp = Mathf.Round(maxExp);
            // currentExp += expFloat;
            // currentExp = Mathf.Round(currentExp);
            currentExp = Mathf.Clamp(currentExp, 0, maxExp);
            UpdateExpBar();
            
        }
        
    }
    public void UpdateExpBar()
    {
        targetKiFillAmount = currentExp / maxExp;
        ExpBarFill.fillAmount = targetKiFillAmount;
    }
}
