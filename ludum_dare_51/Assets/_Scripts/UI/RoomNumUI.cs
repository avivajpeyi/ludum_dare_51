using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomNumUI : MonoBehaviour
{
    private TMP_Text _txt;

    private void Awake() => _txt = GetComponentInChildren<TMP_Text>();

    String RoomStr => RoomFactory.ActiveRoomNumber.ToString("00");
    public void UpdateRoomNumber() => _txt.text = $"Room {RoomStr}";

    private void OnEnable() => GameManager.OnAfterStateChanged += OnStateChange;
    private void OnDisable() => GameManager.OnAfterStateChanged -= OnStateChange;

    void OnStateChange(GameState state)
    {
        if (state == GameState.InRoom)
            UpdateRoomNumber();
    }
}