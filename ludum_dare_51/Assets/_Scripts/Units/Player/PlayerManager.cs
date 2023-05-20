using System;
using System.Collections;
using System.Collections.Generic;
using Special2dPlayerController;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerManager : Singleton<PlayerManager>
{
    // TODO: move to PlayerStats
    [SerializeField] private GameObject deathFx;

    private bool _godMode = false; // for debugging
    private Rigidbody2D _rigidbody2d;
    private CapsuleCollider2D _collider2d;
    private bool _isDead;
    private PlayerInput _myInputHandler;
    private PlayerController _myMovementController;
    private PlayerAnimator _myAnimator;


    protected override void Awake()
    {
        base.Awake();
        SetInitialReferences();
        DisablePlayer(); // disable until the game manager starts game
    }

    private void OnEnable()
    {
        GameEventManager.OnAfterStateChanged += OnGameStateChange;
        GameEventManager.OnToggleGodMode += ToggleGodMode;
    }

    private void OnDisable()
    {
        GameEventManager.OnAfterStateChanged -= OnGameStateChange;
        GameEventManager.OnToggleGodMode -= ToggleGodMode;
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
        else
            DisablePlayer();

    }

    void SetInitialReferences()
    {
        _isDead = false;
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<CapsuleCollider2D>();
        _myAnimator = GetComponentInChildren<PlayerAnimator>();
        _myInputHandler = GetComponent<PlayerInput>();
        _myMovementController = GetComponent<PlayerController>();
    }

    public void ToggleGodMode()
    {
        Instantiate(deathFx, transform.position, Quaternion.identity);
        if (_godMode)
        {
            _TurnOffGodMode();
        }
        else
        {
            _TurnOnGodMode();
        }
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
        if (_isDead) return;


        if (_godMode)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            _rigidbody2d.velocity = new Vector2(horizontalInput * 40,
                verticalInput * 40);
        }
    }


    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Die()
    {
        // TODO: move to the handing of this to PlayerHealth
        
        if (_godMode) return;

        Debug.Log("Player died");
        _isDead = true;
        _rigidbody2d.velocity = Vector3.zero;
        Instantiate(deathFx, transform.position, Quaternion.identity);
        // loop over all child sprite renderers and disable them
        foreach (SpriteRenderer spriteRenderer in
                 GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.enabled = false;
        }

        DisablePlayer();
        GameEventManager.Instance.ChangeState(GameState.Lose);
        gameObject.SetActive(false);
    }
}