using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MessagesUI : MonoBehaviour
{
    [SerializeField] string startStr = "Start dreaming!";
    [SerializeField] string endStr = "Sam Woke Up!\n Snooze?";

    [SerializeField] string startRoomStr = "Catch your Zs";
    [SerializeField] string endRoomStr = "Phew -- made it";


    private TMP_Text _minor;
    private TMP_Text _major;


    private void Awake()
    {
        _minor = transform.Find("minorTxt").GetComponent<TMP_Text>();
        _major = transform.Find("majorTxt").GetComponent<TMP_Text>();
        _minor.text = "";
        _major.text = "";
    }

    private void OnEnable() => GameManager.OnAfterStateChanged += OnStateChange;

    private void OnDisable() => GameManager.OnAfterStateChanged -= OnStateChange;

    void OnStateChange(GameState state)
    {
        if (state == GameState.StartingGame)
            TweenTxt(_major, startStr);
        else if (state == GameState.Lose)
            TweenTxt(_major, endStr);
        else if (state == GameState.BetweenRooms)
            TweenTxt(_minor, endRoomStr);
        else if (state == GameState.InRoom)
            TweenTxt(_minor, startRoomStr);
    }


    void TweenTxt(TMP_Text txt, string str, float duration = 2.0f)
    {
        txt.gameObject.SetActive(true);
        txt.text = str;
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
        DOTween.ToAlpha(
            () => txt.color,
            x => txt.color = x, 0, duration
        ).OnComplete(() => txt.gameObject.SetActive(false));
    }
}