using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Special2dPlayerController;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerManager : Singleton<PlayerManager>
{

    private bool _godMode; // for debugging
    private Rigidbody2D _rigidbody2d;
    private bool _isDead;
    private PlayerInput _myInputHandler;
    private PlayerController _myMovementController;
    private PlayerAnimator _myAnimator;
    private SpriteRenderer _mySpriteRenderer;
    

    protected override void Awake()
    {
        base.Awake();
        SetInitialReferences();
        DisablePlayer(); // disable until the game manager starts game
    }

    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += OnGameStateChange;
        GameManager.OnToggleGodMode += ToggleGodMode;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= OnGameStateChange;
        GameManager.OnToggleGodMode -= ToggleGodMode;
    }


    private void OnGameStateChange(GameState state)
    {
        GameState[] statesToEnablePlayer =
        {
            GameState.MainMenu, GameState.StartingGame,
            GameState.InRoom, GameState.BetweenRooms
        };

        if (Array.Exists(statesToEnablePlayer, element => element == state))
            EnablePlayer();
        else if (state == GameState.Lose)
            Die();
        else
            DisablePlayer();
    }

    void SetInitialReferences()
    {
        _isDead = false;
        _godMode = false;
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponentInChildren<PlayerAnimator>();
        _myInputHandler = GetComponent<PlayerInput>();
        _myMovementController = GetComponent<PlayerController>();
        _mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void ToggleGodMode()
    {
        if (_godMode) _TurnOffGodMode();
        else _TurnOnGodMode();
    }

    void _TurnOnGodMode()
    {
        _godMode = true;
        _rigidbody2d.gravityScale = 0;
        DisablePlayer();
    }

    void _TurnOffGodMode()
    {
        _godMode = false;
        _rigidbody2d.gravityScale = 1;
        EnablePlayer();
    }


    public void EnablePlayer()
    {
        if (_godMode || _isDead) return;
        _myInputHandler.enabled = true;
        _myMovementController.enabled = true;
        _myAnimator.enabled = true;
    }

    public void DisablePlayer()
    {
        _myInputHandler.enabled = false;
        _myMovementController.enabled = false;
        _myAnimator.enabled = false;
    }

    private void Update()
    {
        if (_godMode) GodModeMovement();
    }


    private void GodModeMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        _rigidbody2d.velocity = new Vector2(
            horizontalInput * 40,
            verticalInput * 40
        );
    }

    private void Die()
    {
        Debug.Log("Player died");
        _isDead = true;
        _rigidbody2d.velocity = Vector3.zero;
        _mySpriteRenderer.enabled = false;
        

        DisablePlayer();
        gameObject.SetActive(false);
    }
}