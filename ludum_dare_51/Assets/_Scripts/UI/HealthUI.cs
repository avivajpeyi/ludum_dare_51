using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class HealthUI : MonoBehaviour
{
    [SerializeField] public TMP_Text txt;
    private readonly string _healthChar = '\u2665'.ToString();
    
    private void Awake() => txt = GetComponent<TMP_Text>();

    public void UpdateStr(int current, int max)
    {
        int emptyHearts = max - current;
        string fillStr = _healthChar.RepeatStr(current);
        string emptyStr = _healthChar.RepeatStr(emptyHearts);
        txt.text = $"<color=white>{fillStr}</color>{emptyStr}";
    }
}