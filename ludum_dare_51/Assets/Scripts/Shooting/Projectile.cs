using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float AutoDestroyTime = 10f;
    public GameObject HitParticleFx;
    public bool homingMissile = false;
    public float moveSpeed = 30f;
    public float rotateSpeed = 200f;

    private Rigidbody2D rigidBody;

    private GameObject player;

    public BulletTrailScriptableObject TrailConfig;
    protected TrailRenderer trail;
    [SerializeField] public Renderer renderer;
    private bool IsDisabling = false; // if in the process of disableing

    protected const string DISABLE_METHOD_NAME = "Disable";
    protected const string DO_DISABLE_METHOD_NAME = "DoDisable";

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        trail = GetComponent<TrailRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        IsDisabling = false;
        CancelInvoke(DISABLE_METHOD_NAME);
        ConfigureTrail();
        Invoke(DISABLE_METHOD_NAME, AutoDestroyTime); // disable after X seconds
    }

    private void ConfigureTrail()
    {
        if (trail != null && TrailConfig != null)
        {
            TrailConfig.SetupTrail(trail);
        }
    }


    void HandleHoming()
    {
        Vector2 direction = (Vector2)player.transform.position - rigidBody.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rigidBody.angularVelocity = -rotateAmount * rotateSpeed;
        rigidBody.velocity = transform.up * moveSpeed;
    }

    void HandleNormal()
    {
        rigidBody.velocity = transform.up * moveSpeed;
    }


    void Update()
    {
        if (IsDisabling) return;
        
        if (homingMissile)
        {
            HandleHoming();
        }
        else
        {
            HandleNormal();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(name + " hit " + collider.gameObject.name);

        if (HitParticleFx != null)
            Instantiate(HitParticleFx, transform.position, Quaternion.identity);

        Player p = collider.GetComponent<Player>();
        if (p != null)
        {
            p.Die();
        }

        Disable();
    }

    protected void Disable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        CancelInvoke(DO_DISABLE_METHOD_NAME);
        rigidBody.velocity = Vector3.zero;
        if (renderer != null)
        {
            renderer.enabled = false;
        }


        if (trail != null && TrailConfig != null)
        {
            
            IsDisabling = true;
            Invoke(DO_DISABLE_METHOD_NAME, TrailConfig.Time);
        }
        else
        {
            DoDisable();
        }
    }

    protected void DoDisable()
    {
        if (trail != null && TrailConfig != null)
        {
            trail.Clear();
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}