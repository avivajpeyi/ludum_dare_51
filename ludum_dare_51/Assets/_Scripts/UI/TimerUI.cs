using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimerUI : MonoBehaviour
{
    Animator _anim;
    TMP_Text _txt;

    const string AnimTrig = "startTimer";
    int animHash = Animator.StringToHash(AnimTrig);
    

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _txt = GetComponentInChildren<TMP_Text>();
        TurnOffHourglass();
    }


    private void TurnOnHourglass() => _anim.SetBool(animHash, true);
    private void TurnOffHourglass() => _anim.SetBool(animHash, false);


    // Controlled by RoomTimer
    public void UpdateTime(float time) => _txt.text = time.ToString();


    private void OnEnable() => GameManager.OnAfterStateChanged += OnStateChange;

    private void OnDisable() => GameManager.OnAfterStateChanged -= OnStateChange;


    void OnStateChange(GameState state)
    {
        if (state == GameState.InRoom) TurnOnHourglass();
        else TurnOffHourglass();
    }
}