using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

[RequireComponent(typeof(GameUI))]
public class TimerManager : Singleton<TimerManager>
{
    [SerializeField] private int levelTime = 10;

    private int _timeLeft;
    private GameUI _gameUI;
    private GameEventManager _gameEventManager;
    private bool isOn = false;
    Coroutine _timerCoroutine;


    private void OnEnable()
    {
        GameEventManager.OnAfterStateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        GameEventManager.OnAfterStateChanged -= OnStateChanged;
    }

    void OnStateChanged(GameState state)
    {
        if (state == GameState.InRoom)
        {
            StartTimer();
        }
        else
        {
            StopTimer();
        }
    }

    private void Start()
    {
        _gameEventManager = GameEventManager.Instance;
        _gameUI = GetComponent<GameUI>();
    }

    public void StartTimer()
    {
        _timeLeft = levelTime;
        isOn = true;
        _timerCoroutine = StartCoroutine(DecreaseTime());
    }

    public IEnumerator DecreaseTime()
    {
        while (_timeLeft > 0 && !_gameEventManager.isGameOver)
        {
            _timeLeft--;
            _gameUI.UpdateTime(_timeLeft);
            yield return new WaitForSeconds(1);
        }

        TimerReachedEnd();
        _gameEventManager.ChangeState(GameState.BetweenRooms);
    }

    void TimerReachedEnd()
    {
        isOn = false;
        _timeLeft = 0;
        _gameUI.UpdateTime(0);
    }

    void StopTimer()
    {
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);
        TimerReachedEnd();
    }

    public void SkipTimer()
    {
        if (isOn)
        {
            Debug.Log("Room skipped");
            StopTimer();
            _gameEventManager.ChangeState(GameState.BetweenRooms);
        }
    }
}