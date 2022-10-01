using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HourglassWindow : MonoBehaviour {

    private static HourglassWindow instance;
    private static  Text timeText;
    private static int timeLeft = 10;

    private void Awake() {
        instance = this;
        timeText = transform.Find("timeText").GetComponent<Text>();
        SetTimerTxt();
    }

    public void StartTimer(){
        StartCoroutine(DecreaseTime());
    }
    
    // Decrease time every second
    public static IEnumerator DecreaseTime()
    {
        while (timeLeft > 0)
        {
            timeLeft--;
            SetTimerTxt();
            yield return new WaitForSeconds(1);
        }
    }

    // Set time text
    private static void SetTimerTxt()
    {
        timeText.text = timeLeft.ToString();
    }
    
    public static void ResetTimer() {
        timeLeft = 10;
        SetTimerTxt();
    }

}
