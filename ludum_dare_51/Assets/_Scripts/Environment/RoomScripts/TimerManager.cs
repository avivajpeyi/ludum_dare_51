using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;


public class TimerManager : Singleton<TimerManager>
{
    [SerializeField] private int levelTime = 10;

    private int _timeLeft;
    private TimerUI _ui;
    private GameManager _gm;
    private bool _isOn;
    Coroutine _timerCoroutine;


    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= OnStateChanged;
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

    protected override void Awake()
    {
        base.Awake();
        _ui = FindObjectOfType<TimerUI>();
    }

    private void Start()
    {
        _gm = GameManager.Instance;
    }

    public void StartTimer()
    {
        _timeLeft = levelTime;
        _isOn = true;
        _timerCoroutine = StartCoroutine(DecreaseTime());
    }

    public IEnumerator DecreaseTime()
    {
        while (_timeLeft > 0 && !_gm.isGameOver)
        {
            _timeLeft--;
            _ui.UpdateTime(_timeLeft);
            yield return Helpers.GetWait(1);
        }

        TimerReachedEnd();
        RoomFactory.Instance.TriggerFinishRoom();
    }

    void TimerReachedEnd()
    {
        _isOn = false;
        _timeLeft = 0;
        _ui.UpdateTime(0);
    }

    void StopTimer()
    {
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);
        TimerReachedEnd();
    }

    public void SkipTimer()
    {
        if (_isOn)
        {
            Debug.Log("Room skipped");
            StopTimer();
            RoomFactory.Instance.TriggerFinishRoom();
        }
    }
}