using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    public bool activeStopwatch = false;
    public float currentTime;
    public TMP_Text timeDisplay;

    public GameManager gameManager;

    private void Start()
    {
        currentTime = 0;
    }

    private void Update()
    {
        if (activeStopwatch)
        {
            //start stopwatch + display
            currentTime += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            timeDisplay.text = time.ToString(@"mm\:ss\:fff");
        }
        else
        {
            //reset stopwatch
            currentTime = 0;
        }
    }

    public void StartStopWatch()
    {
        activeStopwatch = true;
    }

    public void StopStopWatch()
    {
        activeStopwatch = false;
        //gameManager.instanceTime = currentTime;
    }
}
