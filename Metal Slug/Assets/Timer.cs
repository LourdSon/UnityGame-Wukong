using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Temps total en secondes
    public float startTime = 60f;
    private float currentTime;
    public Text timerText;
    public Text bonusTimerText;
    private int minutes, seconds;
    public PlayerHealth playerHealth;
    

    private void Start()
    {
        currentTime = startTime;
        bonusTimerText.text = "";
    }

    private void Update()
    {
        // Vérifie si le temps est écoulé
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay(currentTime);
        }
        else
        {
            currentTime = 0;
            if(playerHealth != null)
            playerHealth.Die();
            UpdateTimerDisplay(currentTime);
        }
    }

    private void UpdateTimerDisplay(float time)
    {
        if (timerText != null)
        {
            minutes = Mathf.FloorToInt(time / 60);
            seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
        }
    }

    private IEnumerator UpdateBonusTimer(int bonusSeconds)
    {
        if(bonusTimerText != null)
        {
            bonusTimerText.text = "+" + bonusSeconds + "s";
            yield return new WaitForSeconds(0.8f);
            bonusTimerText.text = "";
            yield return null;
        }
    }

    public IEnumerator bonusTimer(int bonusSeconds)
    {
        currentTime += bonusSeconds;
        StartCoroutine(UpdateBonusTimer(bonusSeconds));
        UpdateTimerDisplay(currentTime); 
        // bonusSeconds = 0;
        yield return null;
    }
}
