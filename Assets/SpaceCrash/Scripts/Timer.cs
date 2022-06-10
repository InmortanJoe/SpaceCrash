using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public string name { get; private set; }

    public bool isRunning { get; private set; }
    private DateTime startTime;
    public TimeSpan timeToFinish  { get; private set; }
    private DateTime finishTime;
    public UnityEvent TimerFinishedEvent;

    public double seconsLeft  { get; private set; }

    public int skipAmount
    {
        get
        {
            return (int) (seconsLeft / 60) * 2;
        }
    }

    public void Initialize(string processName, DateTime start, TimeSpan time)
    {
        name = processName;

        startTime = start;
        timeToFinish = time;
        finishTime = start.Add(time);

        TimerFinishedEvent = new UnityEvent();
    }

    public void StartTimer()
    {
        seconsLeft = timeToFinish.TotalSeconds;
        isRunning = true;
    }

    private void Update() 
    {
        //if the timer is working
        if (isRunning)
        {
            //if there ir time left
            if (seconsLeft > 0)
            {
                //decrease time left
                seconsLeft -= Time.deltaTime;
            }
            else
            {
                //reset seconds left
                seconsLeft = 0;
                //timer is not running
                isRunning = false;
                //timer finished - invoke event
                TimerFinishedEvent.Invoke();
            }
        }
    }

    public string DisplayTime()
    {
        string text = "";
        TimeSpan timeLeft = TimeSpan.FromSeconds(seconsLeft);

        if (timeLeft.Days != 0)
        {
            text += timeLeft.Days + "d";
            text += timeLeft.Hours + "h";
        }
        else if (timeLeft.Hours != 0)
        {
            text += timeLeft.Hours + "h";
            text += timeLeft.Minutes + "min";
        }
        else if (timeLeft.Minutes != 0)
        {
            text += timeLeft.Minutes + "min";
            text += timeLeft.Seconds + "sec";
        }
        else if (seconsLeft > 0)
        {
            text += Mathf.FloorToInt((float) seconsLeft) + "sec";
        }
        else
        {
            text = "Finished";
        }

        return text;
    }

    public void SkipTimer()
    {
        seconsLeft = 0;
        finishTime = DateTime.Now;
    }

}
