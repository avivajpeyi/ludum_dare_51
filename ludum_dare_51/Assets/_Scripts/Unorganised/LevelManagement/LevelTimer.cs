using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

[RequireComponent(typeof(LevelUI))]
public class LevelTimer : MonoBehaviour
{
    public int levelTime = 10;
    private int timeLeft;
    private LevelUI levelUI;
    private GameEventManager _gameEventManager;
    
    
    

    private void Awake()
    {
        _gameEventManager = FindObjectOfType<GameEventManager>();
        levelUI = GetComponent<LevelUI>();
    }

    private void OnEnable()
    {
        _gameEventManager.OnStartLevel += StartTimer;
    }
    
    private void OnDisable()
    {
        _gameEventManager.OnStartLevel -= StartTimer;
    }

    public void StartTimer()
    {
        Debug.Log("Start timer");
        timeLeft = levelTime;
        StartCoroutine(DecreaseTime());
    }
    public IEnumerator DecreaseTime()
    {
        while (timeLeft > 0 && !_gameEventManager.isGameOver)
        {
            timeLeft--;
            levelUI.updateTime(timeLeft);
            yield return new WaitForSeconds(1);
        }
        levelUI.updateTime(0);
        _gameEventManager.RunEndLevel();
    }
    
    
}
