using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance;

    public GameObject deathFx;
    public bool debugMode = false; // for debugging
    public float moveSpeed = 40f;
    
    [SerializeField] private LayerMask platformsLayerMask;
    
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;
    private bool waitForStart;
    private bool isDead;
    [SerializeField] private bool IsGrounded;

    private void Awake() {
        instance = this;

        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        waitForStart = true;
        isDead = false;

        if (debugMode)
        {
            rigidbody2d.gravityScale = 0;
            boxCollider2d.enabled = false;
        }

    }

    private void Update() {
        if (isDead) return;
        if (waitForStart) {
            if (Input.anyKeyDown) {
                waitForStart = false;
                FindObjectOfType<HourglassWindow>().StartTimer();
            }
        } 
        else
        {
            GroundedCheck();
            
            
            if (IsGrounded && Input.GetKeyDown(KeyCode.Space)) {
                float jumpVelocity = 100f;
                rigidbody2d.velocity = Vector2.up * jumpVelocity;
            }

            HandleMovement();
            
        }
    }
    
    
    private void GroundedCheck()
    {
        // FIXME: this doesnt work all the time... not sure why not
        
        Vector2 center = boxCollider2d.bounds.center;
        float distanceMag = 10f;
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(center, boxCollider2d.bounds.size, 0f, Vector2.down, distanceMag, platformsLayerMask);
        IsGrounded =  raycastHit2d.collider != null;
        // Simple debug
        Color debugColor = IsGrounded ? Color.green : Color.red;
        Debug.DrawRay(center, Vector3.down * distanceMag, debugColor);
    }
    
    private void HandleMovement() {
        rigidbody2d.velocity = new Vector2(+moveSpeed, rigidbody2d.velocity.y);
        if (debugMode)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            rigidbody2d.velocity = new Vector2(horizontalInput * moveSpeed * 10, 
            verticalInput * moveSpeed);
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
        // hide player sprite
        // loop over all child sprite renderers and disable them
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>()) {
            spriteRenderer.enabled = false;
        }
        
        GameOverWindow.Show();
    }

    
}
