
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chronometer : MonoBehaviour
{
    public Text timerText;
    public float elapsedTime;
    public bool isRunning;


    private int minutes;
    private int seconds;
    private int milliseconds;
    void Start()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        minutes = Mathf.FloorToInt(elapsedTime / 60F);
        seconds = Mathf.FloorToInt(elapsedTime % 60F);
        milliseconds = Mathf.FloorToInt((elapsedTime * 1000F) % 1000F);
        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerText();
    }
}

