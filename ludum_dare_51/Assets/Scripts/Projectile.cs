using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public GameObject HitParticleFx;
    
    public bool homingMissile = false;
    public float moveSpeed = 30f;
    public float rotateSpeed = 200f;

    private Rigidbody2D rigidBody;

    private GameObject player;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
        //todo destory after 10 seconnds
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
        Debug.Log( name + " hit " + collider.gameObject.name);
        
        if (HitParticleFx != null)
            Instantiate(HitParticleFx, transform.position, Quaternion.identity); 

        Player p = collider.GetComponent<Player>();
        if (p != null)
        {
            p.Die();         
            
        }
        
        Destroy(gameObject);

    }
}
