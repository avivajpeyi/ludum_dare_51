using System;
using System.Collections;
using System.Collections.Generic;
using Special2dPlayerController;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance;

    public GameObject deathFx;
    public bool debugMode = false; // for debugging
    private Rigidbody2D rigidbody2d;
    private CapsuleCollider2D collider2d;
    private bool isDead;
    private PlayerInput myInputHandler;
    private PlayerController myMovementController;
    private PlayerAnimator myAnimator;
    private GameEventManager _gameEventManager;

    private void OnEnable()
    {
        _gameEventManager.OnStartGame += EnablePlayer;
        _gameEventManager.OnEndGame += DisablePlayer;
    }
    
    private void OnDisable()
    {
        _gameEventManager.OnStartGame -= EnablePlayer;
        _gameEventManager.OnEndGame -= DisablePlayer;
    }

    private void Awake()
    {
        SetInitialreferences();
        DisablePlayer(); // disable until the game manager starts game
    }


    void SetInitialreferences()
    {
        instance = this;
        isDead = false;
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        collider2d = transform.GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponentInChildren<PlayerAnimator>();
        myInputHandler = GetComponent<PlayerInput>();
        myMovementController = GetComponent<PlayerController>();
        _gameEventManager = FindObjectOfType<GameEventManager>();
        if (debugMode)
        {
            rigidbody2d.gravityScale = 0;
            collider2d.enabled = false;
        }
    }

    public void EnablePlayer()
    {
        myInputHandler.enabled = true;
        myMovementController.enabled = true;
        myAnimator.enabled = true;
    }
    
    public  void DisablePlayer()
    {
        myInputHandler.enabled = false;
        myMovementController.enabled = false;
        myAnimator.enabled = false;
    }

    private void Update() {
        
        
        if (isDead) return;
        
            
        if (debugMode)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            rigidbody2d.velocity = new Vector2(horizontalInput * 40, 
                verticalInput * 40);
        }
        
    }
    
    

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void Die() {
        if (debugMode) return;
        
        Debug.Log("Player died");
        isDead = true;
        rigidbody2d.velocity = Vector3.zero;
        Instantiate(deathFx, transform.position, Quaternion.identity);
        // loop over all child sprite renderers and disable them
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>()) {
            spriteRenderer.enabled = false;
        }
        DisablePlayer();
        if (_gameEventManager!= null) _gameEventManager.RunEndGame();
        gameObject.SetActive(false);
    }

    
}
