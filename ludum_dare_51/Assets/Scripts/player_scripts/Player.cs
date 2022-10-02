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
    
    [SerializeField] private LayerMask platformsLayerMask;
    
    private Rigidbody2D rigidbody2d;
    private CapsuleCollider2D collider2d;
    private bool waitForStart;
    private bool isDead;
    
    private PlayerInput myInputHandler;
    private PlayerController myMovementController;
    private PlayerAnimator myAnimator;
    
    private HourglassWindow myHourglassWindow;
    

    private void Awake() {
        instance = this;
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        collider2d = transform.GetComponent<CapsuleCollider2D>();
        
        isDead = false;

        myHourglassWindow = FindObjectOfType<HourglassWindow>();

        myAnimator = GetComponentInChildren<PlayerAnimator>();
        myInputHandler = GetComponent<PlayerInput>();
        myMovementController = GetComponent<PlayerController>();
        DisablePlayer();
        if (debugMode)
        {
            rigidbody2d.gravityScale = 0;
            collider2d.enabled = false;
        }

    }
    
    void EnablePlayer()
    {
        
        myInputHandler.enabled = true;
        myMovementController.enabled = true;
        myAnimator.enabled = true;
    }
    
    void DisablePlayer()
    {
        waitForStart = true;
        myInputHandler.enabled = false;
        myMovementController.enabled = false;
        myAnimator.enabled = false;
    }

    private void Update() {
        if (isDead) return;
        if (waitForStart) {
            if (Input.anyKeyDown)
            {
                if (!debugMode) EnablePlayer();
                if (myHourglassWindow != null) myHourglassWindow.StartTimer();
                waitForStart = false;
            }
        } 
        else
        {
            
            if (debugMode)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                rigidbody2d.velocity = new Vector2(horizontalInput * 40, 
                    verticalInput * 40);
            }
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
        // instantiate death fx
        Instantiate(deathFx, transform.position, Quaternion.identity);
        // loop over all child sprite renderers and disable them
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>()) {
            spriteRenderer.enabled = false;
        }
        DisablePlayer();
        
        GameOverWindow.Show();
    }

    
}
