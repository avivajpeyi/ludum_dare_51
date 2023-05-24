using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Special2dPlayerController;
using UnityEngine;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private ScriptableStats _stats;
    bool _godMode;
    private GameManager gm;
    private SpriteRenderer _mySpriteRenderer;
    private float _health;
    float _regenPerSecond = 0.2f;
    float _damageCooldown;
    bool _justTookDamage;
    private HealthUI _ui;
    Coroutine _damageCooldownCoroutine;
    Coroutine _regenCoroutine;

    public static int MaxHealth { get; private set; }


    static public int CurrentHealth => (int) Instance._health;

    

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
        _health = _stats.MaxHealth;
        if (_justTookDamage)
            StopCoroutine(_damageCooldownCoroutine);
        StopRegen();
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
        StopRegen();
        _health = Mathf.Max(0, CurrentHealth - 1);
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
            _regenCoroutine = StartCoroutine(RegenerateHealth());
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

    IEnumerator RegenerateHealth()
    {
        while (_health < MaxHealth)
        {
            yield return Helpers.GetWait(1f);
            _health = Mathf.Min(MaxHealth, _health + _regenPerSecond);
            _ui.UpdateStr(CurrentHealth, MaxHealth);
        }
    }

    void StopRegen() { if (_regenCoroutine != null) StopCoroutine(_regenCoroutine); }


    public void Die() => gm.ChangeState(GameState.Lose);
}