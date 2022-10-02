using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this tutorial was helpful: https://www.youtube.com/watch?v=qc7J0iei3BU

public class TimerController : MonoBehaviour
{
    public static TimerController instance; // so that Player class can access it

    public Text timeCounter;

    private float startTime;

    private Player player;

    private bool timerOngoing;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerOngoing = false;
    }

    public void beginTimer()
    {
        timerOngoing = true;
        startTime = Time.time;
    }

    public void endTimer()
    {
        timerOngoing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOngoing)
        {
            float t = 10f - (Time.time - startTime); // subtracting from 10 seconds

            if (t <= 0) // condition for when t = 0
            {
                this.endTimer();
                Player.instance.Die();
            }

            string seconds = "0" + (t % 60).ToString("f2") + " s"; // formatting
            timeCounter.text = seconds;
        }
    }
}
