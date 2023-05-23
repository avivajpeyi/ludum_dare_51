using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Special2dPlayerController;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private ScriptableStats _stats;
    bool _godMode;
    private GameManager gm;
    private SpriteRenderer _mySpriteRenderer;
    float _damageCooldown;
    bool _justTookDamage;
    private HealthUI _ui;
    
    

    public static int MaxHealth { get; private set; }


    static public int CurrentHealth { get; private set; }

    Coroutine _damageCooldownCoroutine;

    private void Start()
    {
        _mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _damageCooldown = _stats.DamageCooldown;
        _godMode = false;
        ResetHealth();
        gm = GameManager.Instance;
        _ui = FindObjectOfType<HealthUI>();
        _ui.UpdateStr(CurrentHealth, MaxHealth);
    }

    private void ResetHealth()
    {
        MaxHealth = _stats.MaxHealth;
        CurrentHealth = _stats.MaxHealth;
        if (_justTookDamage)
            StopCoroutine(_damageCooldownCoroutine);
        _justTookDamage = false;
        ResetSpriteRender();
    }

    private void OnEnable()
    {
        GameManager.OnPlayerTakeDamage += TakeDamage;
        GameManager.OnToggleGodMode += ToggleGodMode;
        GameManager.OnBeforeStateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerTakeDamage -= TakeDamage;
        GameManager.OnToggleGodMode -= ToggleGodMode;
        GameManager.OnBeforeStateChanged -= OnStateChanged;
    }


    void OnStateChanged(GameState oldS, GameState newS)
    {
        if (newS == GameState.StartingGame) ResetHealth();
    }

    void ToggleGodMode() => _godMode = !_godMode;


    private void TakeDamage()
    {
        if (_godMode || _justTookDamage) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - 1);
        _ui.UpdateStr(CurrentHealth, MaxHealth);
        _justTookDamage = true;
        Instantiate(_stats.HurtEffect, transform.position, Quaternion.identity);
        if (CurrentHealth == 0)
        {
            Die();
            Instantiate(_stats.DeathEffect, transform.position, Quaternion.identity);
        }
        else
        {
            _damageCooldownCoroutine = StartCoroutine(DamageCooldown());
            FlickerSpriteRender();
        }
    }

    private void FlickerSpriteRender(int numTimes = 5)
    {
        float flickerDuration = _damageCooldown / numTimes;
        _mySpriteRenderer.DOFade(0f, flickerDuration).SetLoops(numTimes, LoopType.Yoyo)
        .SetEase(Ease.Flash);
    }

    private void ResetSpriteRender()
    {
        _mySpriteRenderer.DOKill();
        _mySpriteRenderer.DOFade(1f, 0.1f);
    }

    IEnumerator DamageCooldown()
    {
        yield return Helpers.GetWait(_damageCooldown);
        _justTookDamage = false;
        ResetSpriteRender();
    }

    public void Die() => gm.ChangeState(GameState.Lose);
}