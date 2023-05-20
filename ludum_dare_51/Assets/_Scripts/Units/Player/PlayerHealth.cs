using System;
using System.Collections;
using System.Collections.Generic;
using Special2dPlayerController;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    [SerializeField] private ScriptableStats _stats;
    private int _currentHealth;
    bool GodMode = false;
    private GameEventManager gm;
    
    
    
    private void Start()
    {
        _currentHealth = _stats.MaxHealth;
        
    }

    private void OnEnable()
    {
        GameEventManager.OnPlayerTakeDamage += TakeDamage;
    }

    private void OnDestroy()
    {
        GameEventManager.OnPlayerTakeDamage -= TakeDamage;
    }

    public void TakeDamage()
    {
        // decrease health only if not in god mode -- but not below 0
        if (!GodMode)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - 1);
            if (_currentHealth == 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        GameEventManager.Instance.ChangeState(GameState.Lose);
    }



}
