using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

[RequireComponent(typeof(LevelUI))]
public class LevelTimer : MonoBehaviour
{
    
    private int timeLeft = 10;
    private bool timeUp = false;
    private LevelUI levelUI;
    

    private void Awake()
    {
        levelUI = GetComponent<LevelUI>();
    }

    public void StartTimer(){
        StartCoroutine(DecreaseTime());
    }
    public IEnumerator DecreaseTime()
    {
        while (timeLeft > 0)
        {
            timeLeft--;
            levelUI.updateTime(timeLeft);
            yield return new WaitForSeconds(1);
        }
        levelUI.updateTime(0);
        timeUp = true;
    }


    public void TimeupFx()
    {
        // spawn particles or smth
    }
    
}
