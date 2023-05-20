using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    
    [SerializeField] public TMP_Text timeTxt;
    [SerializeField] public TMP_Text roomNumberTxt;
    [SerializeField] public TMP_Text roomMessageTxt;
    [SerializeField] public TMP_Text LargeMessageTxt;
    [SerializeField] public Animator hourglassAnimator;
    private GameEventManager _gameEventManager;
    

    const string HourglassAnimTrigger = "startTimer";
    private void Awake()
    {
        _gameEventManager = FindObjectOfType<GameEventManager>();
        hourglassAnimator.SetBool(HourglassAnimTrigger, false);
    }


    private void OnEnable()
    {
        GameEventManager.OnAfterStateChanged += OnStateChange;
    }

    private void OnDisable()
    {
        GameEventManager.OnAfterStateChanged -= OnStateChange;
    }


    void OnStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.StartingGame:
                ShowStartGameUi();
                break;
            case GameState.Lose:
                ShowEndGameUi();
                break;
            case GameState.BetweenRooms:
                ShowEndRoomTxt();
                break;
            case GameState.InRoom:
                ShowInRoomUI();
                UpdateRoomNumber();
                break;
        }
    }


    public void ShowStartGameUi()
    {
        LargeMessageTxt.text = "";
        UpdateRoomMessageTxt("Start dreaming!");
    }

    public void ShowEndGameUi()
    {
        LargeMessageTxt.text = "Sam Woke Up!\n Snooze?";
        hourglassAnimator.SetBool(HourglassAnimTrigger, false);
    }

    
    public void UpdateTime(float time)
    {
        timeTxt.text = time.ToString();
    }

    public void UpdateRoomNumber()
    {
        roomNumberTxt.text = "Room " + RoomStartTrigger.CountRooms.ToString("00");
    }


    void ShowInRoomUI()
    {
        UpdateRoomMessageTxt("Stay asleep");
        hourglassAnimator.SetBool(HourglassAnimTrigger, true);
    }

    void ShowEndRoomTxt()
    {
        UpdateRoomMessageTxt("Phew -- made it");
        hourglassAnimator.SetBool(HourglassAnimTrigger, false);
    }

    public void UpdateRoomMessageTxt(string txt)
    {
        roomMessageTxt.gameObject.SetActive(true);
        roomMessageTxt.text = txt;
        roomMessageTxt.color =
            new Color(roomMessageTxt.color.r, roomMessageTxt.color.g, roomMessageTxt.color.b, 1);
        StartCoroutine(fadeOutTxt(roomMessageTxt));
    }

    private IEnumerator fadeOutTxt(TMP_Text txt, float duration = 2f)
    {
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        txt.gameObject.SetActive(false);
    }
}