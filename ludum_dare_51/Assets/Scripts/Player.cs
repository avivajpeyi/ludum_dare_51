using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool debugMode = false; // for debugging
    public float moveSpeed = 40f;
    public Camera mainCamera;
    Vector3 cameraPos;
    
    public float deathLine = -100f;
    
    private static Player instance;
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
        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }

        if (debugMode)
        {
            rigidbody2d.gravityScale = 0;
            boxCollider2d.enabled = false;
        }

    }

    private void Update() {
        if (isDead) return;
        if (waitForStart) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                waitForStart = false;
            }
        } 
        else
        {
            cameraFollow();
            // GroundedCheck(); done in fixed update for more frequent checks
            // deathBelowScreen();
            
            if (IsGrounded && Input.GetKeyDown(KeyCode.Space)) {
                float jumpVelocity = 100f;
                rigidbody2d.velocity = Vector2.up * jumpVelocity;
            }

            HandleMovement();
            
        }
    }

    
    // private void deathBelowScreen()
    // {
    //     // FIXME: this logic should be in the level-sections (each level section should have a death line) 
    //     int extent = 10;
    //     Vector3 leftBound = new Vector3(transform.position.x-extent, deathLine, transform.position.z);
    //     Vector3 rightBound = new Vector3(leftBound.x + (extent*2), leftBound.y, leftBound.z);
    //     Debug.DrawLine(leftBound, rightBound, Color.red);
    //     if (transform.position.y < deathLine) {
    //         Debug.Log("Player fell below screen");
    //         Die();
    //     }
    // }
    
    private void cameraFollow() 
    {
        //FIXME: This should be its own script -- and improved (eg the y follow is not great)
        
        if (mainCamera)
        {
            Vector3 targetPosition = new Vector3(transform.position.x-3, transform.position.y, cameraPos.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,  targetPosition, 0.5f);
            
        }
    }
    
    private void FixedUpdate() // runs more often than the 
    {
        GroundedCheck();
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
            rigidbody2d.velocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
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
        GameOverWindow.Show();
    }

    
}
