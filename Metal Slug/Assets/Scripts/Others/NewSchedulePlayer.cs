using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewScheduleManager : MonoBehaviour
{
    public bool isRunning;
    public float elapsedTime;
    public Text timerText;
    public int minutes;
    public int seconds;
    public SchoolTrigger schoolTrigger;
    public GameObject schoolEntry;
    public Text AnouncementText;

    public int timeSelect;
    public int timeSpeed;
    public Vector2 schoolEntryPosition = new Vector2(492f, -6f);
    public HomeTrigger homeTrigger;
    public GameObject homeEntry;


    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
        isRunning = true;
        schoolEntry.SetActive(false);
        homeEntry.SetActive(false);
        timeSelect = 7;
    }
    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime * timeSpeed;
            UpdateTimerText();
        }
        at7hRest();
        at8hSchool();
        at1pmSchool();
        atmidnightRest();
    }

    void UpdateTimerText()
    {
        minutes = Mathf.FloorToInt(timeSelect + elapsedTime / 60F);
        seconds = Mathf.FloorToInt(elapsedTime % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (minutes == 24)
        {
            passTime(0);
        }
    }
    public void at7hRest()
    {
        if(minutes >= 7 && minutes < 8)
        {
            AnouncementText.text = "It is 7am, you have 1 hour of free time";
            schoolEntry.SetActive(false); 
        }
    }
    public void at8hSchool()
    {
        if(minutes >= 8 && minutes < 9)
        {
            schoolEntry.SetActive(true);
            AnouncementText.text = "It is 8am, you have 1 hour of School";
            if(schoolTrigger.isAtSchool)
            {
                passTime(9);
            }
        }
        if(minutes == 9)
        {
            schoolTrigger.isAtSchool = false;
            schoolTrigger.isInFrontOfSchool = false;
            schoolEntry.SetActive(false);    
            AnouncementText.text = "It is 9am, you have until 1pm to do whatever";
        }
    }

    public void at1pmSchool()
    {
        if(minutes >= 13 && minutes < 16)
        {
            schoolEntry.SetActive(true);
            AnouncementText.text = "It is 1pm, you have 3 hour of School";
            if(schoolTrigger.isAtSchool)
            {
                passTime(16);
            }
        }
        if(minutes == 16)
        {
            schoolTrigger.isAtSchool = false;
            schoolTrigger.isInFrontOfSchool = false;
            schoolEntry.SetActive(false);
            AnouncementText.text = "It is 4pm, you have until midnight to do whatever";
        }
    }

    public void atmidnightRest()
    {
        if(minutes >= 0 && minutes < 7)
        {
            homeEntry.SetActive(true);
            AnouncementText.text = "It is midnight, you need to rest";
            if(homeTrigger.isAtHome)
            {
                passTime(7);
            }
        } else
        {
            homeTrigger.isAtHome = false;
            homeTrigger.isInFrontOfHome = false;
            homeEntry.SetActive(false);
        }
    }
    public void passTime(int hourSelect)
    {
        timeSelect = hourSelect;
        elapsedTime = 0;
    }
}
